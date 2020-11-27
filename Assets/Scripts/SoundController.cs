using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSrc;

    public void PlaySFX(AudioClip audioClip, float vol = 1f, float pitch = 1f) {
        audioSrc.clip = audioClip;
        audioSrc.volume = vol;
        audioSrc.pitch = pitch;
        audioSrc.PlayOneShot(audioSrc.clip);
    }
}

