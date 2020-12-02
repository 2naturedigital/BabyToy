using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenScale : MonoBehaviour {
    public bool maintainWidth = false;

    [Range(-1,1)]
    public int adaptPosition;

    float defaultWidth;
    float defaultHeight;
    Vector3 CameraPos;
    public SpriteRenderer bg;

    // Start is called before the first frame update
    void Start() {
        // calculate screen ration
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio;
        if (bg.bounds.size.x <= bg.bounds.size.y) {
            targetRatio = bg.bounds.size.x / bg.bounds.size.y;
        } else {
            targetRatio = bg.bounds.size.y / bg.bounds.size.x;
        }
        Debug.Log("Screen W: " + Screen.width + " Screen H: " + Screen.height);
        Debug.Log("Screen Ratio: " + screenRatio);
        Debug.Log("Target W: " + bg.bounds.size.x + " Target H: " + bg.bounds.size.y);
        Debug.Log("Target Ratio: " + targetRatio);

        if (screenRatio >= targetRatio) {
            Camera.main.orthographicSize = bg.bounds.size.y / 2;
            // set for width
            // Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
            Debug.Log("Screen Larger so Ortho Size is: " + Camera.main.orthographicSize);
        } else {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = bg.bounds.size.y / 2 * differenceInSize;
            // set for width
            // ???
            Debug.Log("Screen Smaller so Ortho Size is: " + Camera.main.orthographicSize);
        }


        // CameraPos = Camera.main.transform.position;
        // defaultHeight = Camera.main.orthographicSize;
        // defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    // Update is called once per frame
    void Update() {
        // if (maintainWidth) {
        //     Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;

        //     Camera.main.transform.position = new Vector3(CameraPos.x, adaptPosition * (defaultHeight - Camera.main.orthographicSize), CameraPos.z);
        // } else {
        //     Camera.main.transform.position = new Vector3(adaptPosition * (defaultWidth - Camera.main.orthographicSize * Camera.main.aspect), CameraPos.y, CameraPos.z);
        // }
    }
}
