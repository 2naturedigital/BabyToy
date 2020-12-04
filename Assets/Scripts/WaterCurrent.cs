using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour {

    private List<Rigidbody2D> bubblesInCurrent = new List<Rigidbody2D>();
    public float currentStrength;
    public Vector2 currentDirection;
    private bool isShaking = false;
    private float magnitudeMult = 1;
    private float shakeForceMultiplier = 1;

    void Start() {
    }

    public void StartShake(Vector3 mult, float shakeForceMult) {
        isShaking = true;
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void ContinueShake(Vector3 mult, float shakeForceMult) {
        // Do anything needed on a continued shake
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void EndShake() {
        isShaking = false;
        magnitudeMult = 1;
        shakeForceMultiplier = 1;
    }

    private void OnTriggerEnter2D(Collider2D col) {
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        // Add only bubbles
        if (objectRigid != null && objectRigid.tag == "Bubble") {
            bubblesInCurrent.Add(objectRigid);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null) {
            bubblesInCurrent.Remove(objectRigid);
        }
    }

    private void FixedUpdate() {
        foreach (Rigidbody2D bubble in bubblesInCurrent) {
            if (bubble != null) {
                bubble.AddForce(currentStrength * magnitudeMult * currentDirection);
            }
        }
    }
}//end of WaterCurrent
