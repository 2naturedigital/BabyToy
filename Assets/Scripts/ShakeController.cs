using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public float shakeForceMultiplier;
    public float shakeResetTimer;
    public FishController[] Fishies;
    public BubblesDup bubblesDup;
    public WaterCurrent waterCurrent;
    private float elapsedTime = 0;
    private bool isShaking = false;

    public void ShakeFish(Vector3 deviceAcceleration) {
        if (!isShaking) {
            isShaking = true;
            foreach (var f in Fishies) {
                f.StartShake(deviceAcceleration);
                Debug.Log("shaking a fish");
            }
            bubblesDup.StartShake(deviceAcceleration);
        } else {
            foreach (var f in Fishies) {
                f.ContinueShake(deviceAcceleration);
                Debug.Log("continue to shake");
            }
            bubblesDup.StartShake(deviceAcceleration);
        }
        // resets the elapsed timer since shaking is still happening
        elapsedTime = 0;
    }

    private void Update() {
        // timer to check for shake (just for testing)
        elapsedTime += Time.deltaTime;
        if (elapsedTime > shakeResetTimer) {
            foreach (var f in Fishies) {
                if (f != null){
                    f.EndShake();
                    isShaking = false;
                }
            }
            bubblesDup.EndShake();
            elapsedTime = 0;
        }
    }
}
