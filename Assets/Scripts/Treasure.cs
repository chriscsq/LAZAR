using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{

    [Header("Healthbar")]
    public Image healthBar;

    public float health = 100;
    [SerializeField]

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount) {
        health -= amount;
        healthBar.fillAmount = health/100;
        Debug.Log("Health is at " + health);

        if (health <= 0) 
        {
            // Game over
            Destroy(this.gameObject);
        }
    }

}
