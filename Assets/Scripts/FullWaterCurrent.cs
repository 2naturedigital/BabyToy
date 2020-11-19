using System.Collections.Generic;
using UnityEngine;

public class FullWaterCurrent : MonoBehaviour {

   List<Rigidbody2D> fishInCurrent = new List<Rigidbody2D>();
   private float currentStrength;
   public float minStrength;
   public float maxStrength;
   private Vector2 currentDirection;
   public bool alternatingCurrent;
   private int currentMovementPeriod;
   public int minMovementPeriod;
   public int maxMovementPeriod;
   private float elapsedTime = 0;

   private void Start() {
       // initialize direction, strength, and movement period for current
       NewDirection();
       NewStrength();
       NewMovementPeriod();
       fishInCurrent.Add(FindObjectOfType<BlowFish>().GetComponent<Rigidbody2D>());
   }


    private void OnTriggerEnter2D(Collider2D col) {
    }


    private void OnTriggerExit2D(Collider2D col) {
    }

    private void NewDirection() {
        // set a current that moves left or right or no direction at all
        currentDirection = new Vector2(Random.Range(-1, 2), 0);
    }

    private void NewStrength() {
        // set a new strength between the min and max provided
        currentStrength = Random.Range(minStrength, maxStrength);
    }

    private void NewMovementPeriod() {
        // set a new movement period from the min to max provided
        currentMovementPeriod = Random.Range(minMovementPeriod, maxMovementPeriod);
    }


    private void FixedUpdate() {
        foreach (Rigidbody2D fish in fishInCurrent) {
            if (fish != null) {
                fish.AddForce(currentStrength * currentDirection);
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
                NewStrength();
                NewMovementPeriod();
            }
        }
    }
}
