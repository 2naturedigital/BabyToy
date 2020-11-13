using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishMovement : MonoBehaviour
{
    public Animator animator;
    public float wobbleSpeed;
    public float wobbleShakeSpeed;
    public float wobbleMinAngle;
    public float wobbleMaxAngle;
    private int direction;
    public bool isShaken = false;
    public bool isResetTime = false;
    private float elapsedTime = 0;
    public int fakeShakeTestTimer;

    // Start is called before the first frame update
    void Start() {
        direction = -1;
    }

    void Wobble() {

        float rotation = 0;
        //Debug.Log("MY Z IS AT: " + transform.rotation.z);
        if (!isShaken && !isResetTime) {                                    // wobble normally
            // flip wobble direction after max or min is reached
            if (transform.rotation.z >= wobbleMinAngle || transform.rotation.z <= wobbleMaxAngle) {
                FlipWobble();
            }
            rotation = direction * wobbleSpeed;
        } else if (isShaken) {                              // spin fast
            rotation = direction * wobbleShakeSpeed;
        } else if (isResetTime) {                           // head back to reset position
            if (transform.rotation.z <= .1 && transform.rotation.z >= -.1) {
                //Debug.Log("Reset Complete");
                //Debug.Log("Complete Z at: " + transform.rotation.z);
                isResetTime = false;
            }
            rotation = direction * wobbleShakeSpeed - 2;
        }

        // rotation based on rotation created
        transform.Rotate(0, 0, rotation);
    }

    void FlipWobble() {
        direction *= -1;
    }

    void OnShakeStart() {
        isShaken = true;
    }

    void OnShakeEnd() {
        isShaken = false;
        isResetTime = true;
    }

    // Update is called once per frame
    void Update() {
        Wobble();

        // timer to check for shake (just for testing)
        elapsedTime += Time.deltaTime;
        if (elapsedTime > fakeShakeTestTimer) {
            if (!isShaken) {
                OnShakeStart();
            } else {
                OnShakeEnd();
            }
            elapsedTime = 0;
        }
    }
}
