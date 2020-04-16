using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingScreenBehavior : MonoBehaviour
{
    [SerializeField]
    private ToggleGroup difficultyToggleGroup;
    private string _difficulty;
    [SerializeField]
    private SettingButtonBehavior buttonBehavior;
    [SerializeField]
    private Toggle easy, medium, hard;
    [SerializeField]
    private TMPro.TextMeshProUGUI currentDifficulty;

    
    public string LevelDifficulty {
        get
        {
            return _difficulty;
        }
        set
        {
            _difficulty = value;
        }

    }

    public string GetCurrentDifficulty() {
        return currentDifficulty.text;
    }

    /* Changes the text in Current difficulty */
    public void SetCurrentDifficulty(string difficulty)
    {
        var toggles = difficultyToggleGroup.GetComponentsInChildren<Toggle>();
        // Toggles right now has a length of 0, find out why

        switch (difficulty)
        {
            case "EasyToggle (UnityEngine.UI.Toggle)":
                currentDifficulty.text = "Current: Easy";
                break;
            case "MediumToggle (UnityEngine.UI.Toggle)":
                currentDifficulty.text = "Current: Medium";
                break;
            case "HardToggle (UnityEngine.UI.Toggle)":
                currentDifficulty.text = "Current: Hard";
                break;
            default:
                currentDifficulty.text = "Current: Easy";
                break;
        }
    }

}
