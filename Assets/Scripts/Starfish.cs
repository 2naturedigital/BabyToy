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

    void Update() {
        MoveFish();
    }

    private void FixedUpdate() {
        SetAnimatorShakeTrigger();
        AnimateFish();
    }

    public override void AnimateFish() {
        //Debug.Log("MY Z IS AT: " + transform.rotation.z);
        if (!IsShaking() && !IsResetTime()) {                                    // wobble normally
            // flip wobble direction after max or min is reached
            if (transform.rotation.z >= wobbleMinAngle || transform.rotation.z <= wobbleMaxAngle) {
                FlipRotationDirection();
            }
            Rotate(wobbleSpeed);
        } else if (IsShaking()) {                              // spin fast
            Rotate(wobbleShakeSpeed);
        } else if (IsResetTime()) {                           // head back to reset position
            Rotate(wobbleShakeSpeed - 2);
            if (transform.rotation.z <= wobbleMinAngle && transform.rotation.z >= wobbleMaxAngle) {
                //Debug.Log("Reset Complete");
                //Debug.Log("Complete Z at: " + transform.rotation.z);
                SetResetTime(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}//end of Starfish
