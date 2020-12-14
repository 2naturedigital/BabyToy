using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSrc;
    public OptionsMenuScript userOptions;
    private float volumeModifier;

    void Awake() {
        //Debug.Log("Awake");
        //userOptions = FindObjectOfType<OptionsMenuScript>();
    }

    void OnEnable() {
        //Debug.Log("OnEnable");
        // Grab user volume options
        //volumeModifier = userOptions.GetVolume();
    }

    public void PlaySFX(AudioClip audioClip, float vol = 1f, float pitch = 1f, bool stop = false) {
        audioSrc.clip = audioClip;
        if (stop) {
            audioSrc.Stop();
        }
        // Modify volume based on user options
        //vol *= volumeModifier;
        audioSrc.volume = vol;
        audioSrc.pitch = pitch;
        audioSrc.PlayOneShot(audioSrc.clip);
    }
}//end of SoundController
