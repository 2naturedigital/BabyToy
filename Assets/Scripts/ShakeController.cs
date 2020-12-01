using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public float shakeForceMultiplier;
    public float shakeResetTimer;
    public FishController[] Fishies;
    public BubblesDup bubblesDup;
    public WaterCurrent waterCurrent;
    private float elapsedTime = 0;

    public void ShakeFish(Vector3 deviceAcceleration) {
        foreach (var f in Fishies) {
            f.StartShake(deviceAcceleration);
            Debug.Log("shaking a fish");
        }
        bubblesDup.StartShake(deviceAcceleration);
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
                }
            }
            bubblesDup.EndShake();
            elapsedTime = 0;
        }
    }
}
