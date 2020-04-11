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
    [SerializeField]
    private SettingScreenBehavior settingBehavior;


    public void BackToHome()
    {
        SettingScreen.SetActive(false);
        HomeScreen.SetActive(true);
    }

    /* Get the values from the toggle group,
     * sets in the difficulty */

    public void SaveSettings() 
    {
        settingBehavior.LevelDifficulty = CurrentSelection.ToString();
       // difficulty = CurrentSelection.ToString();
       // Debug.Log(CurrentSelection.ToString());
        SettingScreen.SetActive(false);
        HomeScreen.SetActive(true);

    }

    public string GetDifficulty {
        get
        {
            return difficulty;
        }
    }


    public Toggle CurrentSelection
    {
        get
        {
            return difficultyToggleGroup.ActiveToggles().FirstOrDefault();

        }
    }
}
