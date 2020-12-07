using System.Collections.Generic;
using UnityEngine;

public class TankCurrent : MonoBehaviour {

   private List<Rigidbody2D> fishInCurrent = new List<Rigidbody2D>();
   private float currentStrength;
   public float minStrength;
   public float maxStrength;
   private Vector2 currentDirection;
   public bool alternatingCurrent;
   private int currentMovementPeriod;
   public int minMovementPeriod;
   public int maxMovementPeriod;
   private float elapsedTime = 0;
   private float magnitudeMult = 1;
   private float shakeForceMultiplier = 1;

   private void Start() {
       // Initialize direction, strength, and movement period for current
       NewDirection();
       NewStrength();
       NewMovementPeriod();
       fishInCurrent.Add(FindObjectOfType<BlowFish>().GetComponent<Rigidbody2D>());
   }

   void Update() {
        // Only do this when currents can alternate
        if (alternatingCurrent) {
            elapsedTime += Time.deltaTime;
            // After movementPeriod has been reached, get a new direction and speed for the current
            if (elapsedTime > currentMovementPeriod) {
                elapsedTime = 0;
                NewDirection();
                NewStrength();
                NewMovementPeriod();
            }
        }
    }

    void FixedUpdate() {
        foreach (Rigidbody2D fish in fishInCurrent) {
            if (fish != null) {
                fish.AddForce(currentStrength * currentDirection);
            }
        }
    }

   public void StartShake(Vector3 mult, float shakeForceMult) {
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void ContinueShake(Vector3 mult, float shakeForceMult) {
        // Do anything needed on a continued shake
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void EndShake() {
        magnitudeMult = 1;
        shakeForceMultiplier = 1;
    }

    private void NewDirection() {
        // Set a current that moves left or right or no direction at all
        currentDirection = new Vector2(Random.Range(-1, 2), 0);
    }

    private void NewStrength() {
        // Set a new strength between the min and max provided
        currentStrength = Random.Range(minStrength * magnitudeMult, maxStrength * magnitudeMult);
    }

    private void NewMovementPeriod() {
        // Set a new movement period from the min to max provided
        currentMovementPeriod = Random.Range(minMovementPeriod, maxMovementPeriod);
    }
}//end of FullWaterCurrent
