using UnityEngine;

// This forces the component to require a ShakeController component as well
[RequireComponent(typeof(ShakeController))]

public class Accelerometer : MonoBehaviour
{
    public float shakeDetectionThreshhold;
    public float minShakeInterval;
    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;
    private ShakeController shakeController;

    void Start() {
        //Debug.Log("Accelerometer Started");
        // Less system taxing to use squared magnintude rather than squareroot
        sqrShakeDetectionThreshold = Mathf.Pow(shakeDetectionThreshhold, 2);
        shakeController = GetComponent<ShakeController>();
    }

    void Update() {
        //Debug.Log("sqrmagnitude: " + Input.acceleration.sqrMagnitude);
        // Shake only if threshold is met and it's been enough time since last shake
        if (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold
            && Time.unscaledTime >= timeSinceLastShake + minShakeInterval) {
            //Debug.Log("Shake Detected");
            if (shakeController != null) {
                shakeController.Shake(Input.acceleration);
            }
            // Reset time since last shake
            timeSinceLastShake = Time.unscaledTime;
        }
    }
}//end of Accelerometer
