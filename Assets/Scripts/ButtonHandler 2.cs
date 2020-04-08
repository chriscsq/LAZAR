using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public void NextScene() 
    {
        Debug.Log("Switching to samplescene");
        SceneManager.LoadScene("SampleScene");
    }
}
