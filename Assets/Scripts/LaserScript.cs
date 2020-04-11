using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; // for the LastOrDefault() method;
public class LaserScript : MonoBehaviour
{
    private LineRenderer line;
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
    }
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = true;
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
        bounces = FireLaserRecursive(firstBounce, 0);

        int numPositions = 2 + 3*(bounces.Count - 2);
        line.positionCount = numPositions;

        line.SetPosition(0, bounces[0].hitLocation);
        for(int bounceNum = 1, positionNum = 1; bounceNum < bounces.Count - 1; bounceNum += 1, positionNum += 3) {
            Vector3 lastHitLoc = bounces[bounceNum - 1].hitLocation;
            Vector3 currentHitLoc = bounces[bounceNum].hitLocation;
            line.SetPosition(positionNum, Vector3.MoveTowards(currentHitLoc, lastHitLoc, END_OF_SEGMENT_LENGTH));
            line.SetPosition(positionNum + 1, currentHitLoc);
            line.SetPosition(positionNum + 2, currentHitLoc);
        }
        line.SetPosition(numPositions-1, bounces.Last().hitLocation);

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
                    furtherBounces.Add(newBounce);

                    if (hit.collider.tag == "Enemy") {
                        EnemyController enemy = hit.collider.GetComponent<EnemyController>();
                        enemy.TakeDamage(0.5f);

                        
                        //Debug.Log("Enemy hit! Kapow!");
                    }
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
