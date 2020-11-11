using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterCurrent : MonoBehaviour {
   
   List<Rigidbody2D> bubblesInCurrent = new List<Rigidbody2D>();
   public float currentStrength;
   public Vector2 currentDirection;


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


    private void FixedUpdate() {        
        if (bubblesInCurrent.Count > 0) {
            foreach (Rigidbody2D bubble in bubblesInCurrent) {
                if (bubble == null) {
                    bubblesInCurrent.Remove(bubble);
                } else {
                    bubble.AddForce(currentStrength * currentDirection);
                }                
            }
        }
    }
}