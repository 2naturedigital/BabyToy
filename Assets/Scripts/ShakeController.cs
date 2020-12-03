using UnityEngine;

public class ShakeController : MonoBehaviour
{
    // Used to increase the magnintude of the shake but loses vector level information
    //public float shakeForceMultiplier;
    public float shakeResetTimer;
    public FishController[] fishies;
    public WaterCurrent[] waterCurrent;
    public BubblesDup bubblesDup;
    private float elapsedTime = 0;
    private bool isShaking = false;

    public void Shake(Vector3 deviceAcceleration) {
        if (!isShaking) {
            isShaking = true;
            foreach (var f in fishies) {
                f.StartShake(deviceAcceleration);
                //Debug.Log("shaking a fish");
            }
            foreach (var c in waterCurrent) {
                c.StartShake(deviceAcceleration);
                //Debug.Log("shaking a fish");
            }
            bubblesDup.StartShake(deviceAcceleration);
        } else {
            foreach (var f in fishies) {
                f.ContinueShake(deviceAcceleration);
                //Debug.Log("continue to shake");
            }
            foreach (var c in waterCurrent) {
                c.ContinueShake(deviceAcceleration);
                //Debug.Log("shaking a fish");
            }
            bubblesDup.ContinueShake(deviceAcceleration);
        }
        // Resets the elapsed timer since shaking is still happening
        elapsedTime = 0;
    }

    void Update() {
        // Timer to check for shake (just for testing)
        elapsedTime += Time.deltaTime;
        if (elapsedTime > shakeResetTimer) {
            isShaking = false;
            foreach (var f in fishies) {
                f.EndShake();
            }
            foreach (var c in waterCurrent) {
                c.EndShake();
                //Debug.Log("shaking a fish");
            }
            bubblesDup.EndShake();
            elapsedTime = 0;
        }
    }
}//end of ShakeController
