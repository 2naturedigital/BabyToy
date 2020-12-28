using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]

public class CameraScreenScale : MonoBehaviour {
    public SpriteRenderer bg;
    public SpriteRenderer waterLayer;
    public SpriteRenderer leftHand;
    public SpriteRenderer rightHand;
    public Sprite[] backgrounds;
    private float spriteAdjustmentRatio;
    private int screenHeight;
    private int screenWidth;
    private bool landscape;
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

    void OnEnable() {
        // Change orientation depending on user settings
        landscape = PlayerPrefs.GetString("landscape", "false") == "true" ? true : false;
        if (landscape) {
            Screen.orientation = ScreenOrientation.Landscape;
        }
        // Change water visibility
        // bool water = PlayerPrefs.GetString("water", "true") == "true" ? true : false;
        // waterLayer.enabled = water;
        // Change hands visibility
        bool hands = PlayerPrefs.GetString("hands", "true") == "true" ? true : false;
        leftHand.enabled = hands;
        rightHand.enabled = hands;
        // Change blur on or off
        // bool blur = PlayerPrefs.GetString("blur", "true") == "true" ? true : false;
        // if (blur) {
        //     bg.sprite = backgrounds[0];
        // } else {
        //     bg.sprite = backgrounds[1];
        // }
    }

    // In case we decide to go with landscape rotation **
    void Update() {
        if (screenHeight != Screen.height || screenWidth != Screen.width) {
            screenHeight = Screen.height;
            screenWidth = Screen.width;
            CalculateScreen();
        }


        // DEBUG: Go back to main menu on a 4 finger touch TODO: make this work differently maybe?
        if (Input.touchCount == 4) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                SceneManager.LoadScene("Menu");
            }
        }
        // DEBUG: Go back to main menu on right click
        if (Input.GetMouseButtonUp(1)) {
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

    // Possibly a much simpler way to calculate screen ortho and sprite ratios
    void CalculateScreen() {
        // Adjust sprites to match BG ratio
        if (bg.bounds.size.x > bg.bounds.size.y) {
            spriteAdjustmentRatio = bg.bounds.size.y / bg.bounds.size.x;
        } else {
            spriteAdjustmentRatio = 1;
        }

        // If the screen's height is greater than the bacground's height, stretch ortho to fit BG top to bottom
        // Otherwise, stretch the ortho to fit BG side to side and adjust hand size if needed
        if (Screen.height > bg.bounds.size.y) {
            Camera.main.orthographicSize = bg.bounds.size.y/2;
            leftHand.transform.localScale *= 0.8f;
            rightHand.transform.localScale *= 0.8f;
        } else {
            Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
        }

        // Move the hands to the sides of the screen
        float newLeftX = -(Camera.main.orthographicSize * Camera.main.aspect) + leftHand.bounds.size.x/2;
        float newLeftY = -(Camera.main.orthographicSize) + leftHand.bounds.size.y/2;
        float newRightX = (Camera.main.orthographicSize * Camera.main.aspect) - rightHand.bounds.size.x/2;
        float newRightY = -(Camera.main.orthographicSize) + rightHand.bounds.size.y/2;
        leftHand.transform.position = new Vector3(newLeftX,newLeftY,0);
        rightHand.transform.position = new Vector3(newRightX,newRightY,0);
    }

    // Older way to calculate screen ortho and sprite ratios
    // KEEP in case of any issues
    // void CalculateScreen() {
    //     // Calculate screen ratio based on orientation
    //     float screenRatio;
    //     if (landscape) {
    //         screenRatio = (float)Screen.height / (float)Screen.width;
    //     } else {
    //         screenRatio = (float)Screen.width / (float)Screen.height;
    //     }

    //     // Calculate background ratio based on which is larger, width or height
    //     // Also adjust sprite ratio accordingly
    //     float backgroundRatio;
    //     if (bg.bounds.size.x > bg.bounds.size.y) {
    //         backgroundRatio = bg.bounds.size.y / bg.bounds.size.x;
    //         spriteAdjustmentRatio = backgroundRatio;
    //     } else {
    //         backgroundRatio = bg.bounds.size.x / bg.bounds.size.y;
    //         spriteAdjustmentRatio = 1;
    //     }

    //     // Set ortho size based on screen orientation, ratio differences, and screen height vs bg height
    //     if (landscape) {
    //         //Debug.Log("CameraScreenScale - Landscape");
    //         if (backgroundRatio > screenRatio) {
    //             Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
    //             //Debug.Log("CameraScreenScale - BackgroundRatio Larger");
    //         } else {
    //             Camera.main.orthographicSize = bg.bounds.size.y/2;
    //             //Debug.Log("CameraScreenScale - ScreenRatio Larger");
    //         }
    //     } else {
    //         //Debug.Log("CameraScreenScale - Portrait");
    //         if (backgroundRatio > screenRatio) {
    //             Camera.main.orthographicSize = bg.bounds.size.y/2;
    //             leftHand.transform.localScale *= 0.8f;
    //             rightHand.transform.localScale *= 0.8f;
    //             //Debug.Log("CameraScreenScale - BackgroundRatio Larger");
    //         } else {
    //             if (Screen.height > bg.bounds.size.y) {
    //                 Camera.main.orthographicSize = bg.bounds.size.y/2;
    //                 leftHand.transform.localScale *= 0.8f;
    //                 rightHand.transform.localScale *= 0.8f;
    //                 //Debug.Log("CameraScreenScale - ScreenRatio Larger - Screen taller than bg");
    //             } else {
    //                 Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
    //                 //Debug.Log("CameraScreenScale - ScreenRatio Larger - screen shorter than bg");
    //             }
    //         }
    //     }
    //     //Debug.Log("CameraScreenScale - Ortho Size Set: " + Camera.main.orthographicSize);
    // }

    public float GetSpriteAdjustmentRatio() {
        return spriteAdjustmentRatio;
    }
}//end of CameraScreenScale
