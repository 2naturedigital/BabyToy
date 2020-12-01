using System.Collections.Generic;
using UnityEngine;

public class WaterCurrent : MonoBehaviour {

   List<Rigidbody2D> bubblesInCurrent = new List<Rigidbody2D>();
   private float currentStrength;
   public float minStrength;
   public float maxStrength;
   public Vector2 currentDirection;
   public bool alternatingCurrent;
   private float currentMovementPeriod = 5f;
   //public int minMovementPeriod;
   //public int maxMovementPeriod;
   private float elapsedTime = 0;

   private void Start() {
       // initialize direction, strength, and movement period for current
       NewStrength();
       //NewMovementPeriod();
   }


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

    private void NewDirection() {
        // set a current that moves left or right or no direction at all
        // if (currentDirection == null) {
        //     currentDirection = new Vector2(Random.Range(-1, 1), 0);
        // }
        if (currentDirection == Vector2.left) {
            currentDirection = Vector2.right;
        } 
        if (currentDirection == Vector2.right) {
            currentDirection = Vector2.left;
        }
    }

    private void NewStrength() {
        // set a new strength between the min and max provided
        currentStrength = Random.Range(minStrength, maxStrength);
    }

    private void NewMovementPeriod() {
        // set a new movement period from the min to max provided
        //currentMovementPeriod = Random.Range(minMovementPeriod, maxMovementPeriod);
    }


    private void FixedUpdate() {
        foreach (Rigidbody2D bubble in bubblesInCurrent) {
            if (bubble != null) {
                bubble.AddForce(currentStrength * currentDirection);
            }
        }
    }

    void Update() {
        // only do this when currents can alternate
        if (alternatingCurrent) {
            elapsedTime += Time.deltaTime;
            // after movementPeriod has been reached, get a new direction and speed for the current
            if (elapsedTime > currentMovementPeriod) {
                elapsedTime = 0;
                NewDirection();
                //NewStrength();
                //NewMovementPeriod();
            }
        }
    }
}
