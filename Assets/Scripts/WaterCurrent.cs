using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour {

    private List<Rigidbody2D> bubblesInCurrent = new List<Rigidbody2D>();
    public float currentStrength;
    public Vector3 currentDirection;
    public bool alternatingCurrent;
    private int currentMovementPeriod;
    public int minMovementPeriod;
    public int maxMovementPeriod;
    private float elapsedTime = 0;
    private float magnitudeMult = 1;
    private float shakeForceMultiplier = 1;


    void Start() {
    }

    void Update() {
        // Only do this when currents can alternate
        if (alternatingCurrent) {
            elapsedTime += Time.deltaTime;
            // After movementPeriod has been reached, get a new direction and speed for the current
            if (elapsedTime > currentMovementPeriod) {
                elapsedTime = 0;
                NewDirection();
                NewMovementPeriod();
            }
        }
    }

    public void StartShake(Vector3 mult, float shakeForceMult) {
        //isShaking = true;
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
        //shakeData = mult;
    }

    public void ContinueShake(Vector3 mult, float shakeForceMult) {
        // Do anything needed on a continued shake
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
        //shakeData = mult;
    }

    public void EndShake() {
        //isShaking = false;
        magnitudeMult = 1;
        shakeForceMultiplier = 1;
        //shakeData = new Vector3(0,0,0);
    }

    private void NewDirection() {
        // Set a current that moves left or right
        int x = Random.Range(0, 2);
        if (x == 0) {
            x = -1;
        }
        currentDirection = new Vector3(x, 0, 0);
    }

    private void NewMovementPeriod() {
        // Set a new movement period from the min to max provided
        currentMovementPeriod = Random.Range(minMovementPeriod, maxMovementPeriod);
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
                bubble.AddForce(currentStrength * magnitudeMult * shakeForceMultiplier * currentDirection);
            }
        }
    }
}//end of WaterCurrent
