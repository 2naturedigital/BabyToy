using UnityEngine;

[DefaultExecutionOrder(-2)]
[RequireComponent(typeof(ShakeController))]
public class Accelerometer : MonoBehaviour
{
    private const float DEFAULTSHAKESENSITIVITY = 1.6f;

    [SerializeField]
    private float _shakeDetectionThreshold;
    public float ShakeDetectionThreshold
    {
        get { return _shakeDetectionThreshold; }
        set
        {
            _shakeDetectionThreshold = value;
            sqrShakeDetectionThreshold = _shakeDetectionThreshold * _shakeDetectionThreshold; // Update the squared threshold
        }
    }

    [SerializeField]
    private float minShakeInterval;

    private float sqrShakeDetectionThreshold;
    private float timeSinceLastShake;
    private ShakeController shakeController;

    void Start()
    {
        // Less system taxing to use squared magnitude rather than square root
        sqrShakeDetectionThreshold = Mathf.Pow(ShakeDetectionThreshold, 2);

        // Try to get the ShakeController component and log an error if it's not found
        if (!TryGetComponent<ShakeController>(out shakeController))
        {
            Debug.LogError("ShakeController component not found!");
        }
    }

    void OnEnable()
    {
        // Grab user options
        ShakeDetectionThreshold = PlayerPrefs.GetFloat("shakesensitivity", DEFAULTSHAKESENSITIVITY);
    }

    void Update()
    {
        // Shake only if threshold is met and it's been enough time since last shake
        if (Input.acceleration.sqrMagnitude >= sqrShakeDetectionThreshold
            && Time.unscaledTime >= timeSinceLastShake + minShakeInterval)
        {
            if (shakeController != null)
            {
                shakeController.Shake(Input.acceleration);
            }
            // Reset time since last shake
            timeSinceLastShake = Time.unscaledTime;
        }
    }
}
