using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Animator animator;
    Collider2D col;
    Rigidbody2D rb;



    void start() {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        //set rando bubble sounds here
    }    
    
    void Update() {      

        

    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);
    //        Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // When a touch begins, grab its location and see if it is overlaping a collider2d object
    //        if (touch.phase == TouchPhase.Began)
    //        {
    //            Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
    //            if (col == touchedCollider)
    //            {
    //                animator.SetTrigger("Touched");
    //            }
    //        }            
    //    }
    }
}
