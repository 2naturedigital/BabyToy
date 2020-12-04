using UnityEngine;

public class CameraScreenScale : MonoBehaviour {
    public bool maintainWidth = false;

    [Range(-1,1)]
    public int adaptPosition;
    public SpriteRenderer bg;
    // private Vector3 CameraPos;
    // private float defaultWidth;
    // private float defaultHeight;

    void Start() {
        //Debug.Log("CameraScreenScale Started");
        // Calculate screen ratio
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio;
        if (bg.bounds.size.x <= bg.bounds.size.y) {
            targetRatio = bg.bounds.size.x / bg.bounds.size.y;
        } else {
            targetRatio = bg.bounds.size.y / bg.bounds.size.x;
        }

        if (screenRatio >= targetRatio) {
            Camera.main.orthographicSize = bg.bounds.size.y / 2;
            // Set for landscape
            // Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
            //Debug.Log("Screen Larger so Ortho Size is: " + Camera.main.orthographicSize);
        } else {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = bg.bounds.size.y / 2 * differenceInSize;
            // Set for landscape
            // Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f  * differenceInSize;
            //Debug.Log("Screen Smaller so Ortho Size is: " + Camera.main.orthographicSize);
        }

        /*In case we decide to go with landscape rotation **
        CameraPos = Camera.main.transform.position;
        defaultHeight = Camera.main.orthographicSize;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        */
    }

    /*In case we decide to go with landscape rotation **
    void Update() {
        if (maintainWidth) {
            Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;
            Camera.main.transform.position = new Vector3(CameraPos.x, adaptPosition * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
        } else {
            Camera.main.transform.position = new Vector3(adaptPosition * (defaultWidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
        }
    }
    */
}//end of CameraScreenScale
