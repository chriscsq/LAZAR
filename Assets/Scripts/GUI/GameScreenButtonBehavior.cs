using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject WelcomeScreen, InGameOverlay, MenuPanel;
    [SerializeField] GameLogic game;
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
        game.SetDifficulty();
	}

    /* Interaction with "X" on top right */
    public void MenuPanelOnClick()
    {
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
    }
}
