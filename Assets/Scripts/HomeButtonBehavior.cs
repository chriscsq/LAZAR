using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;


public class HomeButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject HomeScreen, LoadScreen;

    [SerializeField]
    private RectTransform HomeTransform;

    [SerializeField]
    private GameObject SaveButton;
    private string difficulty;
    [SerializeField]
    private ToggleGroup difficultyToggleGroup;
    [SerializeField]
    private HomePageBehavior settingBehavior;

    // Start is called before the first frame update
    public void PlayButtonOnPress() 
    {
        HomeScreen.SetActive(false);
        LoadScreen.SetActive(true);
        FindObjectOfType<SceneLoader>().LoadScene("SampleScene");
    }

    public void SettingButtonOnPress() 
    {
        HomeTransform.DOAnchorPos(new Vector2(-1533, 2637.4f), 0.75f);
        /*
        HomeScreen.SetActive(false);
        settingBehavior.SetCurrentDifficulty(settingBehavior.LevelDifficulty);
        */
        settingBehavior.SetCurrentDifficulty(settingBehavior.LevelDifficulty);
        Debug.Log(difficulty);
    }


    public void BackToHome()
    {
        HomeTransform.DOAnchorPos(new Vector2(1010, 2637.4f), 0.75f);
    }

    /* Get the values from the toggle group,
     * sets in the difficulty */

    public void SaveSettings()
    {
        settingBehavior.LevelDifficulty = CurrentSelection.ToString();
        // difficulty = CurrentSelection.ToString();
        // Debug.Log(CurrentSelection.ToString());
        HomeTransform.DOAnchorPos(new Vector2(1010, 2637.4f), 0.75f);

    }

    public string GetDifficulty
    {
        get
        {
            return difficulty;
        }
    }


    public Toggle CurrentSelection
    {
        get
        {
            Debug.Log(difficultyToggleGroup.ActiveToggles().FirstOrDefault());
            return difficultyToggleGroup.ActiveToggles().FirstOrDefault();

        }
    }
}
