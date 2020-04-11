using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject HomeScreen;
    [SerializeField]
    private GameObject SettingScreen;
    [SerializeField]
    private GameObject LoadScreen;
    
    // Start is called before the first frame update
    public void PlayButtonOnPress() 
    {
        HomeScreen.SetActive(false);
        LoadScreen.SetActive(true);
        FindObjectOfType<SceneLoader>().LoadScene("SampleScene");
    }

    public void SettingButtonOnPress() 
    {
        HomeScreen.SetActive(false);
        SettingScreen.SetActive(true);
    }
}
