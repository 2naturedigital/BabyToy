﻿using UnityEngine;

public class Starfish : FishController
{
    private const float DEFAULTSPRITESIZE = 1.0f;

    // User/Unity Adjustable Public Class Variables
    [SerializeField]
    private float wobbleSpeed;
    [SerializeField]
    private float wobbleShakeSpeed;
    [SerializeField]
    private float wobbleMinAngle;
    [SerializeField]
    private float wobbleMaxAngle;

    void Start() {
        InitializeFish();
        SetRandomTarget();
    }

    void OnEnable() {
        // Grab user options
        SetUserSpriteSize(PlayerPrefs.GetFloat("starfishsize", DEFAULTSPRITESIZE));
    }

    void Update() {
        SetCameraProperties();
        MoveFish();
    }

    private void FixedUpdate() {
        HandleTouch();
        SetAnimatorShakeTrigger();
        AnimateFish();
    }

    public void HandleTouch() {
        // Get touch position when the screen is touched
        if (Input.touchCount > 0) {
            // Handle all touches
            foreach (Touch touch in Input.touches) {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                // When a touch begins, grab its location and see if it is overlaping a collider2d object
                if (touch.phase == TouchPhase.Began && GetCollider2D() == Physics2D.OverlapPoint(touchPosition)) {
                    // Starfish big eyes animation
                    GetAnimator().SetTrigger("isTapped");
                }
            }
        }
    }

    public override void AnimateFish() {
        //Debug.Log("MY Z IS AT: " + transform.rotation.z);
        if (!IsShaking() && !IsResetTime()) {                                    // wobble normally
            // Flip wobble direction after max or min is reached
            if (transform.rotation.z >= wobbleMinAngle || transform.rotation.z <= wobbleMaxAngle) {
                FlipRotationDirection();
            }
            Rotate(wobbleSpeed);
        } else if (IsShaking()) {                              // spin fast
            Rotate(wobbleShakeSpeed);
        } else if (IsResetTime()) {                           // head back to reset position
            Rotate(wobbleShakeSpeed);
            if (transform.rotation.z <= wobbleMinAngle && transform.rotation.z >= wobbleMaxAngle) {
                SetResetTime(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}//end of Starfish
