using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    private bool firstRun;
    [SerializeField]
    private GameObject mainMenu = null;
    private OptionsMenuScript optionsMenuScript;
    [SerializeField]
    private GameObject aboutUs = null;

    void Awake() {
        // First run defaults to true
        firstRun = PlayerPrefs.GetString("firstrun", "true") == "true" ? true : false;
        optionsMenuScript = FindObjectOfType<OptionsMenuScript>();
        if (firstRun) {
            mainMenu.SetActive(false);
            aboutUs.SetActive(true);
            // If it is the first time since install (or cache clear) set player prefs to false
            PlayerPrefs.SetString("firstrun", "false");

            // Initialize the first run of the game
            optionsMenuScript.DefaultSettings();
        }
    }

    void OnEnable() {
        // Force portrait mode for menu
        Screen.orientation = ScreenOrientation.Portrait;
        if (firstRun) {
            optionsMenuScript.LoadPlayerPreferences();
        }
    }

    void OnDisable() {
        if (firstRun) {
            optionsMenuScript.SavePlayerPreferences();
        }
    }

    public void PlayGame() {
        //SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadScene("FishTank");
    }

    public void OpenLink() {
        Application.OpenURL("https://support.google.com/android/answer/9455138");
    }

    public void InitializeFirstRun() {
        if (firstRun) {

        }
        firstRun = false;
    }

    // public void ExitGame() {
    //     Debug.Log ("MainMenuScript - Exiting App");
    //     Application.Quit();
    // }
}//end of MainMenuScript
