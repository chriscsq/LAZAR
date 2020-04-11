using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class SettingButtonBehavior : MonoBehaviour
{

    [SerializeField]
    private GameObject HomeScreen;
    [SerializeField]
    private GameObject SettingScreen;
    [SerializeField]
    private GameObject SaveButton;
    private string difficulty;
    [SerializeField]
    private ToggleGroup difficultyToggleGroup;

    private void Start()
    {
        Debug.Log("Difficulty is set to: " + difficulty);
    }
    public void BackToHome()
    {
        SettingScreen.SetActive(false);
        HomeScreen.SetActive(true);
    }

    public void SaveSettings() 
    {
        difficulty = CurrentSelection.ToString();
        /* Get the values from the toggle group, and set it in the difficulty */
        Debug.Log(CurrentSelection.ToString());
        SettingScreen.SetActive(false);
        HomeScreen.SetActive(true);

    }

    public string GetDifficulty() {
        return difficulty;
    }


    public Toggle CurrentSelection
    {
        get
        {
            return difficultyToggleGroup.ActiveToggles().FirstOrDefault();

        }
    }
}
