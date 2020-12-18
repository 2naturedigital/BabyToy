using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraScreenScale : MonoBehaviour {
    public SpriteRenderer bg;
    private float spriteAdjustmentRatio;
    private int screenHeight;
    private int screenWidth;
    // public bool maintainWidth = false;
    // [Range(-1,1)]
    // public int adaptPosition;
    // private Vector3 CameraPos;
    // private float defaultWidth;
    // private float defaultHeight;

    void Start() {
        // Calculate screen ratio
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        CalculateScreen();

        /*In case we decide to go with landscape rotation **
        CameraPos = Camera.main.transform.position;
        defaultHeight = Camera.main.orthographicSize;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        */
    }

    // In case we decide to go with landscape rotation **
    void Update() {
        if (screenHeight != Screen.height || screenWidth != Screen.width) {
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            CalculateScreen();
        }

        // Go back to main menu on a 4 finger touch TODO: make this work differently
        if (Input.touchCount == 4) {
            SceneManager.LoadScene("Menu");
        }

        // In case we decide to go with landscape rotation **
        // if (maintainWidth) {
        //     Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;
        //     Camera.main.transform.position = new Vector3(CameraPos.x, adaptPosition * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
        // } else {
        //     Camera.main.transform.position = new Vector3(adaptPosition * (defaultWidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
        // }
    }

    void CalculateScreen() {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio;
        if (bg.bounds.size.x <= bg.bounds.size.y) {
            targetRatio = bg.bounds.size.x / bg.bounds.size.y;
            spriteAdjustmentRatio = 1;
        } else {
            targetRatio = bg.bounds.size.y / bg.bounds.size.x;
            spriteAdjustmentRatio = targetRatio;
        }

        if (screenRatio >= targetRatio) {
            //Debug.Log("CameraScreenScale - BG Size Is: " + bg.bounds.size.y);
            Camera.main.orthographicSize = bg.bounds.size.y/2;
            Debug.Log("CameraScreenScale - ScreenRatio Larger - Ortho Size Set: " + Camera.main.orthographicSize);
            // Set for landscape
            // Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
        } else {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = bg.bounds.size.y/2 * differenceInSize;
            Debug.Log("CameraScreenScale - TargetRatio Larger - Ortho Size Set: " + Camera.main.orthographicSize);
            // Set for landscape
            // Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f  * differenceInSize;
        }
    }

    public float GetSpriteAdjustmentRatio() {
        return spriteAdjustmentRatio;
    }
}//end of CameraScreenScale
