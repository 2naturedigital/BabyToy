using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSrc;

    void Start() {
    }

    public void PlaySFX(AudioClip audioClip, float vol = 1f, float pitch = 1f, bool stop = false) {
        audioSrc.clip = audioClip;
        if (stop) {
            audioSrc.Stop();
        }
        audioSrc.volume = vol;
        audioSrc.pitch = pitch;
        audioSrc.PlayOneShot(audioSrc.clip);
    }
}//end of SoundController
