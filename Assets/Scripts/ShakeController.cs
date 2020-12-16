﻿using UnityEngine;

public class ShakeController : MonoBehaviour
{
    // Used to increase the magnintude of the shake all across the game
    public float shakeForceMultiplier;
    public float shakeResetTimer;
    public FishController[] fishies;
    public WaterCurrent[] waterCurrent;
    public BubblesDup bubblesDup;
    public TankCurrent tankCurrent;
    private float elapsedTime = 0;
    private bool isShaking = false;

    void OnEnable() {
        // Grab user volume options
        shakeForceMultiplier = PlayerPrefs.GetFloat("shakepower");
        Debug.Log("Shake Power: " + shakeForceMultiplier);
    }

    public void Shake(Vector3 deviceAcceleration) {
        if (!isShaking) {
            isShaking = true;
            foreach (var f in fishies) {
                f.StartShake(deviceAcceleration, shakeForceMultiplier);
                //Debug.Log("shaking a fish");
            }
            foreach (var c in waterCurrent) {
                c.StartShake(deviceAcceleration, shakeForceMultiplier);
                //Debug.Log("shaking a fish");
            }
            bubblesDup.StartShake(deviceAcceleration, shakeForceMultiplier);
            tankCurrent.StartShake(deviceAcceleration, shakeForceMultiplier);
        } else {
            foreach (var f in fishies) {
                f.ContinueShake(deviceAcceleration, shakeForceMultiplier);
                //Debug.Log("continue to shake");
            }
            foreach (var c in waterCurrent) {
                c.ContinueShake(deviceAcceleration, shakeForceMultiplier);
                //Debug.Log("shaking a fish");
            }
            bubblesDup.ContinueShake(deviceAcceleration, shakeForceMultiplier);
            tankCurrent.ContinueShake(deviceAcceleration, shakeForceMultiplier);
        }
        // Resets the elapsed timer since shaking is still happening
        elapsedTime = 0;
    }

    void Update() {
        // If shaking, countdown the timer to call endshake on objects
        if (isShaking) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > shakeResetTimer) {
                isShaking = false;
                foreach (var f in fishies) {
                    f.EndShake();
                }
                foreach (var c in waterCurrent) {
                    c.EndShake();
                    //Debug.Log("shaking a fish");
                }
                bubblesDup.EndShake();
                tankCurrent.EndShake();
                elapsedTime = 0;
            }
        }
    }
}//end of ShakeController
