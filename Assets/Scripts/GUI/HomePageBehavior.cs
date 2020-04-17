using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HomePageBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject HomeScreen, LoadingScreen;
    [SerializeField]
    private ToggleGroup difficultyToggleGroup;
    [SerializeField]
    private HomeButtonBehavior buttonBehavior;
    [SerializeField]
    private Toggle easy, medium, hard;
    [SerializeField]
    private TMPro.TextMeshProUGUI currentDifficulty;


    private string _difficulty;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadingScreen.SetActive(false);

        HomeScreen.SetActive(true);
    }

    public string LevelDifficulty
    {
        get
        {
            return _difficulty;
        }
        set
        {
            _difficulty = value;
        }

    }

    /* Changes the text in Current difficulty */
    public void SetCurrentDifficulty(string difficulty)
    {
        var toggles = difficultyToggleGroup.GetComponentsInChildren<Toggle>();
        // Toggles right now has a length of 0, find out why
        Debug.Log("test");
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

    public string test()
    {
        return "test";
    }

}
