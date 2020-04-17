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
    [SerializeField]
    private ToggleGroup difficultyToggleGroup;
    [SerializeField]
    private HomePageBehavior settingBehavior;

    void Start() {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    // Start is called before the first frame update
    public void PlayButtonOnPress() 
    {
        HomeScreen.GetComponent<Canvas>().enabled = false;
        LoadScreen.SetActive(true);
        FindObjectOfType<SceneLoader>().LoadScene("GameScene");
    }

    public void SettingButtonOnPress() 
    {
        settingBehavior.UpdateDifficultyText();
        HomeTransform.DOAnchorPos(new Vector2(-1533, 2637.4f), 0.75f);
        /*
        HomeScreen.SetActive(false);
        settingBehavior.SetCurrentDifficulty(settingBehavior.LevelDifficulty);
        */
    }


    public void BackToHome()
    {
        HomeTransform.DOAnchorPos(new Vector2(1010, 2637.4f), 0.75f);
    }

    /* Get the values from the toggle group,
     * sets in the difficulty */

    public void LeaveSettings()
    {
        // difficulty = CurrentSelection.ToString();
        // Debug.Log(CurrentSelection.ToString());
        HomeTransform.DOAnchorPos(new Vector2(1010, 2637.4f), 0.75f);

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
