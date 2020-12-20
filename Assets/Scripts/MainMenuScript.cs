using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{


    public void PlayGame() {
        SceneManager.LoadScene("Rattlers");//SceneManager.GetActiveScene().buildIndex + 1);
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Menu") {
            Debug.Log("Orientation Portrait for Menu");
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // public void ExitGame() {
    //     Debug.Log ("MainMenuScript - Exiting App");
    //     Application.Quit();
    // }
}//end of MainMenuScript
