using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI; // Required when Using UI elements.

public class OptionsMenuScript : MonoBehaviour
{
    //public Slider mainSlider;
    private float currentVolume;
    private int currentShakePower;
    private float currentBubbleAmount;
    private int currentBubbleCount;

    public void VolumeChanged(float vol) {
        // Store volume changes for use by rattler
        //Debug.Log("Volume Changed");
        currentVolume = vol;
        //Debug.Log("Volume: " + currentVolume);
    }

    public void ShakePowerChanged(int power) {
        // Store shake power changes for use by rattler
        currentShakePower = power;
    }

    public void BubbleAmountChanged(float amount) {
        // Store bubble amount changes for use by rattler
        currentBubbleAmount = amount;
        //Debug.Log("Bubble Amount: " + currentBubbleAmount);
    }

    public void BubbleCountChanged(int count) {
        // Store shake power changes for use by rattler
        currentBubbleCount = count;
    }


    public void DefaultSettings() {
        // Reset defaults
        currentVolume = 1f;
        currentShakePower = 1;
        currentBubbleAmount = .15f;
        currentBubbleCount = 1;
    }


    public float GetVolume() {
        return currentVolume;
    }
}
