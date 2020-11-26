using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public float shakeForceMultiplier;
    public int shakeResetTimer = 3;
    public FishController[] Fishies;
    //public StarfishMovement[] Starfishies;
    public Bubble[] Bubbles;

    public BubblesDup bubblesDup;
    private float elapsedTime = 0;

    public void ShakeFish(Vector3 deviceAcceleration) {
        foreach (var f in Fishies) {
            f.StartShake(deviceAcceleration);
            Debug.Log("shaking a fish");
        }
        foreach (var b in Bubbles) {
            if (b != null) {
                b.StartShake(deviceAcceleration);
            }
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
            foreach (var b in Bubbles) {
                if (b != null) {
                    b.EndShake();
                }
            }
            bubblesDup.EndShake();
            elapsedTime = 0;
        }
    }
}
