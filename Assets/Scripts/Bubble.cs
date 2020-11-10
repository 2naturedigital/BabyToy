using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Animator animator;
    Collider2D col;
    private float bubbleVelocity = 0;
    public int bubbleSpeed = 1000;
    private int direction = 0;
    public int movementPeriod = 3;
    private float elapsedTime = 0;



    void start() {
        bubbleVelocity = bubbleSpeed * Time.deltaTime;
        col = GetComponent<Collider2D>();
        //set rando bubble sounds here
    }
    
    
    void Update() {
        //set bubble movements here        

        if (elapsedTime > movementPeriod) {
            direction = Random.Range(0, 3);
            switch (direction) {
                case 1:
                GetComponent<Rigidbody2D>().velocity = Vector2.left * bubbleVelocity;
                break;

                case 2:
                GetComponent<Rigidbody2D>().velocity = Vector2.right * bubbleVelocity;
                break;
            }
            elapsedTime = 0;            
        } else {
            elapsedTime += Time.deltaTime;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // When a touch begins, grab its location and see if it is overlaping a collider2d object
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if (col == touchedCollider)
                {
                    animator.SetTrigger("Touched");
                }
            }            
        }
    }
}
