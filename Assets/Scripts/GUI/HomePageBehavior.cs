using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePageBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject HomeScreen;
    [SerializeField]
    private GameObject SettingScreen;
    [SerializeField]
    private GameObject LoadingScreen;
    // Start is called before the first frame update
    void Start()
    {
        HomeScreen.SetActive(true);
        SettingScreen.SetActive(false);
        LoadingScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
