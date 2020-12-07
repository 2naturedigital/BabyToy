﻿using UnityEngine;

public class CameraScreenScale : MonoBehaviour {
    public SpriteRenderer bg;
    // public bool maintainWidth = false;
    // [Range(-1,1)]
    // public int adaptPosition;
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
        Debug.Log("screen ratio is: " + screenRatio);
        Debug.Log("target ratio is: " + targetRatio);

        if (screenRatio >= targetRatio) {
            //Debug.Log("BG Size Is: " + bg.bounds.size.y);
            float differenceInSize = screenRatio / targetRatio;
            Debug.Log("1 ratio diff is: " + differenceInSize);
            Camera.main.orthographicSize = bg.bounds.size.y/2;
            Debug.Log("1 Ortho Size Set: " + Camera.main.orthographicSize);
            // Set for landscape
            // Camera.main.orthographicSize = bg.bounds.size.x * Screen.height / Screen.width * 0.5f;
            //Debug.Log("Screen Larger so Ortho Size is: " + Camera.main.orthographicSize);
        } else {
            float differenceInSize = targetRatio / screenRatio;
            Debug.Log("2 ratio diff is: " + differenceInSize);
            Camera.main.orthographicSize = bg.bounds.size.y/2 * differenceInSize;
            Debug.Log("2 Ortho Size Set: " + Camera.main.orthographicSize);
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
