using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
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

    public float currentVolume = 1.0f;
    public float currentShakePower = 2.0f;
    public float currentBubbleFrequency = 0.15f;
    public float currentBubbleCount = 1.0f;
    public float currentBubbleSizeVariation = 0.5f;
    public float currentBubbleSize = 1.0f;
    public float currentBlowfishSize = 1.0f;
    public float currentGuppySize = 1.0f;
    public float currentStarfishSize = 1.0f;
    public TextMeshProUGUI volumeValue;
    public TextMeshProUGUI shakePowerValue;
    public TextMeshProUGUI bubbleFrequencyValue;
    public TextMeshProUGUI bubbleCountValue;
    public TextMeshProUGUI bubbleSizeVariationValue;
    public TextMeshProUGUI bubbleSizeValue;
    public TextMeshProUGUI blowfishSizeValue;
    public TextMeshProUGUI guppySizeValue;
    public TextMeshProUGUI starfishSizeValue;

    void Start() {
        SetPercentageText(volumeValue, currentVolume);
        SetMultiplierText(shakePowerValue, currentShakePower);
        SetFrequencyText(bubbleFrequencyValue, currentBubbleFrequency);
        SetMultiplierText(bubbleCountValue, currentBubbleCount);
        SetVariationText(bubbleSizeVariationValue, currentBubbleSizeVariation);
        SetSizeText(bubbleSizeValue, currentBubbleSize);
        SetSizeText(blowfishSizeValue, currentBlowfishSize);
        SetSizeText(guppySizeValue, currentGuppySize);
        SetSizeText(starfishSizeValue, currentStarfishSize);
    }

    void SetPercentageText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    void SetMultiplierText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = Mathf.RoundToInt(value) + "X";
    }

    void SetSizeText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = "Normal";
        if (value >= 2.3f) {
            sliderText.text = "Huge";
        } else if (value < 2.3f && value > 1.5f) {
            sliderText.text = "Big";
        } else if (value < 0.8f) {
            sliderText.text = "Small";
        }
    }

    void SetVariationText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = "Some";
        if (value >= 0.7f) {
            sliderText.text = "More";
        } else if (value > 0 && value <= 0.3f) {
            sliderText.text = "Less";
        } else if (value == 0.0f) {
            sliderText.text = "None";
        }
    }

    void SetFrequencyText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = "Normal";
        if (value >= 0.16f) {
            sliderText.text = "Less";
        } else if (value <= 0.08f) {
            sliderText.text = "More";
        }
    }

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
        SetPercentageText(volumeValue, currentVolume);
    }

    public void ShakePowerChanged(float power) {
        currentShakePower = power;
        SetMultiplierText(shakePowerValue, currentShakePower);
    }

    public void BubbleFrequencyChanged(float frequency) {
        currentBubbleFrequency = BUBBLEFREQUENCYDIFFERENCE - frequency;
        SetFrequencyText(bubbleFrequencyValue, currentBubbleFrequency);
    }

    public void BubbleCountChanged(float count) {
        currentBubbleCount = count;
        SetMultiplierText(bubbleCountValue, currentBubbleCount);
    }

    public void BubbleSizeVariationChanged(float variation) {
        currentBubbleSizeVariation = variation;
        SetVariationText(bubbleSizeVariationValue, currentBubbleSizeVariation);
    }

    public void BubbleSizeChanged(float size) {
        currentBubbleSize = size;
        SetSizeText(bubbleSizeValue, currentBubbleSize);
    }

    public void BlowfishSizeChanged(float size) {
        currentBlowfishSize = size;
        SetSizeText(blowfishSizeValue, currentBlowfishSize);
    }

    public void GuppySizeChanged(float size) {
        currentGuppySize = size;
        SetSizeText(guppySizeValue, currentGuppySize);
    }

    public void StarfishSizeChanged(float size) {
        currentStarfishSize = size;
        SetSizeText(starfishSizeValue, currentStarfishSize);
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
