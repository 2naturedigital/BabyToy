using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]

public class CameraScreenScale : MonoBehaviour {
    public SpriteRenderer bg;
    public Button button;
    public CanvasRenderer canvasRenderer;
    public float holdTimer = 5.0f;
    private float elapsedTime = 0;
    public SpriteRenderer waterLayer;
    public SpriteRenderer leftHand;
    public SpriteRenderer rightHand;
    public Sprite[] backgrounds;
    private float spriteAdjustmentRatio;
    private int screenHeight;
    private int screenWidth;
    private bool landscape;
    private Camera cameraMain;
    // public bool maintainWidth = false;
    // [Range(-1,1)]
    // public int adaptPosition;
    // private Vector3 CameraPos;
    // private float defaultWidth;
    // private float defaultHeight;

    private void Awake() {
        cameraMain = Camera.main;
    }

    void Start() {
        // Calculate screen ratio
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        CalculateScreen();


        /*In case we decide to go with landscape rotation **
        CameraPos = cameraMain.transform.position;
        defaultHeight = cameraMain.orthographicSize;
        defaultWidth = cameraMain.orthographicSize * cameraMain.aspect;
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
        //     cameraMain.orthographicSize = defaultWidth / cameraMain.aspect;
        //     cameraMain.transform.position = new Vector3(CameraPos.x, adaptPosition * (defaultHeight - cameraMain.orthographicSize), CameraPos.z);
        // } else {
        //     cameraMain.transform.position = new Vector3(adaptPosition * (defaultWidth - cameraMain.orthographicSize * cameraMain.aspect), CameraPos.y, CameraPos.z);
        // }
    }

    public void OnClick() {
        if (Input.GetMouseButtonUp(0)) {
            SceneManager.LoadScene("Menu");
        }
        // while (Input.touches[0].phase == TouchPhase.Stationary) {
        //     elapsedTime += Time.deltaTime;
        //     if (elapsedTime >= holdTimer) {
        //         SceneManager.LoadScene("Menu");
        //     }
        // }
        // if (Input.touches[0].phase == TouchPhase.Ended) {
        //     elapsedTime = 0;
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
            cameraMain.orthographicSize = bg.bounds.size.y/2;
            leftHand.transform.localScale *= 0.8f;
            rightHand.transform.localScale *= 0.8f;
        } else {
            cameraMain.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
        }

        // Move the hands to the sides of the screen
        float newLeftX = -(cameraMain.orthographicSize * cameraMain.aspect) + leftHand.bounds.size.x/2;
        float newLeftY = -(cameraMain.orthographicSize) + leftHand.bounds.size.y/2;
        float newRightX = (cameraMain.orthographicSize * cameraMain.aspect) - rightHand.bounds.size.x/2;
        float newRightY = -(cameraMain.orthographicSize) + rightHand.bounds.size.y/2;
        leftHand.transform.position = new Vector3(newLeftX,newLeftY,0);
        rightHand.transform.position = new Vector3(newRightX,newRightY,0);

        // Move Parent Button
        float newX = -(cameraMain.orthographicSize * cameraMain.aspect) - canvasRenderer.transform.position.x/2;
        float newY = (cameraMain.orthographicSize) - canvasRenderer.transform.position.y/2;
        Debug.Log("btn x: " + newX + " btn y: " + newY);
        button.transform.position = new Vector3(newX,newY,0);
        canvasRenderer.transform.position = new Vector3(newX,newY,0);
        Debug.Log("Button: " + button.transform.position);
        Debug.Log("CanvasRenderer: " + canvasRenderer.transform.position);
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
    //             cameraMain.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
    //             //Debug.Log("CameraScreenScale - BackgroundRatio Larger");
    //         } else {
    //             cameraMain.orthographicSize = bg.bounds.size.y/2;
    //             //Debug.Log("CameraScreenScale - ScreenRatio Larger");
    //         }
    //     } else {
    //         //Debug.Log("CameraScreenScale - Portrait");
    //         if (backgroundRatio > screenRatio) {
    //             cameraMain.orthographicSize = bg.bounds.size.y/2;
    //             leftHand.transform.localScale *= 0.8f;
    //             rightHand.transform.localScale *= 0.8f;
    //             //Debug.Log("CameraScreenScale - BackgroundRatio Larger");
    //         } else {
    //             if (Screen.height > bg.bounds.size.y) {
    //                 cameraMain.orthographicSize = bg.bounds.size.y/2;
    //                 leftHand.transform.localScale *= 0.8f;
    //                 rightHand.transform.localScale *= 0.8f;
    //                 //Debug.Log("CameraScreenScale - ScreenRatio Larger - Screen taller than bg");
    //             } else {
    //                 cameraMain.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
    //                 //Debug.Log("CameraScreenScale - ScreenRatio Larger - screen shorter than bg");
    //             }
    //         }
    //     }
    //     //Debug.Log("CameraScreenScale - Ortho Size Set: " + cameraMain.orthographicSize);
    // }

    public float GetSpriteAdjustmentRatio() {
        return spriteAdjustmentRatio;
    }
}//end of CameraScreenScale
