using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{   
    public AudioSource audioSrc;

    public void PlaySFX(AudioClip audioClip) {
        audioSrc.clip = audioClip;
        audioSrc.PlayOneShot(audioSrc.clip);
    }

}

