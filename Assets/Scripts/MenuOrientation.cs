using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuOrientation : MonoBehaviour
{
    
    private void Start() {
        if (Application.isMobilePlatform == true) {
            if (SceneManager.GetActiveScene().name == "Menu") {
                Screen.orientation = ScreenOrientation.Portrait;
            }
        }
    }
}
