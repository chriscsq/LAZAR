using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLoad : MonoBehaviour
{

    [SerializeField]
    private GameObject welcome, overlay;
    // Start is called before the first frame update
    void Start()
    {
        GameObject load = GameObject.Find("SceneLoader");
        if(load != null) load.SetActive(false);
        welcome.SetActive(true);
        overlay.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
