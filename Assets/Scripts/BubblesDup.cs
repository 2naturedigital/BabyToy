﻿using UnityEngine;

public class BubblesDup : MonoBehaviour
{
    public GameObject bubbleOriginal;
    public GameObject bubbleContainer;
    public float bubbleSpawnMinTime;
    public float bubbleSpawnMaxTime;
    public float bubbleMinScale;
    public float bubbleMaxScale;
    private bool isShaking = false;
    private float bubbleTimer = 0;
    public float shakeBubbleTimer;
    public int shakeBubbleCount;
    private Vector3 CameraPos;
    private float defaultWidth;
    private float defaultHeight;
    public AudioClip shake1;
    public AudioClip shake2;
    private SoundController sndCtrl;
    private float magnitudeMult = 1;
    private float shakeForceMultiplier = 1;


    void Start() {
        Debug.Log("BubblesDup Started");
        sndCtrl = FindObjectOfType<SoundController>();
        SetCameraProperties();
    }

    void Update() {
        bubbleTimer -= Time.deltaTime;
        CreateBubbles();
    }

    public void SetCameraProperties() {
        CameraPos = Camera.main.transform.position;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        defaultHeight = Camera.main.orthographicSize;
    }

    public void StartShake(Vector3 mult, float shakeForceMult) {
        isShaking = true;
        bubbleTimer = shakeBubbleTimer;
        sndCtrl.PlaySFX(shake1, 1f, 1f, true);
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void ContinueShake(Vector3 mult, float shakeForceMult) {
        // Do anything needed on a continued shake
        bubbleTimer = shakeBubbleTimer;
        sndCtrl.PlaySFX(shake2, 1f, 1f, true);
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void EndShake() {
        isShaking = false;
        magnitudeMult = 1;
        shakeForceMultiplier = 1;
    }

    public void CreateBubbles() {
        if (isShaking) {
            if (bubbleTimer <= 0) {
                MakeBubble(shakeBubbleCount);
                bubbleTimer = shakeBubbleTimer;
            }
        } else if (bubbleTimer <= 0) {
            MakeBubble(1);
            bubbleTimer = Random.Range(bubbleSpawnMinTime, bubbleSpawnMaxTime);
        }
    }

    public void MakeBubble(int count) {
        for (int i = 0; i < count; i++) {
            Vector3 bubblePosition = new Vector3(Random.Range(CameraPos.x - defaultWidth, defaultWidth), CameraPos.y - defaultHeight, 0f);
            GameObject bubbleClone = Instantiate(bubbleOriginal, bubblePosition, bubbleOriginal.transform.rotation);
            bubbleClone.transform.SetParent(bubbleContainer.transform);
            bubbleClone.transform.localScale *= Random.Range(bubbleMinScale, bubbleMaxScale);
        }
    }
}//end of BubblesDup
