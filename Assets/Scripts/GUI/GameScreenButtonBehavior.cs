using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject WelcomeScreen;
    [SerializeField]
    private GameObject InGameOverlay;
    [SerializeField]
    private GameObject MenuPanel;

    /* Interaction with the quit button in welcome screen */
    public void BackToHomeScreen()
	{
        SceneManager.LoadScene("HomeScreen");

    }

    /* Interaction with play button */
    public void BeginPlayGame()
	{
        WelcomeScreen.SetActive(false);
        InGameOverlay.SetActive(true);
        MenuPanel.SetActive(false);
        Screen.orientation = ScreenOrientation.AutoRotation;

	}

    /* Interaction with "X" on top right */
    public void MenuPanelOnClick()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        MenuPanel.SetActive(true);
    }

    /* Interaction with quit button in overlay */
    public void QuitGameOnClick()
    {
        SceneManager.LoadScene("HomeScreen");
    }

    /* Hide menu */
    public void BackButtonOnClick()
    {
        MenuPanel.SetActive(false);
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}
