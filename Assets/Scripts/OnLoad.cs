using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoad : MonoBehaviour
{
    [SerializeField]
    private GameObject InGameOverlay;
    [SerializeField]
    private GameObject WelcomeScreen;

    // Start is called before the first frame update
    void Start()
    {
        WelcomeScreen.SetActive(true);
        InGameOverlay.SetActive(false);
        GameObject load = GameObject.Find("SceneLoader");
        load.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
