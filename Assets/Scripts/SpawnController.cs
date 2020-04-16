using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [HideInInspector] //This bool is only for Vuforia behaviour. We don't want it visible in the editor.
    public bool showEnemy = true;

    public Transform enemyGoal;

    [SerializeField]
    private float spawnRadius;

    [SerializeField]
    private float groundBuffer = 0.01f; // Gap to leave between spawned enemies and the ground.

    // The "default" spawn point, though some random horizontal "noise" will be added to it for each individual enemy spawn.
    private Vector3 centreSpawnPoint; 

    private float timeSinceLastSpawn = 0.0f;

    private Transform thisTransform;
    private Collider thisCollider;

    private float spawnPeriod;
    private int subwaveIndex;
    private int subwaveEnemiesSpawnedSoFar;
    private SubwaveInfo[] subwaveArray;

    
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<Transform>();
        thisCollider = GetComponent<Collider>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (subwaveArray != null && subwaveIndex < subwaveArray.Length) {
            timeSinceLastSpawn += Time.deltaTime;
            
            if (timeSinceLastSpawn > spawnPeriod) {
                timeSinceLastSpawn = 0.0f;

                GameObject enemyToSpawn = subwaveArray[subwaveIndex].enemyType;

                Vector3 spawnLoc = thisTransform.position;
                if (thisCollider != null) {
                    spawnLoc = thisCollider.bounds.center + thisCollider.bounds.extents.y*thisTransform.up;
                }
                //centreSpawnPoint = spawnLoc;

                // Get enemy collider's bounding box extents
                float enemyExtents = enemyToSpawn.GetComponent<Collider>().bounds.extents.y;
                spawnLoc = spawnLoc + (groundBuffer + enemyExtents)*thisTransform.up;
                float angle = Random.Range(0.0f, 2.0f*Mathf.PI);
                spawnLoc += Random.Range(0.0f, spawnRadius) * (Mathf.Cos(angle)*thisTransform.forward + Mathf.Sin(angle)*thisTransform.right);

                var spawnedEnemy = Instantiate(enemyToSpawn, spawnLoc, Quaternion.identity);
                spawnedEnemy.transform.parent = thisTransform.parent;
                spawnedEnemy.transform.localRotation = Quaternion.identity;
                EnemyController ec = spawnedEnemy.GetComponent<EnemyController>();
                ec.enemyGoal = enemyGoal;
                //Destroy(spawnedEnemy, 25);

                if (!showEnemy) {
                    // If an object is parented to an image target after the target is lost,
                    // the mesh renderer on that object will still be enabled,
                    // even though it'll be disabled for all of the target's previous children.
                    // Thus, we have to "manually" hide the newly-added enemy...
                    spawnedEnemy.GetComponent<Renderer>().enabled = false;
                    // ... and, recursively, do the same for it's children.
                    Renderer[] renderers = GetComponentsInChildren<Renderer>(); // Recursively gets child renderers
                    foreach(Renderer r in renderers) {
                        r.enabled = false;
                    }
                }

                subwaveEnemiesSpawnedSoFar++;
                if (subwaveEnemiesSpawnedSoFar >= subwaveArray[subwaveIndex].enemyCount) {
                    subwaveEnemiesSpawnedSoFar = 0;
                    subwaveIndex++;
                }
            }
        }
    }

    public void StartWave(float period, SubwaveInfo[] subwaves) {
        spawnPeriod = period;
        subwaveIndex = 0;
        subwaveEnemiesSpawnedSoFar = 0;
        subwaveArray = subwaves;
        timeSinceLastSpawn = 0.0f;
    }
}
