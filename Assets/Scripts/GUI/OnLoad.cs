using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject load = GameObject.Find("SceneLoader");
        load.SetActive(false);
        GameObject screen = GameObject.Find("WelcomeScreen");
        GameObject overlay = GameObject.Find("InGameOverlay");
        screen.SetActive(true);
        overlay.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
