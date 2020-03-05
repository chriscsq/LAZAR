using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform enemyGoal;
    [SerializeField]
    private float speed = 0.5f;
    [SerializeField]
    private float targetProximityThreshold = 0.005f;

    private Transform thisTransform;
    private Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<Transform>();
        parentTransform = thisTransform.parent.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 locDiff = enemyGoal.localPosition - thisTransform.localPosition;
        Vector3 travelDir = Vector3.Normalize(locDiff);
        Vector3 nextMove = speed * Time.deltaTime * travelDir;
        
        // If we're very close to the goal, we'll slow down.
        // This is to prevent oscillation at the goal.
        // It's not the best fix (should use lerp), but should be fine for this prototype.
        Vector3 newDist = enemyGoal.localPosition - thisTransform.localPosition;
        if (locDiff.magnitude < targetProximityThreshold)
            nextMove = locDiff.magnitude * nextMove;
        
        thisTransform.Translate(nextMove, parentTransform);

    }
}
