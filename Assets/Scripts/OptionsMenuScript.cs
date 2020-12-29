using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;
public class OptionsMenuScript : MonoBehaviour
{
    // Default Settings  TODO: find a better way to do this
    private const bool DEFAULTLANDSCAPE = false;
    private const bool DEFAULTHANDS = true;
    private const float DEFAULTVOLUME = 1.0f;
    private const float DEFAULTBUBBLEAMOUNT = 4.0f;
    private const float DEFAULTBUBBLEFREQUENCY = 4.0f;
    private const float BUBBLEFREQUENCY_MIN = 1.0f;
    private const float BUBBLEFREQUENCY_MAX = 8.0f;
    private const float DEFAULTSHAKENBUBBLEFREQUENCY = 0.13f;
    private const float SHAKENBUBBLEFREQUENCY_MIN = 0.05f;
    private const float SHAKENBUBBLEFREQUENCY_MAX = 0.2f;
    private const float DEFAULTBUBBLECOUNT = 2.0f;
    private const float DEFAULTSPRITESIZE = 1.0f;
    private const float DEFAULTSHAKESENSITIVITY = 1.6f;

    private float currentBubbleFrequency = DEFAULTBUBBLEFREQUENCY;
    private float currentShakenBubbleFrequency = DEFAULTSHAKENBUBBLEFREQUENCY;
    private float currentBubbleCount = DEFAULTBUBBLECOUNT;
    // Sliders, Toggles, Etc.
    public Toggle landscapeToggle;
    public Toggle handsToggle;
    public Slider volumeSlider;
    public Slider bubbleAmountSlider;
    public Slider bubbleSizeSlider;
    public Slider fishSizeSlider;
    public Slider shakeSensitivitySlider;

    // TextMesh Text Value Boxes
    public TextMeshProUGUI volumeValue;
    public TextMeshProUGUI bubbleAmountValue;
    public TextMeshProUGUI bubbleSizeValue;
    public TextMeshProUGUI fishSizeValue;
    public TextMeshProUGUI shakeSensitivityValue;

    void Start() {
        SetPercentageText(volumeValue, volumeSlider.value);
        SetFrequencyText(bubbleAmountValue, bubbleAmountSlider.value);
        SetSizeText(bubbleSizeValue, bubbleSizeSlider.value);
        SetSizeText(fishSizeValue, fishSizeSlider.value);
        SetSensitivityText(shakeSensitivityValue, shakeSensitivitySlider.value);
    }

    void OnEnable() {
        // Force portrait mode for menu
        Screen.orientation = ScreenOrientation.Portrait;
        // Load user options
        landscapeToggle.isOn = PlayerPrefs.GetString("landscape", "false") == "true" ? true : false;
        handsToggle.isOn = PlayerPrefs.GetString("hands", "true") == "true" ? true : false;
        volumeSlider.value = PlayerPrefs.GetFloat("volume", DEFAULTVOLUME);
        bubbleAmountSlider.value = PlayerPrefs.GetFloat("bubbleamount", DEFAULTBUBBLEAMOUNT);
        currentBubbleFrequency = (BUBBLEFREQUENCY_MIN + BUBBLEFREQUENCY_MAX) - PlayerPrefs.GetFloat("bubblefrequency", DEFAULTBUBBLEFREQUENCY);
        currentShakenBubbleFrequency = (SHAKENBUBBLEFREQUENCY_MIN + SHAKENBUBBLEFREQUENCY_MAX) - PlayerPrefs.GetFloat("shakenbubblefrequency", DEFAULTSHAKENBUBBLEFREQUENCY);
        currentBubbleCount = PlayerPrefs.GetFloat("bubblecount", DEFAULTBUBBLECOUNT);
        bubbleSizeSlider.value = PlayerPrefs.GetFloat("bubblesize", DEFAULTSPRITESIZE);
        fishSizeSlider.value = PlayerPrefs.GetFloat("blowfishsize", DEFAULTSPRITESIZE);
        //fishSizeSlider.value = PlayerPrefs.GetFloat("guppysize", DEFAULTSPRITESIZE);
        //fishSizeSlider.value = PlayerPrefs.GetFloat("starfishsize", DEFAULTSPRITESIZE);
        shakeSensitivitySlider.value = PlayerPrefs.GetFloat("shakesensitivity", DEFAULTSHAKESENSITIVITY);
    }

