using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public float shakeForceMultiplier;
    public MovementController[] Fishies;
    public StarfishController[] Starfishies;
    public Bubble[] Bubbles;

    public void ShakeFish(Vector3 deviceAcceleration) {
        foreach (var f in Fishies) {
            f.StartShake();
            Debug.Log("shaking a fish");
        }
        foreach (var s in Starfishies) {
            s.StartShake();
            Debug.Log("Shaking a starfish");
        }
        foreach (var b in Bubbles) {
            if (b != null) {
                b.StartShake();
            }
        }
    }
}
