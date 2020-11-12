using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSounds : MonoBehaviour
{

    public AudioSource audioSrc;
    public AudioClip[] audioClipArray;


    private void Awake() {
        audioSrc = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    public void Start() {
        audioSrc.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        audioSrc.PlayOneShot(audioSrc.clip);
    }

   
}
