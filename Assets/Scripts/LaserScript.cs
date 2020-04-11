using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; // for the LastOrDefault() method;
public class LaserScript : MonoBehaviour
{
    private LineRenderer thisLine;
    private List<GameObject> additionaLineObjects;
    private Transform thisTransform;

    private int LINE_TERM_DIST = 1000; // Line length if it doesn't hit anything.
    private int MAX_REC_DEPTH = 20; // Max reflection recursion depth

    private float END_OF_SEGMENT_LENGTH = 0.001f;
    private int damage = 5;
    public float updateFreq = 60.0f;

    private float timer;

    private Vector3 lastPosition;

    enum Power {DEFAULT, XTREME};

    struct LaserBounce {
        public Vector3 hitLocation;
        public Vector3 reflectionDir;
        public Power powerAfter;

        public Color beamColour;
    }
    // Start is called before the first frame update
    void Start()
    {
        thisLine = GetComponent<LineRenderer>();
        additionaLineObjects = new List<GameObject>();
        thisLine.enabled = true;
        thisTransform = GetComponent<Transform>();
        timer = 0;

        StartCoroutine(UpdateLaser());
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 1.0f/updateFreq) { // Consider also changing immediately if position relative to the mountain (world centre) changes.
            timer = 0;
            StartCoroutine(UpdateLaser());
        }

        timer += Time.deltaTime;
    }

    IEnumerator UpdateLaser() {
        List<LaserBounce> bounces = new List<LaserBounce>();
        // First position of the laser "line" is the ray origin.
        LaserBounce firstBounce = new LaserBounce();
        firstBounce.powerAfter = Power.DEFAULT;
        firstBounce.hitLocation = thisTransform.position;
        firstBounce.reflectionDir = thisTransform.forward;
        firstBounce.beamColour = Color.red; //UPDATE THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        // Get the "abstract" reflection info
        bounces = FireLaserRecursive(firstBounce, 0);


        // Once that abstract info is obtained, convert it to the LineRenderer implementation
        LineRenderer currentRenderer = thisLine;
        thisLine.positionCount = 1;
        thisLine.SetPosition(0, bounces[0].hitLocation);
        int nextAdditionalLineIndex = 0;
        int positionNum = 1;
        // Add the first line location
        for(int bounceNum = 1; bounceNum < bounces.Count - 1; bounceNum ++) {
            Vector3 lastHitLoc = bounces[bounceNum - 1].hitLocation;
            Vector3 currentHitLoc = bounces[bounceNum].hitLocation;
            Color lastCol = bounces[bounceNum - 1].beamColour;
            Color currentCol = bounces[bounceNum].beamColour;

            if (currentCol == lastCol) {
                currentRenderer.positionCount += 3;
                currentRenderer.SetPosition(positionNum, Vector3.MoveTowards(currentHitLoc, lastHitLoc, END_OF_SEGMENT_LENGTH));
                currentRenderer.SetPosition(positionNum + 1, currentHitLoc);
                currentRenderer.SetPosition(positionNum + 2, currentHitLoc);
                positionNum += 3;
            }
            else {
                // Terminate the line of the previous colour
                currentRenderer.positionCount += 1;
                currentRenderer.SetPosition(positionNum, currentHitLoc);
                // Start a new line
                if (nextAdditionalLineIndex >= additionaLineObjects.Count) {
                    GameObject newLineStart = new GameObject();
                    LineRenderer newLineRenderer = newLineStart.AddComponent(typeof(LineRenderer)) as LineRenderer;
                    Transform newLineStartTransform = newLineStart.GetComponent<Transform>();
                    newLineStartTransform.parent = thisTransform;
                    newLineStartTransform.position = thisTransform.position;
                    additionaLineObjects.Add(newLineStart);
                }
                currentRenderer = additionaLineObjects[nextAdditionalLineIndex].GetComponent<LineRenderer>();
                nextAdditionalLineIndex++;
                currentRenderer.positionCount = 1;
                positionNum = 0;
                currentRenderer.SetPosition(positionNum, currentHitLoc);
                positionNum++;
            }
        }
        // Add the very last laser position
        currentRenderer.positionCount += 1;
        currentRenderer.SetPosition(positionNum, bounces.Last().hitLocation);

        // if additionalLineObjects has more objects (and, thus, more linerenderers) than needed, we should delete the remaining ones
        if (nextAdditionalLineIndex < additionaLineObjects.Count) {
            for(int i = nextAdditionalLineIndex; i < additionaLineObjects.Count; i++) {
                Destroy(additionaLineObjects[i]);
            }
            additionaLineObjects.RemoveRange(nextAdditionalLineIndex, additionaLineObjects.Count - nextAdditionalLineIndex);
        }

        yield return new WaitForEndOfFrame();
    }

    // TODO:
    //      * Return a List of points and colours. That way, multiple lines can be drawn as needed for the power-ups.
    //      * After returning the array of points, set positionCount to its length and use SetPositions() with the array.
    //      * For the multiple colours, I think what'll need to be done is similar to that Unity Forum link with "ray splitting", but instead of using tags, I think we should just reference the other instantiations in a list.
    //          * That is, have a list called "pseudoChildren" referencing all new instantiations further down the line.
    //          * And on each update, before clearing it, see if you can just modify it.
    List<LaserBounce> FireLaserRecursive(LaserBounce startBounce, int depth) {

        List<LaserBounce> allRemainingBounces = new List<LaserBounce>();
        allRemainingBounces.Add(startBounce);

        if (depth > MAX_REC_DEPTH) return allRemainingBounces;

        Vector3 rayStart = startBounce.hitLocation;
        Vector3 rayDir = startBounce.reflectionDir;

        Ray ray = new Ray(rayStart, rayDir);
        RaycastHit hit;

        LaserBounce newBounce = new LaserBounce();

        newBounce.beamColour = Color.red; //UPDATE THIS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
                    newBounce.powerAfter = Power.DEFAULT;

                    
                    // Since the mirror has reflected the ray, we recurse.
                    furtherBounces = FireLaserRecursive(newBounce, depth+1);
                }
                // Stuff for 
                else{
                    if (hit.collider.tag == "Enemy") {
                        EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                        enemy.TakeDamage(0.5f);
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
