﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AdvancedOptionsMenuScript : MonoBehaviour
{
    // Default Settings  TODO: find a better way to do this
    private const bool DEFAULTBLUR = true;
    private const bool DEFAULTLANDSCAPE = false;
    private const bool DEFAULTWATER = true;
    private const bool DEFAULTHANDS = true;
    private const float DEFAULTVOLUME = 1.0f;
    private const float DEFAULTSHAKEPOWER = 2.0f;
    private const float DEFAULTBUBBLEFREQUENCY = 4.0f;
    private const float DEFAULTSHAKENBUBBLEFREQUENCY = 0.1f;
    private const float DEFAULTBUBBLECOUNT = 1.0f;
    private const float DEFAULTBUBBLESIZEVARIATION = 0.5f;
    private const float DEFAULTSPRITESIZE = 1.0f;
    private const float DEFAULTSHAKESENSITIVITY = 1.6f;
    // Sliders, Toggles, Etc.
    public Toggle blurToggle;
    public Toggle landscapeToggle;
    public Toggle waterToggle;
    public Toggle handsToggle;
    public Slider volumeSlider;
    public Slider shakePowerSlider;
    public Slider bubbleFrequencySlider;
    public Slider shakenBubbleFrequencySlider;
    public Slider bubbleCountSlider;
    public Slider bubbleSizeVariationSlider;
    public Slider bubbleSizeSlider;
    public Slider blowfishSizeSlider;
    public Slider guppySizeSlider;
    public Slider starfishSizeSlider;
    public Slider shakeSensitivitySlider;

    // TextMesh Text Value Boxes
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
    public TextMeshProUGUI shakeSensitivityValue;

    void Start() {
        SetPercentageText(volumeValue, volumeSlider.value);
        SetMultiplierText(shakePowerValue, shakePowerSlider.value);
        SetFrequencyText(bubbleFrequencyValue, bubbleFrequencySlider.value);
        SetFrequencyText(shakenBubbleFrequencyValue, shakenBubbleFrequencySlider.value);
        SetMultiplierText(bubbleCountValue, bubbleCountSlider.value);
        SetVariationText(bubbleSizeVariationValue, bubbleSizeVariationSlider.value);
        SetSizeText(bubbleSizeValue, bubbleSizeSlider.value);
        SetSizeText(blowfishSizeValue, blowfishSizeSlider.value);
        SetSizeText(guppySizeValue, guppySizeSlider.value);
        SetSizeText(starfishSizeValue, starfishSizeSlider.value);
        SetSensitivityText(shakeSensitivityValue, shakeSensitivitySlider.value);
    }

    void OnEnable() {
        // Load user options
        blurToggle.isOn = PlayerPrefs.GetString("blur", "true") == "true" ? true : false;  // If "true" set to true else false
        landscapeToggle.isOn = PlayerPrefs.GetString("landscape", "false") == "true" ? true : false;
        waterToggle.isOn = PlayerPrefs.GetString("water", "true") == "true" ? true : false;
        handsToggle.isOn = PlayerPrefs.GetString("hands", "true") == "true" ? true : false;
        volumeSlider.value = PlayerPrefs.GetFloat("volume", DEFAULTVOLUME);
        shakePowerSlider.value = PlayerPrefs.GetFloat("shakepower", DEFAULTSHAKEPOWER);
        bubbleFrequencySlider.value = (bubbleFrequencySlider.minValue + bubbleFrequencySlider.maxValue) - PlayerPrefs.GetFloat("bubblefrequency", DEFAULTBUBBLEFREQUENCY);
        shakenBubbleFrequencySlider.value = (shakenBubbleFrequencySlider.minValue + shakenBubbleFrequencySlider.maxValue) - PlayerPrefs.GetFloat("shakenbubblefrequency", DEFAULTSHAKENBUBBLEFREQUENCY);
        bubbleCountSlider.value = PlayerPrefs.GetFloat("bubblecount", DEFAULTBUBBLECOUNT);
        bubbleSizeVariationSlider.value = PlayerPrefs.GetFloat("bubblesizevariation", DEFAULTBUBBLESIZEVARIATION);
        bubbleSizeSlider.value = PlayerPrefs.GetFloat("bubblesize", DEFAULTSPRITESIZE);
        blowfishSizeSlider.value = PlayerPrefs.GetFloat("blowfishsize", DEFAULTSPRITESIZE);
        guppySizeSlider.value = PlayerPrefs.GetFloat("guppysize", DEFAULTSPRITESIZE);
        starfishSizeSlider.value = PlayerPrefs.GetFloat("starfishsize", DEFAULTSPRITESIZE);
        shakeSensitivitySlider.value = PlayerPrefs.GetFloat("shakesensitivity", DEFAULTSHAKESENSITIVITY);
    }

    void OnDisable() {
        // Save user options
        PlayerPrefs.SetString("blur", blurToggle.isOn ? "true" : "false");  // If true set string "true" else "false"
        PlayerPrefs.SetString("landscape", landscapeToggle.isOn ? "true" : "false");
        PlayerPrefs.SetString("water", waterToggle.isOn ? "true" : "false");
        PlayerPrefs.SetString("hands", handsToggle.isOn ? "true" : "false");
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetFloat("shakepower", shakePowerSlider.value);
        PlayerPrefs.SetFloat("bubblefrequency", (bubbleFrequencySlider.minValue + bubbleFrequencySlider.maxValue) - bubbleFrequencySlider.value); // Send the min+max - current
        PlayerPrefs.SetFloat("shakenbubblefrequency", (shakenBubbleFrequencySlider.minValue + shakenBubbleFrequencySlider.maxValue) - shakenBubbleFrequencySlider.value); // Send the min+max - current
        PlayerPrefs.SetFloat("bubblecount", bubbleCountSlider.value);
        PlayerPrefs.SetFloat("bubblesizevariation", bubbleSizeVariationSlider.value);
        PlayerPrefs.SetFloat("bubblesize", bubbleSizeSlider.value);
        PlayerPrefs.SetFloat("blowfishsize", blowfishSizeSlider.value);
        PlayerPrefs.SetFloat("guppysize", guppySizeSlider.value);
        PlayerPrefs.SetFloat("starfishsize", starfishSizeSlider.value);
        PlayerPrefs.SetFloat("shakesensitivity", shakeSensitivitySlider.value);
    }

    // On Changed Functions
    public void VolumeChanged(float volume) {
        SetPercentageText(volumeValue, volume);
    }

    public void ShakePowerChanged(float power) {
        SetMultiplierText(shakePowerValue, power);
    }

    public void BubbleFrequencyChanged(float frequency) {
        SetFrequencyText(bubbleFrequencyValue, frequency);
    }

    public void ShakenBubbleFrequencyChanged(float frequency) {
        SetFrequencyText(shakenBubbleFrequencyValue, frequency);
    }

    public void BubbleCountChanged(float count) {
        SetMultiplierText(bubbleCountValue, count);
    }

    public void BubbleSizeVariationChanged(float variation) {
        SetVariationText(bubbleSizeVariationValue, variation);
    }

    public void BubbleSizeChanged(float size) {
        SetSizeText(bubbleSizeValue, size);
    }

    public void BlowfishSizeChanged(float size) {
        SetSizeText(blowfishSizeValue, size);
    }

    public void GuppySizeChanged(float size) {
        SetSizeText(guppySizeValue, size);
    }

    public void StarfishSizeChanged(float size) {
        SetSizeText(starfishSizeValue, size);
    }

    public void ShakeSensitivityChanged(float sensitivity) {
        SetSensitivityText(shakeSensitivityValue, sensitivity);
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
                sliderText.text = "More";
            } else if (value <= 3.0f) {
                sliderText.text = "Less";
            }
        } else {
            if (value >= 0.16f) {
                sliderText.text = "More";
            } else if (value <= 0.08f) {
                sliderText.text = "Less";
            }
        }
    }

    void SetSensitivityText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = "Normal";
        if (value >= 1.9f) {
            sliderText.text = "Harder";
        } else if (value <= 1.4f) {
            sliderText.text = "Easier";
        }
    }

    public void DefaultSettings() {
        // Reset defaults and reload scene
        blurToggle.isOn = DEFAULTBLUR;
        landscapeToggle.isOn = DEFAULTLANDSCAPE;
        waterToggle.isOn = DEFAULTWATER;
        handsToggle.isOn = DEFAULTHANDS;
        volumeSlider.value = DEFAULTVOLUME;
        shakePowerSlider.value = DEFAULTSHAKEPOWER;
        bubbleFrequencySlider.value = DEFAULTBUBBLEFREQUENCY;
        shakenBubbleFrequencySlider.value = DEFAULTSHAKENBUBBLEFREQUENCY;
        bubbleCountSlider.value = DEFAULTBUBBLECOUNT;
        bubbleSizeVariationSlider.value = DEFAULTBUBBLESIZEVARIATION;
        bubbleSizeSlider.value = DEFAULTSPRITESIZE;
        blowfishSizeSlider.value = DEFAULTSPRITESIZE;
        guppySizeSlider.value = DEFAULTSPRITESIZE;
        starfishSizeSlider.value = DEFAULTSPRITESIZE;
        shakeSensitivitySlider.value = DEFAULTSHAKESENSITIVITY;
    }
}//end of OptionsMenuScript
