using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("Rattlers");//SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame() {
        Debug.Log ("MainMenuScript - Exiting App");
        Application.Quit();
    }
}