    void OnDisable() {
        // Save user options
        PlayerPrefs.SetString("landscape", landscapeToggle.isOn ? "true" : "false");
        PlayerPrefs.SetString("hands", handsToggle.isOn ? "true" : "false");
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetFloat("bubbleamount", bubbleAmountSlider.value);
        PlayerPrefs.SetFloat("bubblefrequency", (BUBBLEFREQUENCY_MIN + BUBBLEFREQUENCY_MAX) - currentBubbleFrequency); // Send the min+max - current
        PlayerPrefs.SetFloat("shakenbubblefrequency", (SHAKENBUBBLEFREQUENCY_MIN + SHAKENBUBBLEFREQUENCY_MAX) - currentShakenBubbleFrequency); // Send the min+max - current
        PlayerPrefs.SetFloat("bubblecount", currentBubbleCount);
        PlayerPrefs.SetFloat("bubblesize", bubbleSizeSlider.value);
        PlayerPrefs.SetFloat("blowfishsize", fishSizeSlider.value);
        PlayerPrefs.SetFloat("guppysize", fishSizeSlider.value);
        PlayerPrefs.SetFloat("starfishsize", fishSizeSlider.value);
        PlayerPrefs.SetFloat("shakesensitivity", shakeSensitivitySlider.value);
    }

    // On Changed Functions
    public void VolumeChanged(float volume) {
        SetPercentageText(volumeValue, volume);
    }

    public void BubbleAmountChanged(float amount) {
        // Calculate and set the three values based on the selected amount
        currentBubbleFrequency = amount;
        float ratio = amount / BUBBLEFREQUENCY_MAX;
        currentShakenBubbleFrequency = SHAKENBUBBLEFREQUENCY_MAX * ratio;
        // TODO: ensure this is smooth enough
        currentBubbleCount = DEFAULTBUBBLECOUNT;
        if (amount > 2.66f) {
            currentBubbleCount = 2;
        } else if (amount > 5.32f) {
            currentBubbleCount = 3;
        }
        currentBubbleCount = DEFAULTBUBBLECOUNT;
        SetFrequencyText(bubbleAmountValue, amount);
    }

    public void BubbleSizeChanged(float size) {
        SetSizeText(bubbleSizeValue, size);
    }

    public void FishSizeChanged(float size) {
        SetSizeText(fishSizeValue, size);
    }

    public void ShakeSensitivityChanged(float sensitivity) {
        SetSensitivityText(shakeSensitivityValue, sensitivity);
    }


    // Set Text Helper Functions
    void SetPercentageText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    void SetSizeText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = "Normal";
        if (value < 0.75f) {
            sliderText.text = "Small";
        } else if (value > 1.5f && value < 2.25f) {
            sliderText.text = "Big";
        } else if (value >= 2.25f) {
            sliderText.text = "Huge";
        }
    }

    void SetFrequencyText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = "Normal";
        if (value >= 5.32f) {
            sliderText.text = "More";
        } else if (value <= 2.66f) {
            sliderText.text = "Less";
        }
    }

    void SetSensitivityText(TextMeshProUGUI sliderText, float value) {
        sliderText.text = "Normal";
        if (value >= 2.1f) {
            sliderText.text = "Harder";
        } else if (value <= 1.5f) {
            sliderText.text = "Easier";
        }
    }

    public void DefaultSettings() {
        // Reset defaults and reload scene
        landscapeToggle.isOn = DEFAULTLANDSCAPE;
        handsToggle.isOn = DEFAULTHANDS;
        volumeSlider.value = DEFAULTVOLUME;
        bubbleAmountSlider.value = DEFAULTBUBBLEAMOUNT;
        currentBubbleFrequency = DEFAULTBUBBLEFREQUENCY;
        currentShakenBubbleFrequency = DEFAULTSHAKENBUBBLEFREQUENCY;
        currentBubbleCount = DEFAULTBUBBLECOUNT;
        bubbleSizeSlider.value = DEFAULTSPRITESIZE;
        fishSizeSlider.value = DEFAULTSPRITESIZE;
        shakeSensitivitySlider.value = DEFAULTSHAKESENSITIVITY;
    }
}//end of OptionsMenuScript
