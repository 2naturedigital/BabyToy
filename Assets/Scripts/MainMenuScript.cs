﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{


    public void PlayGame() {
        SceneManager.LoadScene("Rattlers");//SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnEnable() {
        // Force portrait mode for menu
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void OpenLink() {
        Application.OpenURL("https://support.google.com/android/answer/9455138");
    }

    // public void ExitGame() {
    //     Debug.Log ("MainMenuScript - Exiting App");
    //     Application.Quit();
    // }
}//end of MainMenuScript
