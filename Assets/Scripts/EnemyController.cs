using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [System.Serializable]
    public struct PowerHealthPairing {
        public LaserPowers power;
        public float healthImpact;    
    }
    // Unity does not expose Dictionaries in the inspector.
    // Therefore, the following workaround is used: https://answers.unity.com/questions/642431/dictionary-in-inspector.html
    private Dictionary<LaserPowers, float> healthDeclineRatesDict;

    [SerializeField]
    public PowerHealthPairing[] healthDeclineRates;
    public float health = 100;

    [HideInInspector] //This Transform is never set directly by the designer; just by the spawn point
    public Transform enemyGoal;
    /*[SerializeField]
    private float speed = 0.5f;*/
    [SerializeField]
    private float targetProximityThreshold = 0.005f;

    [Header("Healthbar")]
    public Image healthBar;

    private Transform thisTransform;
    //private Transform parentTransform;
    private NavMeshAgent navMeshAgent;


    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<Transform>();
        //parentTransform = thisTransform.parent.GetComponent<Transform>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        healthDeclineRatesDict = new Dictionary<LaserPowers, float>();

        foreach(PowerHealthPairing p in healthDeclineRates) {
            healthDeclineRatesDict[p.power] = p.healthImpact;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyGoal != null) {
            navMeshAgent.SetDestination(enemyGoal.position);
        
        /*
            Vector3 locDiff = enemyGoal.localPosition - thisTransform.localPosition;
            //Vector3 travelDir = Vector3.Normalize(locDiff);
            //Vector3 nextMove = speed * Time.deltaTime * travelDir;
            

            //Vector3 newDist = enemyGoal.localPosition - thisTransform.localPosition;
            if (locDiff.magnitude < targetProximityThreshold)
                Destroy(this.gameObject);
        */
        }

        
        
        //thisTransform.Translate(nextMove, parentTransform);

    }

    public void TakeDamage(LaserPowers power) 
    {
        if (healthDeclineRatesDict.ContainsKey(power)) {
            float amount = healthDeclineRatesDict[power];
            health -= amount;
            healthBar.fillAmount = health/100;

            /* Maybe we can add a death animation  here */
            if (health <= 0) 
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Chest")) 
        {
            Debug.Log("Collide");
            Treasure chest = other.gameObject.GetComponent<Treasure>();
            chest.TakeDamage(5.0f);
            Destroy(gameObject);
        }
    }
}
