// The following script focuses the camera when deploying to Android (and possibly iPhone; currently untested).
// Without it, the camera is slightly blurry and objects do not track as well.
// Code has been copied from the "Camera Focus Modes" section of "Working with the Camera",
// an article found on the "vuforia Developer Library" site library.vuforia.com.
// Hyperlink: https://library.vuforia.com/articles/Solution/Working-with-the-Camera#Camera-Focus-Modes
// Accessed on March 17th, 2020

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia; // The "using Vuforia;" part was not mentioned in the above link, but is necessary.

public class CameraFocus : MonoBehaviour
{
    void Start ()
    {
        var vuforia = VuforiaARController.Instance;
        vuforia.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        vuforia.RegisterOnPauseCallback(OnPaused);
    }
    
    private void OnVuforiaStarted()
    {
        CameraDevice.Instance.SetFocusMode(
            CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }
    
    private void OnPaused(bool paused)
    {
        if (!paused) // resumed
        {
            // Set again autofocus mode when app is resumed
            CameraDevice.Instance.SetFocusMode(
                CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }
    }
}
