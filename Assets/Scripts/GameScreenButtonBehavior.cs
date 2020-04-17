using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject WelcomeScreen;
    [SerializeField]
    private GameObject LoadScreen;
    [SerializeField]
    private GameObject InGameOverlay;
    [SerializeField]
    private GameObject MenuPanel;

    public void BackToHomeScreen()
	{
        SceneManager.LoadScene("HomeScreen");
        LoadScreen.SetActive(false);

    }

    public void BeginPlayGame()
	{
        Debug.Log("Play game started");
        WelcomeScreen.SetActive(false);
        InGameOverlay.SetActive(true);
        MenuPanel.SetActive(false);
	}

    public void MenuPanelOnClick()
    {
        MenuPanel.SetActive(true);
    }

    public void QuitGameOnClick()
    {
        SceneManager.LoadScene("HomeScreen");
    }

    public void BackButtonOnClick()
    {
        MenuPanel.SetActive(false);
    }
}
