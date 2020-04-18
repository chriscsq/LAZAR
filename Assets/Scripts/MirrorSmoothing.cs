using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSmoothing : MonoBehaviour
{
    private GameObject mirror;
    private Queue previousRotations = new Queue();

    // Start is called before the first frame update
    void Start()
    {
        mirror = GameObject.FindGameObjectWithTag("MirrorTarget");
    }

    // Update is called once per frame
    void Update()
    {
        smooth(mirror.transform.localRotation);   

    }

    void smooth(Quaternion rotation)
    {
        var previousRotationLimit = 200;

        /* Adds to queue */
        if (previousRotations.Count < previousRotationLimit)
        {
            previousRotations.Enqueue(rotation);
        }
        else
        {
            previousRotations.Dequeue();
            previousRotations.Enqueue(rotation);
        }

        Quaternion average = new Quaternion(0, 0, 0, 0);

        var amount = 0;
        foreach (Quaternion singleRotation in previousRotations)
        {
            amount++;
            average = Quaternion.Slerp(average, singleRotation, 1 / amount);
        }

        mirror.transform.localRotation = average;
    }


}
