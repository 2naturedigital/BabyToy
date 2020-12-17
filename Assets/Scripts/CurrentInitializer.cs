using UnityEngine;

public class CurrentInitializer : MonoBehaviour
{
    public WaterCurrent[] waterCurrents;
    public TankCurrent tankCurrent;
    private Vector3 CameraPos;
    private float screenWidth;
    private float screenHeight;
    private CameraScreenScale cameraScreenScale;
    private float spriteAdjustmentRatio;

    void Start() {
        SetCameraProperties();

        float offset = 1.0f;
        foreach (var c in waterCurrents) {
            // Adjust the water currents to be wider than the screen by a small margin and cut into slices by the aspect ratio
            c.GetComponent<BoxCollider2D>().size = new Vector3(screenWidth*2 * 1.1f, screenHeight*2 / 10);
            // Adjust each collider to be a bit closer to the top
            c.GetComponent<BoxCollider2D>().offset = new Vector3(0, -(screenHeight * 1.1f * offset));
            offset -= 0.25f;
        }
        // Adjust the tank current to encompase an area slightly larger than the screen
        tankCurrent.GetComponent<BoxCollider2D>().size = new Vector3(screenWidth*2 * 1.2f, screenHeight*2 * 1.2f);
    }
    void Update() {
    }

    public void SetCameraProperties() {
        CameraPos = Camera.main.transform.position;
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize;
        cameraScreenScale = FindObjectOfType<CameraScreenScale>();
        spriteAdjustmentRatio = cameraScreenScale.GetSpriteAdjustmentRatio();
    }
}//end of CurrentInitializer
