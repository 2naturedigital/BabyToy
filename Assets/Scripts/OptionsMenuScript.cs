using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI; // Required when Using UI elements.

public class OptionsMenuScript : MonoBehaviour
{
    //public Slider mainSlider;
    private float currentVolume;
    public void VolumeChanged(float v) {
        // Store volume changes for use by rattler
        Debug.Log("Volume Changed");
        currentVolume = v;
        Debug.Log("Volume: " + currentVolume);
    }
}
