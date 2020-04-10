﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    public float health = 100;
    public Transform enemyGoal;
    /*[SerializeField]
    private float speed = 0.5f;*/
    [SerializeField]
    private float targetProximityThreshold = 0.005f;

    [Header("Healthbar")]
    public Image healthBar;

    private Transform thisTransform;
    private Transform parentTransform;
    private NavMeshAgent navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        thisTransform = GetComponent<Transform>();
        parentTransform = thisTransform.parent.GetComponent<Transform>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyGoal != null) navMeshAgent.SetDestination(enemyGoal.position);
        
        Vector3 locDiff = enemyGoal.localPosition - thisTransform.localPosition;
        //Vector3 travelDir = Vector3.Normalize(locDiff);
        //Vector3 nextMove = speed * Time.deltaTime * travelDir;
        

        //Vector3 newDist = enemyGoal.localPosition - thisTransform.localPosition;
        if (locDiff.magnitude < targetProximityThreshold)
            Destroy(this.gameObject);
        
        //thisTransform.Translate(nextMove, parentTransform);

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
}
