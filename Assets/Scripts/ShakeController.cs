using UnityEngine;

public class ShakeController : MonoBehaviour
{
    public float shakeForceMultiplier;
    public FishController[] Fishies;
    public StarfishMovement[] Starfishies;

    public void ShakeFish(Vector3 deviceAcceleration) {
        foreach (var f in Fishies) {
            f.StartShake();
            Debug.Log("shaking a fish");
        }
        foreach (var s in Starfishies) {
            s.StartShake();
            Debug.Log("Shaking a starfish");
        }
    }
}
