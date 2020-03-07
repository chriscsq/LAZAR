using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    private LineRenderer line;
    private Transform thisTransform;

    private int LINE_TERM_DIST = 1000; // Line length if it doesn't hit anything.
    private int MAX_REC_DEPTH = 100; // Max reflection recursion depth
    
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.enabled = true;
        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // First position of the laser "line" is the ray origin.
        line.positionCount = 2;
        line.SetPosition(0, thisTransform.position);
        FireLaserRecursive(thisTransform.position, thisTransform.forward, 0);
    }

    void FireLaserRecursive(Vector3 rayStart, Vector3 rayDir, int depth) {
        if (depth > MAX_REC_DEPTH) return;

        Ray ray = new Ray(rayStart, rayDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000)) {
            line.SetPosition(depth+1, hit.point);

            if(hit.collider) {
                // Stuff for mirror hit here.
                if (hit.collider.tag == "Mirror") {
                    Debug.Log("Hit Mirror! Bzzt!");
                    Vector3 inVec = hit.point - rayStart;

                    // Use the point's normal to calculate the reflection vector.
                    Vector3 rVec = Vector3.Normalize(Vector3.Reflect(inVec, hit.normal));

                    // Since the mirror has reflected the ray, we recurse.
                    line.positionCount += 1;
                    FireLaserRecursive(hit.point + 0.001f*hit.normal, rVec, depth+1);
                }
                // Stuff for enemy hit here.
                else if (hit.collider.tag == "Enemy") {
                    //Debug.Log("Enemy hit! Kapow!");
                }
            }
        }
        else {
            // If the ray doesn't hit anything, terminate the drawn line some large distance away.
            line.SetPosition(depth+1, ray.GetPoint(LINE_TERM_DIST));
        }
        return;
    }
}
