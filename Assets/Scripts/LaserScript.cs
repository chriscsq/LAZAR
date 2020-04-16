using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering; // for the shadow modes
using System.Linq; // for the Last() method;
public class LaserScript : MonoBehaviour
{

    private int LINE_TERM_DIST = 1000; // Line length if it doesn't hit anything.
    private int MAX_REC_DEPTH = 20; // Max reflection recursion depth
    private float UPDATE_FREQ = 60.0f; // Frequency at which the laser updates (in seconds).

    public LaserPowers startPower = LaserPowers.DEFAULT; 

    
    // The length of the mini-segment before a reflection.
    // This mini-segment is needed to make LineRenderer not twist all funnily.
    private float END_OF_SEGMENT_LENGTH = 0.001f; 

    // Hold the current LineRenderer
    private LineRenderer thisLine;
    private Transform thisTransform;

    // This List of GameObjects is to hold other laser starting points, equipped with LineRenderers.
    // This is needed for when a laser changes colour; rather than messing with colour gradients, it's easier for now to just start a new laser.
    private List<GameObject> additionaLineObjects;


    // For controlling the laser update frequency.
    private float timer;

    private PowerColourPairingInfo laserColourInfo;


    struct LaserBounce {
        public Vector3 hitLocation;
        public Vector3 reflectionDir;
        public LaserPowers powerAfter;

        public Color beamColour;
    }

    // Start is called before the first frame update.
    // For this class, we just initialize things.
    void Start()
    {
        thisLine = GetComponent<LineRenderer>();
        additionaLineObjects = new List<GameObject>();
        thisLine.enabled = true;
        thisTransform = GetComponent<Transform>();
        timer = 0;

        laserColourInfo = GameObject.FindGameObjectWithTag("GameController").GetComponent<PowerColourPairingInfo>();

        Color turretStartColour = laserColourInfo.GetAssociatedColor(startPower);
        thisLine.startColor = turretStartColour;
        thisLine.endColor = turretStartColour;

        StartCoroutine(UpdateLaser());


    }

    // Update is called once per frame.
    void Update()
    {
        if (timer >= 1.0f/UPDATE_FREQ) { // Consider also changing immediately if position relative to the mountain (world centre) changes.
            timer = 0;
            StartCoroutine(UpdateLaser());
        }

        timer += Time.deltaTime;
    }

    IEnumerator UpdateLaser() {
        List<LaserBounce> bounces = new List<LaserBounce>();
        // First position of the laser "line" is the ray origin.
        LaserBounce firstBounce = new LaserBounce();
        firstBounce.powerAfter = LaserPowers.DEFAULT; // Start with the default power.
        firstBounce.hitLocation = thisTransform.position;
        firstBounce.reflectionDir = thisTransform.forward;
        firstBounce.beamColour = laserColourInfo.GetAssociatedColor(LaserPowers.DEFAULT);


        // Get the "abstract"/"theoretical" reflection info
        bounces = FireLaserRecursive(firstBounce, 0);


        // Once that abstract info is obtained, convert it to the LineRenderer implementation
        LineRenderer currentRenderer = thisLine;
        // Add the first line location
        thisLine.positionCount = 1;
        thisLine.SetPosition(0, bounces[0].hitLocation);

        int nextAdditionalLineIndex = 0;
        int positionNum = 1;
        for(int bounceNum = 1; bounceNum < bounces.Count - 1; bounceNum ++) {
            Vector3 lastHitLoc = bounces[bounceNum - 1].hitLocation;
            Vector3 currentHitLoc = bounces[bounceNum].hitLocation;
            Color lastCol = bounces[bounceNum - 1].beamColour;
            Color currentCol = bounces[bounceNum].beamColour;

            // To get the LineRenderer not to look twisted, the technique from this link is used: https://forum.unity.com/threads/free-bouncing-laser-script.286967/#post-2761538
            // For an explanation of why the twisting occurs and why the above fix works, see: https://gamedev.stackexchange.com/questions/93823/how-to-make-line-renderer-lines-stay-flat
            if (currentCol == lastCol) {
                currentRenderer.positionCount += 3;
                currentRenderer.SetPosition(positionNum, Vector3.MoveTowards(currentHitLoc, lastHitLoc, END_OF_SEGMENT_LENGTH));
                currentRenderer.SetPosition(positionNum + 1, currentHitLoc);
                currentRenderer.SetPosition(positionNum + 2, currentHitLoc);
                positionNum += 3;
            }
            else {
                // If the current colour != the last one, we continue the laser from a new LineRenderer (paired to a new GameObject).
                // First, terminate the line of the previous colour
                currentRenderer.positionCount += 1;
                currentRenderer.SetPosition(positionNum, currentHitLoc);

                // If we can't reuse a LineRenderer object from the previous frame, create a new one.
                // Otherwise, we reuse one from before, so we're not creating and deleting a ton of objects every frame.
                // I've never experimented with creating and deleting a bunch of objects every frame... but I imagine it's not the most efficient.
                if (nextAdditionalLineIndex >= additionaLineObjects.Count) {
                    GameObject newLineStart = new GameObject();
                    LineRenderer newLineRenderer = newLineStart.AddComponent(typeof(LineRenderer)) as LineRenderer;
                    Transform newLineStartTransform = newLineStart.GetComponent<Transform>();
                    newLineStartTransform.parent = thisTransform;
                    newLineStartTransform.position = thisTransform.position;
                    additionaLineObjects.Add(newLineStart);
                    // Material handling reference: https://answers.unity.com/questions/587380/linerenderer-drawing-in-pink.html
                    // Material: Legacy/Particles/Alpha Blended Premultiply
                    Material newLaserMat = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                    newLineRenderer.material = newLaserMat;
                    

                    // Copy most properties from the original LineRenderer, but apply new colours.
                    newLineRenderer.receiveShadows = thisLine.receiveShadows;
                    newLineRenderer.shadowCastingMode = thisLine.shadowCastingMode;
                    newLineRenderer.widthCurve = new AnimationCurve(thisLine.widthCurve.keys);
                    newLineRenderer.startColor = currentCol;
                    newLineRenderer.endColor = currentCol;
                    newLineRenderer.materials = thisLine.materials;
                }

                // Set the "currentRenderer" to the new renderer.
                currentRenderer = additionaLineObjects[nextAdditionalLineIndex].GetComponent<LineRenderer>();
                // Increase the index for a potential next time if we have to do this again.
                nextAdditionalLineIndex++;

                // Add the new-coloured bounce as the first position for this new line renderer
                currentRenderer.positionCount = 1;
                positionNum = 0;
                currentRenderer.SetPosition(positionNum, currentHitLoc);
                positionNum++;
            }
        }
        // Add the very last laser position
        currentRenderer.positionCount += 1;
        currentRenderer.SetPosition(positionNum, bounces.Last().hitLocation);

        // if additionalLineObjects has more objects (and, thus, more linerenderers) than needed, we should delete the remaining ones.
        if (nextAdditionalLineIndex < additionaLineObjects.Count) {
            for(int i = nextAdditionalLineIndex; i < additionaLineObjects.Count; i++) {
                Destroy(additionaLineObjects[i].GetComponent<LineRenderer>().material);
                Destroy(additionaLineObjects[i]);
            }
            additionaLineObjects.RemoveRange(nextAdditionalLineIndex, additionaLineObjects.Count - nextAdditionalLineIndex);
        }

        yield return new WaitForEndOfFrame();
    }

