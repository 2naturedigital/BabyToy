using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI; // Required when Using UI elements.

public class OptionsMenuScript : MonoBehaviour
{
    //public Slider mainSlider;
    private float currentVolume;
    private int currentShakePower;
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
}
