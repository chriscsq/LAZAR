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



    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        LoadingScreen.SetActive(false);

        HomeScreen.SetActive(true);
        if(GameSettings.Difficulty == GameDifficulty.UNSET) GameSettings.Difficulty = GameDifficulty.EASY;
    }

    public void SetEasy(bool wasItSet) {
        if (wasItSet) SetCurrentDifficulty(GameDifficulty.EASY);
    }

    public void SetMedium(bool wasItSet) {
        if (wasItSet) SetCurrentDifficulty(GameDifficulty.MEDIUM);
    }
    public void SetHard(bool wasItSet) {
        if (wasItSet) SetCurrentDifficulty(GameDifficulty.HARD);
    }
    /* Changes the text in Current difficulty */
    private void SetCurrentDifficulty(GameDifficulty difficulty)
    {
        GameSettings.Difficulty = difficulty;
        UpdateDifficultyText();
    }

    public void UpdateDifficultyText() {
        switch (GameSettings.Difficulty)
        {
            case GameDifficulty.HARD:
                currentDifficulty.text = "Current: Hard";
                break;
            case GameDifficulty.MEDIUM:
                currentDifficulty.text = "Current: Medium";
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
