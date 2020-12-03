using UnityEngine;

public class RandomSounds : MonoBehaviour
{

    public AudioSource audioSrc;
    public AudioClip[] audioClipArray;

    void Start() {
        audioSrc.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        audioSrc.PlayOneShot(audioSrc.clip);
    }
}
