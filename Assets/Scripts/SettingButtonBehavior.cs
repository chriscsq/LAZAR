using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingButtonBehavior : MonoBehaviour
{

    [SerializeField]
    private GameObject HomeScreen;
    [SerializeField]
    private GameObject SettingScreen;
    [SerializeField]
    private GameObject SaveButton;
    private string difficulty;

    public void BackToHome()
    {
        SettingScreen.SetActive(false);
        HomeScreen.SetActive(true);
    }

    public void SaveSettings() 
    {
        /* Get the values from the toggle group, and set it in the difficulty */
    }

    public string GetDifficulty() {
        return difficulty;
    }
}
