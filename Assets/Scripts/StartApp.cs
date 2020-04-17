using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartApp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("open");
        GameObject screen = GameObject.Find("WelcomeScreen");
        screen.SetActive(false);
        GameObject overlay = GameObject.Find("InGameOverlay");
        overlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
