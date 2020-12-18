using UnityEngine;

public class Starfish : FishController
{
    // User/Unity Adjustable Public Class Variables
    public float wobbleSpeed;
    public float wobbleShakeSpeed;
    public float wobbleMinAngle;
    public float wobbleMaxAngle;

    void Start() {
        InitializeFish();
        SetRandomTarget();
    }

    void OnEnable() {
        // Grab user options
        SetUserSpriteSize(PlayerPrefs.GetFloat("starfishsize"));
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
                if (GetCollider2D() == Physics2D.OverlapPoint(touchPosition)) {
                    // Animate the fish
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
            Rotate(wobbleShakeSpeed - 2);
            if (transform.rotation.z <= wobbleMinAngle && transform.rotation.z >= wobbleMaxAngle) {
                SetResetTime(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}//end of Starfish
