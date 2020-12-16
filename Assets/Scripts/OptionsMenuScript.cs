using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuScript : MonoBehaviour
{
    // Default Settings  TODO: find a better way to do this
    private const float DEFAULTVOLUME = 1.0f;
    private const float DEFAULTSHAKEPOWER = 2.0f;
    private const float DEFAULTBUBBLEFREQUENCY = 0.15f;
    private const float BUBBLEFREQUENCYDIFFERENCE = 0.25f; // should be same as min + max
    private const float DEFAULTBUBBLECOUNT = 1.0f;
    private const float DEFAULTBUBBLESIZEVARIATION = 1.0f;
    private const float DEFAULTSPRITESIZE = 1.0f;

    public float currentVolume;
    public float currentShakePower;
    public float currentBubbleFrequency;
    public float currentBubbleCount;
    public float currentBubbleSizeVariation;
    public float currentBubbleSize;
    public float currentBlowfishSize;
    public float currentGuppySize;
    public float currentStarfishSize;

    void OnDisable() {
        // Save user options
        PlayerPrefs.SetFloat("volume", currentVolume);
        PlayerPrefs.SetFloat("shakepower", currentShakePower);
        PlayerPrefs.SetFloat("bubblefrequency", currentBubbleFrequency);
        PlayerPrefs.SetFloat("bubblecount", currentBubbleCount);
        PlayerPrefs.SetFloat("bubblevariation", currentBubbleSizeVariation);
        PlayerPrefs.SetFloat("bubblesize", currentBubbleSize);
        PlayerPrefs.SetFloat("blowfishsize", currentBlowfishSize);
        PlayerPrefs.SetFloat("guppysize", currentGuppySize);
        PlayerPrefs.SetFloat("starfishsize", currentStarfishSize);
    }

    public void VolumeChanged(float volume) {
        currentVolume = volume;
    }

    public void ShakePowerChanged(float power) {
        currentShakePower = power;
    }

    public void BubbleFrequencyChanged(float frequency) {
        currentBubbleFrequency = BUBBLEFREQUENCYDIFFERENCE - frequency;
    }

    public void BubbleCountChanged(float count) {
        currentBubbleCount = count;
    }

    public void BubbleSizeVariationChanged(float variation) {
        currentBubbleSizeVariation = variation;
    }

    public void BubbleSizeChanged(float size) {
        currentBubbleSize = size;
    }

    public void BlowfishSizeChanged(float size) {
        currentBlowfishSize = size;
    }

    public void GuppySizeChanged(float size) {
        currentGuppySize = size;
    }

    public void StarfishSizeChanged(float size) {
        currentStarfishSize = size;
    }


    public void DefaultSettings() {
        // Reset defaults and reload scene
        currentVolume = DEFAULTVOLUME;
        currentShakePower = DEFAULTSHAKEPOWER;
        currentBubbleFrequency = DEFAULTBUBBLEFREQUENCY;
        currentBubbleCount = DEFAULTBUBBLECOUNT;
        currentBubbleSizeVariation = DEFAULTBUBBLESIZEVARIATION;
        currentBubbleSize = DEFAULTSPRITESIZE;
        currentBlowfishSize = DEFAULTSPRITESIZE;
        currentGuppySize = DEFAULTSPRITESIZE;
        currentStarfishSize = DEFAULTSPRITESIZE;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
