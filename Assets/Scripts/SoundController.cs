using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSrc;
    // private bool isShaking = false;
    // private bool isStillShaking = false;
    // private bool shakePlayed = false;
    // private bool medShakePlayed = false;
    // private int shakeCheckTimer;
    // public SoundController sndCtrl;
    // public AudioClip shake1;
    // public AudioClip shake2;
    // public AudioClip shake3;

    private void Start() {
        // sndCtrl = FindObjectOfType<SoundController>();
        // shakeCheckTimer = null;
    }

    public void PlaySFX(AudioClip audioClip, float vol = 1f, float pitch = 1f) {
        audioSrc.clip = audioClip;
        audioSrc.volume = vol;
        audioSrc.pitch = pitch;
        audioSrc.PlayOneShot(audioSrc.clip);
    }

    // public void WaterShakeSFX() {
    //     if (isShaking) {
    //         if (shakePlayed == false) {
    //             shakeCheckTimer = 1;
    //             PlaySFX(shake1);
    //             shakePlayed = true;
    //         } else {
    //             shakeCheckTimer -= Time.DeltaTime;
    //         }

    //         if (shakeCheckTimer <= 0 && shakePlayed == true && medShakePlayed == false) {

    //         }
    //     }
    //     if (isShaking && shakePlayed == true && shakeCheckTimer > 0) {
    //         shakeCheckTimer -= Time.DeltaTime;
    //     }
    //     if (medShakePlayed == false && shakeCheckTimer <= 0) {
    //         isStillShaking = true;
    //         audioSrc.Stop();
    //         PlaySFX(shake2);
    //         medShakePlayed == true;
    //         shakeCheckTimer = 1;
    //     }
    //     if (medShakePlayed == true && shakeCheckTimer > 0) {
    //         shakeCheckTimer -= Time.DeltaTime;
    //     }
    //     if (medShakePlayed == true && shakeCheckTimer <= 0) {

    //     }
    // }
}

