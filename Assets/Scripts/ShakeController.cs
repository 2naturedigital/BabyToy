using UnityEngine;

public class ShakeController : MonoBehaviour
{
    // Used to increase the magnintude of the shake all across the game
    [SerializeField]
    private float shakeForceMultiplier = 2;
    [SerializeField]
    private float shakeResetTimer;
    public FishController[] fishies;
    public WaterCurrent[] waterCurrent;
    public BubblesDup bubblesDup;
    public TankCurrent tankCurrent;

    private float elapsedTime = 0;
    private bool isShaking = false;

    void OnEnable() {
        // Grab user options
        //shakeForceMultiplier = PlayerPrefs.GetFloat("shakepower");
        //Debug.Log("ShakeController - Shake Power: " + shakeForceMultiplier);
    }

    public void Shake(Vector3 deviceAcceleration) {
        if (!isShaking) {
            isShaking = true;
            foreach (var f in fishies) {
                f.StartShake(deviceAcceleration, shakeForceMultiplier);
            }
            foreach (var c in waterCurrent) {
                c.StartShake(deviceAcceleration, shakeForceMultiplier);
            }
            bubblesDup.StartShake(deviceAcceleration, shakeForceMultiplier);
            tankCurrent.StartShake(deviceAcceleration, shakeForceMultiplier);
        } else {
            foreach (var f in fishies) {
                f.ContinueShake(deviceAcceleration, shakeForceMultiplier);
            }
            foreach (var c in waterCurrent) {
                c.ContinueShake(deviceAcceleration, shakeForceMultiplier);
            }
            bubblesDup.ContinueShake(deviceAcceleration, shakeForceMultiplier);
            tankCurrent.ContinueShake(deviceAcceleration, shakeForceMultiplier);
        }
        // Resets the elapsed timer since shaking is still happening
        elapsedTime = 0;
    }

    void Update() {
        // If shaking, countdown the timer to call endshake on objects
        if (isShaking) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > shakeResetTimer) {
                isShaking = false;
                foreach (var f in fishies) {
                    f.EndShake();
                }
                foreach (var c in waterCurrent) {
                    c.EndShake();
                }
                bubblesDup.EndShake();
                tankCurrent.EndShake();
                elapsedTime = 0;
            }
        }
    }
}//end of ShakeController
