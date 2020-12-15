using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{
    public float currentVolume = 1f;
    public float currentShakePower = 1f;
    public float currentBubbleAmount = 0.15f;
    public float currentBubbleCount = 1f;

    void Awake() {
        Debug.Log("Volume: " + currentVolume);
        Debug.Log("Power: " + currentShakePower);
        Debug.Log("Bubble Amount: " + currentBubbleAmount);
        Debug.Log("Bubble Count: " + currentBubbleCount);
    }

    public void VolumeChanged(float vol) {
        // Store volume changes for use by rattler
        currentVolume = vol;
        Debug.Log("Volume: " + currentVolume);
    }

    public void ShakePowerChanged(float power) {
        // Store shake power changes for use by rattler
        currentShakePower = power;
        Debug.Log("Power: " + currentShakePower);
    }

    public void BubbleAmountChanged(float amount) {
        // Store bubble amount changes for use by rattler
        currentBubbleAmount = amount;
        Debug.Log("Bubble Amount: " + currentBubbleAmount);
    }

    public void BubbleCountChanged(float count) {
        // Store shake power changes for use by rattler
        currentBubbleCount = count;
        Debug.Log("Bubble Count: " + currentBubbleCount);
    }


    public void DefaultSettings() {
        // Reset defaults
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public float GetVolume() {
        return currentVolume;
    }
}
