using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class OptionsMenuScript : MonoBehaviour
{
    // Default Settings  TODO: find a better way to do this
    private const bool DEFAULTBLUR = true;
    private const bool DEFAULTLANDSCAPE = false;
    private const float DEFAULTVOLUME = 1.0f;
    private const float DEFAULTSHAKEPOWER = 2.0f;
    private const float DEFAULTBUBBLEFREQUENCY = 4.0f;
    private const float BUBBLEFREQUENCYDIFFERENCE = 9.0f; // should be same as min + max
    private const float DEFAULTSHAKENBUBBLEFREQUENCY = 0.15f;
    private const float SHAKENBUBBLEFREQUENCYDIFFERENCE = 0.25f; // should be same as min + max
    private const float DEFAULTBUBBLECOUNT = 1.0f;
    private const float DEFAULTBUBBLESIZEVARIATION = 0.5f;
    private const float DEFAULTSPRITESIZE = 1.0f;

    public bool currentBlur = DEFAULTBLUR;
    public bool orientation = DEFAULTLANDSCAPE;
    public float currentVolume = DEFAULTVOLUME;
    public float currentShakePower = DEFAULTSHAKEPOWER;
    public float currentBubbleFrequency = DEFAULTBUBBLEFREQUENCY;
    public float currentShakenBubbleFrequency = DEFAULTSHAKENBUBBLEFREQUENCY;
    public float currentBubbleCount = DEFAULTBUBBLECOUNT;
    public float currentBubbleSizeVariation = DEFAULTBUBBLESIZEVARIATION;
    public float currentBubbleSize = DEFAULTSPRITESIZE;
    public float currentBlowfishSize = DEFAULTSPRITESIZE;
    public float currentGuppySize = DEFAULTSPRITESIZE;
    public float currentStarfishSize = DEFAULTSPRITESIZE;
    public TextMeshProUGUI volumeValue;
    public TextMeshProUGUI shakePowerValue;
    public TextMeshProUGUI bubbleFrequencyValue;
    public TextMeshProUGUI shakenBubbleFrequencyValue;
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
        SetFrequencyText(shakenBubbleFrequencyValue, currentShakenBubbleFrequency);
        SetMultiplierText(bubbleCountValue, currentBubbleCount);
        SetVariationText(bubbleSizeVariationValue, currentBubbleSizeVariation);
        SetSizeText(bubbleSizeValue, currentBubbleSize);
        SetSizeText(blowfishSizeValue, currentBlowfishSize);
        SetSizeText(guppySizeValue, currentGuppySize);
        SetSizeText(starfishSizeValue, currentStarfishSize);
    }

    void OnEnable() {
        // Load user options
        // currentBlur = PlayerPrefs.GetString("blur", "true") == "true" ? true : false;  // if "true" set to true else false
        // currentVolume = PlayerPrefs.GetFloat("volume", DEFAULTVOLUME);
        // currentShakePower = PlayerPrefs.GetFloat("shakepower", DEFAULTSHAKEPOWER);
        // currentBubbleFrequency = PlayerPrefs.GetFloat("bubblefrequency", DEFAULTBUBBLEFREQUENCY);
        // currentShakenBubbleFrequency = PlayerPrefs.GetFloat("shakenbubblefrequency", DEFAULTSHAKENBUBBLEFREQUENCY);
        // currentBubbleCount = PlayerPrefs.GetFloat("bubblecount", DEFAULTBUBBLECOUNT);
        // currentBubbleSizeVariation = PlayerPrefs.GetFloat("bubblesizevariation", DEFAULTBUBBLESIZEVARIATION);
        // currentBubbleSize = PlayerPrefs.GetFloat("bubblesize", DEFAULTSPRITESIZE);
        // currentBlowfishSize = PlayerPrefs.GetFloat("blowfishsize", DEFAULTSPRITESIZE);
        // currentGuppySize = PlayerPrefs.GetFloat("guppysize", DEFAULTSPRITESIZE);
        // currentStarfishSize = PlayerPrefs.GetFloat("starfishsize", DEFAULTSPRITESIZE);
    }

    void OnDisable() {
        // Save user options
        PlayerPrefs.SetString("blur", currentBlur ? "true" : "false");  // if true set string "true" else "false"
        PlayerPrefs.SetString("landscape", orientation ? "true" : "false");
        PlayerPrefs.SetFloat("volume", currentVolume);
        PlayerPrefs.SetFloat("shakepower", currentShakePower);
        PlayerPrefs.SetFloat("bubblefrequency", currentBubbleFrequency);
        PlayerPrefs.SetFloat("shakenbubblefrequency", currentShakenBubbleFrequency);
        PlayerPrefs.SetFloat("bubblecount", currentBubbleCount);
        PlayerPrefs.SetFloat("bubblesizevariation", currentBubbleSizeVariation);
        PlayerPrefs.SetFloat("bubblesize", currentBubbleSize);
        PlayerPrefs.SetFloat("blowfishsize", currentBlowfishSize);
        PlayerPrefs.SetFloat("guppysize", currentGuppySize);
        PlayerPrefs.SetFloat("starfishsize", currentStarfishSize);
    }

    // On Changed Functions
    public void BlurChanged(bool blur) {
        currentBlur = blur;
    }

    public void Orientation(bool landscape) {
        orientation = landscape;
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

    public void ShakenBubbleFrequencyChanged(float frequency) {
        currentShakenBubbleFrequency = SHAKENBUBBLEFREQUENCYDIFFERENCE - frequency;
        SetFrequencyText(shakenBubbleFrequencyValue, currentShakenBubbleFrequency);
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


    // Set Text Helper Functions
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
        // Greater than 1 is for regular frequency, less than 1 is for shaken frequency
        if (value >= 1.0f) {
            if (value >= 6.0f) {
                sliderText.text = "Less";
            } else if (value <= 3.0f) {
                sliderText.text = "More";
            }
        } else {
            if (value >= 0.16f) {
                sliderText.text = "Less";
            } else if (value <= 0.08f) {
                sliderText.text = "More";
            }
        }
    }

    public void DefaultSettings() {
        // Reset defaults and reload scene
        currentVolume = DEFAULTVOLUME;
        currentShakePower = DEFAULTSHAKEPOWER;
        currentBubbleFrequency = DEFAULTBUBBLEFREQUENCY;
        currentShakenBubbleFrequency = DEFAULTSHAKENBUBBLEFREQUENCY;
        currentBubbleCount = DEFAULTBUBBLECOUNT;
        currentBubbleSizeVariation = DEFAULTBUBBLESIZEVARIATION;
        currentBubbleSize = DEFAULTSPRITESIZE;
        currentBlowfishSize = DEFAULTSPRITESIZE;
        currentGuppySize = DEFAULTSPRITESIZE;
        currentStarfishSize = DEFAULTSPRITESIZE;
        SceneManager.LoadScene("Menu");
    }
}//end of OptionsMenuScript
