using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyController : MonoBehaviour
{

    public float health = 100;
    public Transform enemyGoal;
    [SerializeField]
    private float speed = 0.5f;
    [SerializeField]
    private float targetProximityThreshold = 0.005f;

    [Header("Healthbar")]
    public Image healthBar;

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

    public void TakeDamage(float amount) 
    {
        health -= amount;
        healthBar.fillAmount = health/100;

        /* Maybe we can add a death animation  here */
        if (health <= 0) 
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Chest")) 
        {
            Debug.Log("Collide");
            Treasure chest = other.gameObject.GetComponent<Treasure>();
            chest.TakeDamage(5.0f);
        }
    }
}
