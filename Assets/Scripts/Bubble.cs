using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSrc;
    public AudioClip bubbleSound;
    public float bubblePitchMin;
    public float bubblePitchMax;
    public float bubbleVolMin;
    public float bubbleVolMax;
    public float bubbleLifetimeMin;
    public float bubbleLifetimeMax;
    private float lifetimer;
    public AudioClip[] popSounds;
    public float destroyAnimationTimer;
    private GameObject bubble;
    Collider2D col;
    Rigidbody2D rb;


    void start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
    }    

    void Awake() {
        RandomBubbles();
        lifetimer = Random.Range(bubbleLifetimeMin, bubbleLifetimeMax);
    }
    
    void Update() {

        //get touch position when the screen is touched
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // When a touch begins, grab its location and see if it is overlaping a collider2d object
            if (touch.phase == TouchPhase.Began) {
                if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition)) {
                    
                    //set the collider2d object to a gameobject & destroy it
                    bubble = GetComponent<Collider2D>().gameObject;
                    Destroyer();                    
                    Destroy(bubble, destroyAnimationTimer);
                }
            }            
        }

        //autodestruct bubbles
        if (lifetimer > 0) {
            lifetimer -= Time.deltaTime;
        } 
        if (lifetimer <= 0) {
            Destroyer();            
            Destroy(this.gameObject, destroyAnimationTimer);
            lifetimer = Random.Range(bubbleLifetimeMin, bubbleLifetimeMax);
        }
        if (this.transform.position.y >= 1000.0f) {
            Destroy(this.gameObject);
        }        
    }
    
    //triggers the pop animation, sets a random pop sound & plays, destroys bubble object
    private void Destroyer() {
        animator.SetTrigger("Touched");
        audioSrc.clip = popSounds[Random.Range(0, popSounds.Length)];
        audioSrc.PlayOneShot(audioSrc.clip);
    }

    //sets the bubble sound, randomizes pitch & volume
    private void RandomBubbles() {
        audioSrc.clip = bubbleSound;
        audioSrc.pitch = Random.Range(bubblePitchMin, bubblePitchMax);
        audioSrc.volume = Random.Range(bubbleVolMin, bubbleVolMax);
        audioSrc.PlayOneShot(audioSrc.clip);
    }

}