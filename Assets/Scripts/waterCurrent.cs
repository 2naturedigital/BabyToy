using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour {

    private List<Rigidbody2D> bubblesInCurrent = new List<Rigidbody2D>();
    public float currentStrength;
    public float currentStrengthDuringShake;
    private float shakeMult;
    public Vector2 currentDirection;
    private bool isShaking = false;

    void Start() {
    }

    public void StartShake(Vector3 mult) {
        isShaking = true;
        shakeMult = mult.sqrMagnitude;
    }

    public void ContinueShake(Vector3 mult) {
        // Do anything needed on a continued shake
        shakeMult = mult.sqrMagnitude;
    }

    public void EndShake() {
        isShaking = false;
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
                if (isShaking) {
                    bubble.AddForce(currentStrengthDuringShake * shakeMult * currentDirection);
                } else {
                    bubble.AddForce(currentStrength * currentDirection);
                }
            }
        }
    }
}//end of WaterCurrent
