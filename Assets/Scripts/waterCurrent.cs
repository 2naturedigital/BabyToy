using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour {

   List<Rigidbody2D> bubblesInCurrent = new List<Rigidbody2D>();
   public float currentStrength;
   public float minStrength = (float)0.001;
   public float maxStrength = (float)0.005;
   public Vector2 currentDirection;
   public bool alternatingCurrent = true;
   public int movementPeriod = 3;
   private float elapsedTime = 0;


    private void OnTriggerEnter2D(Collider2D col) {
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null)
            bubblesInCurrent.Add(objectRigid);
    }


    private void OnTriggerExit2D(Collider2D col) {
        Rigidbody2D objectRigid = col.gameObject.GetComponent<Rigidbody2D>();
        if (objectRigid != null)
            bubblesInCurrent.Remove(objectRigid);
    }

    private Vector2 newDirection() {
        // return a current that moves left or right or no direction at all
        return new Vector2(Random.Range(-1, 2), 0);
    }

    private float newStrength() {
        // return a new strength between the min and max provided
        return Random.Range(minStrength, maxStrength);
    }


    private void FixedUpdate() {
        if (bubblesInCurrent.Count > 0) {
            foreach (Rigidbody2D bubble in bubblesInCurrent) {
                if (bubble != null) {
                    bubble.AddForce(currentStrength * currentDirection);
                }
            }
        }
    }

    void Update() {
        // only do this when currents can alternate
        if (alternatingCurrent) {
            elapsedTime += Time.deltaTime;
            // after movementPeriod has been reached, get a new direction and speed for the current
            if (elapsedTime > movementPeriod) {
                elapsedTime = 0;
                currentDirection = newDirection();
                currentStrength = newStrength();
            }
        }
    }
}
