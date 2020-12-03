﻿using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Animator animator;
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
    private SoundController sndCtrl;
    private Vector3 CameraPos;
    private float defaultWidth;
    private float defaultHeight;


    void Start() {
    }

    // Bubbles are made and destroyed on the fly, so we are using Awake() instead of Start()
    void Awake() {
        sndCtrl = FindObjectOfType<SoundController>();
        lifetimer = Random.Range(bubbleLifetimeMin, bubbleLifetimeMax);
        RandomizeBubbleSounds();
        SetCameraProperties();
        // Must set tag so only bubbles get affected by the small water currents
        this.tag = "Bubble";
    }

    void Update() {
        // Get touch position when the screen is touched
        if (Input.touchCount > 0) {
            // Handle all touches
            foreach (Touch touch in Input.touches) {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                // When a touch begins, grab its location and see if it is overlaping a collider2d object
                if (touch.phase == TouchPhase.Began) {
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPosition)) {
                        // Set the collider2d object to a gameobject & destroy it
                        bubble = GetComponent<Collider2D>().gameObject;
                        PopBubble(bubble);
                    }
                }
            }
        }

        // Autodestruct Bubbles
        // Destroy bubbles above the screen
        if (this.transform.position.y >= defaultHeight + 250) {
            Destroy(this.gameObject);
        }
        // Pop bubles based on timer
        if (lifetimer > 0) {
            lifetimer -= Time.deltaTime;
        }
        if (lifetimer <= 0) {
            PopBubble(this.gameObject);
            lifetimer = Random.Range(bubbleLifetimeMin, bubbleLifetimeMax);
        }
    }

    public void SetCameraProperties() {
        CameraPos = Camera.main.transform.position;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        defaultHeight = Camera.main.orthographicSize;
    }

    // Triggers the pop animation, sets a random pop sound & plays, destroys bubble object
    private void PopBubble(GameObject gameObject) {
        animator.SetTrigger("Touched");
        sndCtrl.PlaySFX(popSounds[Random.Range(0, popSounds.Length)]);
        Destroy(gameObject, destroyAnimationTimer);
    }

    // Sets the bubble sound, randomizes pitch & volume
    private void RandomizeBubbleSounds() {
        sndCtrl.PlaySFX(bubbleSound, Random.Range(bubbleVolMin, bubbleVolMax), Random.Range(bubblePitchMin, bubblePitchMax));
    }
}//end of Bubble
