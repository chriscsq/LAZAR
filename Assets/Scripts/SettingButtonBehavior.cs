using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButtonBehavior : MonoBehaviour
{

    public GameObject HomeScreen;
    public GameObject SettingScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToHome()
    {
        SettingScreen.SetActive(false);
        HomeScreen.SetActive(true);
    }
}
