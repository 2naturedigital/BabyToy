using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuScript : MonoBehaviour
{
    // Default Settings  TODO: find a better way to do this
    private const float DEFAULTVOLUME = 1.0f;
    private const float DEFAULTSHAKEPOWER = 2.0f;
    private const float DEFAULTBUBBLEAMOUNT = 0.15f;
    private const float BUBBLETIMERDIFFERENCE = 0.25f; // should be same as min + max
    private const float DEFAULTBUBBLECOUNT = 1.0f;

    public float currentVolume;
    public float currentShakePower;
    public float currentBubbleAmount;
    public float currentBubbleCount;

    void OnDisable() {
        // Save user options
        PlayerPrefs.SetFloat("volume", currentVolume);
        PlayerPrefs.SetFloat("shakepower", currentShakePower);
        PlayerPrefs.SetFloat("bubbleamount", currentBubbleAmount);
        PlayerPrefs.SetFloat("bubblecount", currentBubbleCount);
    }

    public void VolumeChanged(float vol) {
        // Store volume changes for use by rattler
        currentVolume = vol;
    }

    public void ShakePowerChanged(float power) {
        // Store shake power changes for use by rattler
        currentShakePower = power;
    }

    public void BubbleAmountChanged(float amount) {
        // Store bubble amount changes for use by rattler
        currentBubbleAmount = BUBBLETIMERDIFFERENCE - amount;
    }

    public void BubbleCountChanged(float count) {
        // Store shake power changes for use by rattler
        currentBubbleCount = count;
    }


    public void DefaultSettings() {
        // Reset defaults and reload scene
        currentVolume = DEFAULTVOLUME;
        currentShakePower = DEFAULTSHAKEPOWER;
        currentBubbleAmount = DEFAULTBUBBLEAMOUNT;
        currentBubbleCount = DEFAULTBUBBLECOUNT;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
