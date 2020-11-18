using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishMovement : FishController
{
    public Animator animator;
    public float wobbleSpeed;
    public float wobbleShakeSpeed;
    public float wobbleMinAngle;
    public float wobbleMaxAngle;
    private int direction;

    // Start is called before the first frame update
    void Start() {
        direction = -1;
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

    // Update is called once per frame
    void Update() {
        //Debug.Log("MY Z IS AT: " + transform.rotation.z);
        if (!isShaking && !isResetTime) {                                    // wobble normally
            // flip wobble direction after max or min is reached
            if (transform.rotation.z >= wobbleMinAngle || transform.rotation.z <= wobbleMaxAngle) {
                FlipWobble();
            }
            Wobble(wobbleSpeed);
        } else if (isShaking) {                              // spin fast
            Wobble(wobbleShakeSpeed);
        } else if (isResetTime) {                           // head back to reset position
            if (transform.rotation.z <= .1 && transform.rotation.z >= -.1) {
                //Debug.Log("Reset Complete");
                //Debug.Log("Complete Z at: " + transform.rotation.z);
                isResetTime = false;
            }
            Wobble(wobbleShakeSpeed - 2);
        }
    }
}
