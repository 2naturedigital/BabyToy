using UnityEngine;

public class Starfish : FishController
{
    private const int FISHWIDTH = 577;
    private const int FISHHEIGHT = 547;
    public float wobbleSpeed;
    public float wobbleShakeSpeed;
    public float wobbleMinAngle;
    public float wobbleMaxAngle;
    private int direction;

    void Start() {
        SetCameraProperties();
        SetSoundController(FindObjectOfType<SoundController>());
        SetAnimator(GetComponent<Animator>());
        SetFishSize(FISHWIDTH, FISHHEIGHT);
        direction = -1;
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
                FlipWobble();
            }
            Wobble(wobbleSpeed);
        } else if (IsShaking()) {                              // spin fast
            Wobble(wobbleShakeSpeed);
        } else if (IsResetTime()) {                           // head back to reset position
            Wobble(wobbleShakeSpeed - 2);
            if (transform.rotation.z <= wobbleMinAngle && transform.rotation.z >= wobbleMaxAngle) {
                //Debug.Log("Reset Complete");
                //Debug.Log("Complete Z at: " + transform.rotation.z);
                SetResetTime(false);
            }
        }
    }

    void Wobble(float speed) {
        // Rotation based on rotation created
        float rotation = 0;
        rotation = direction * speed * GetShakeMultiplier();
        transform.Rotate(0, 0, rotation);
    }

    void FlipWobble() {
        direction *= -1;
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}//end of Starfish
