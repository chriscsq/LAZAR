using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenButtonBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject overlay;
    [SerializeField]
    private GameObject LoadScreen;

    public void BackToHomeScreen()
	{
        SceneManager.LoadScene("HomeScreen");
        LoadScreen.SetActive(false);

    }

    public void BeginPlayGame()
	{
        Debug.Log("Play game started");
        overlay.SetActive(false);
	}
}
