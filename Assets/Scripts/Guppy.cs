using UnityEngine;

public class Guppy : FishController
{
    private const float DEFAULTSPRITESIZE = 1.0f;

    void Start() {
        InitializeFish();
        SetRandomTarget();
    }

    void OnEnable() {
        // Grab user options
        SetUserSpriteSize(PlayerPrefs.GetFloat("guppysize", DEFAULTSPRITESIZE));
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
                    // Guppy oh shit animation
                    GetAnimator().SetTrigger("isTapped");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Guppy - Guppy Bumpy!");
        if (other.tag == "Fish") {
            SetRandomTarget();
        }
    }
}//end of Guppy
