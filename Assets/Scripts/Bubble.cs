using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    private float bubbleVelocity = 0;
    public int bubbleSpeed = 1000;
    private int direction = 0;
    public int movementPeriod = 3;
    private float elapsedTime = 0;



    void start() {
        direction = Random.Range(1, 2);
        bubbleVelocity = bubbleSpeed * Time.deltaTime;
        //set rando bubble sounds here
    }
    
    
    void Update() {
        //set bubble movements here        

        if (elapsedTime > movementPeriod) {
            switch (direction) {
                case 1:
                GetComponent<Rigidbody2D>().velocity = Vector2.left * bubbleVelocity;
                break;

                case 2:
                GetComponent<Rigidbody2D>().velocity = Vector2.right * bubbleVelocity;
                break;
            }            
        } else {
            elapsedTime += Time.deltaTime;
        }        
    }
}
