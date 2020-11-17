using UnityEngine;

[RequireComponent(typeof(ShakeController))]

public class Accelerometer : MonoBehaviour
{
    public float shakeDetectionThreshhold;
    public float minShakeInterval;
    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;
    private ShakeController shakeController;

    void Start() {
        // less system taxing to use squared magnintude rather than squareroot
        sqrShakeDetectionThreshold = Mathf.Pow(shakeDetectionThreshhold, 2);
        shakeController = GetComponent<ShakeController>();
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log("sqrmagnitude: " + Input.acceleration.sqrMagnitude);
        // Shake only if threshold is met and it's been enough time since last shake
        if (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold
            && Time.unscaledTime >= timeSinceLastShake + minShakeInterval) {
            Debug.Log("Shake Detected");
            if (shakeController != null) {
                shakeController.ShakeFish(Input.acceleration);
            }
            // reset time since last shake
            timeSinceLastShake = Time.unscaledTime;
        }
    }
}
