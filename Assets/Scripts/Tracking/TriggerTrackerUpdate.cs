using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrackerUpdate : DefaultTrackableEventHandler
{

    
    protected override void OnTrackingFound() {
        base.OnTrackingFound();
        
        SpawnController[] spawnControllers = GetComponentsInChildren<SpawnController>();
        foreach (SpawnController sc in spawnControllers) {
            sc.showEnemy = true;
        }
    }

    protected override void OnTrackingLost() {
        base.OnTrackingLost();

        SpawnController[] spawnControllers = GetComponentsInChildren<SpawnController>();
        foreach (SpawnController sc in spawnControllers) {
            sc.showEnemy = false;
        }
    }
}