    // Returns a List of LaserBounce info. This will be the "abstract" representation of the laser's bounces, their positions and colours, etc.
    List<LaserBounce> FireLaserRecursive(LaserBounce startBounce, int depth) {

        List<LaserBounce> allRemainingBounces = new List<LaserBounce>();
        allRemainingBounces.Add(startBounce);

        if (depth > MAX_REC_DEPTH){
            // Debug.Log("MAX LASER RECURSION DEPTH EXCEEDED!"); Was considering writing this to the console, but it would be every frame unless I restructure things...
            return allRemainingBounces;
        }

        Vector3 rayStart = startBounce.hitLocation;
        Vector3 rayDir = startBounce.reflectionDir;

        Ray ray = new Ray(rayStart, rayDir);
        RaycastHit hit;

        LaserBounce newBounce = new LaserBounce();

        //Set these fields the same as last bounce by default; they may get overwritten if ray hits a mirror.
        newBounce.powerAfter = startBounce.powerAfter;
        newBounce.beamColour = startBounce.beamColour; 

        List<LaserBounce> furtherBounces = new List<LaserBounce>();

        if (Physics.Raycast(ray, out hit, LINE_TERM_DIST)) {
            
            if(hit.collider) {

                newBounce.hitLocation = hit.point;
                
                // Stuff for mirror hit here.
                if (hit.collider.tag == "Mirror") {
                    
                    Vector3 inVec = hit.point - rayStart;

                    // Use the point's normal to calculate the reflection vector.
                    Vector3 rVec = Vector3.Normalize(Vector3.Reflect(inVec, hit.normal));

                    newBounce.reflectionDir = rVec;
                    MirrorInfo mirrorInfo = hit.transform.gameObject.GetComponent<MirrorInfo>();
                    if (mirrorInfo != null && mirrorInfo.replacePreviousPower) {
                        newBounce.powerAfter = mirrorInfo.reflectionPower;
                        newBounce.beamColour = laserColourInfo.GetAssociatedColor(newBounce.powerAfter);
                    }
                    
                    // Since the mirror has reflected the ray, we recurse.
                    furtherBounces = FireLaserRecursive(newBounce, depth+1);
                }
                // Stuff for 
                else{
                    if (hit.collider.tag == "Enemy") {
                        EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                        enemy.TakeDamage(newBounce.powerAfter);
                    }

                    furtherBounces.Add(newBounce);
                }
            }
        }
        else {
            // If the ray doesn't hit anything, terminate the drawn line some large distance away.
            newBounce.hitLocation = startBounce.hitLocation + startBounce.reflectionDir * LINE_TERM_DIST;
            furtherBounces.Add(newBounce);
        }
        allRemainingBounces.AddRange(furtherBounces);
        return allRemainingBounces;
    }
}
