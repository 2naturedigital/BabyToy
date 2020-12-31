using UnityEngine;

public class SoundController : MonoBehaviour
{
    private const float DEFAULTVOLUME = 1.0f;

    public AudioSource audioSrc;
    private float volumeModifier;

    void OnEnable() {
        // Grab user options
        volumeModifier = PlayerPrefs.GetFloat("volume", DEFAULTVOLUME);
        //Debug.Log("Volume: " + volumeModifier);
    }

    public void PlaySFX(AudioClip audioClip, float vol = 1f, float pitch = 1f, bool stop = false) {
        audioSrc.clip = audioClip;
        if (stop) {
            audioSrc.Stop();
        }
        // Modify volume based on user options
        vol *= volumeModifier;
        audioSrc.volume = vol;
        audioSrc.pitch = pitch;
        audioSrc.PlayOneShot(audioSrc.clip);
    }
}//end of SoundController
