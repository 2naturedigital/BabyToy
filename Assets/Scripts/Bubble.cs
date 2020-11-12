using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO -- randomize sounds for spawned bubbles

public class Bubble : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSrc;
    public AudioClip[] audioClipArray;
    public float destroyTimer;
    private GameObject bubble;
    Collider2D col;
    Rigidbody2D rb;


    void start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        //set rando bubble sounds here
    }    
    
    void Update() {      
        

        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // When a touch begins, grab its location and see if it is overlaping a collider2d object
            if (touch.phase == TouchPhase.Began) {
                if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition)) {
                    bubble = GetComponent<Collider2D>().gameObject;
                    Destroyer();
                }
            }            
        }
    }

    private void Destroyer() {
        animator.SetTrigger("Touched");
        audioSrc.clip = audioClipArray[Random.Range(0, audioClipArray.Length)];
        audioSrc.PlayOneShot(audioSrc.clip);
        Destroy(bubble, destroyTimer);
    }
}