using UnityEngine;

public class Starfish : FishController
{
    //public Animator animator;
    public float wobbleSpeed;
    public float wobbleShakeSpeed;
    public float wobbleMinAngle;
    public float wobbleMaxAngle;
    private int direction;

    // Start is called before the first frame update
    void Start() {
        direction = -1;
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        AnimateFish();
        MoveFish();
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
            if (transform.rotation.z <= .1 && transform.rotation.z >= -.1) {
                //Debug.Log("Reset Complete");
                //Debug.Log("Complete Z at: " + transform.rotation.z);
                SetResetTime(false);
            }
            Wobble(wobbleShakeSpeed - 2);
        }
    }

    void Wobble(float speed) {
        // rotation based on rotation created
        float rotation = 0;
        rotation = direction * speed;
        transform.Rotate(0, 0, rotation);
    }

    void FlipWobble() {
        direction *= -1;
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
