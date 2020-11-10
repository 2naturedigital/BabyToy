using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles : MonoBehaviour
{

    public Animator animator;
    public GameObject bubbleOriginal;
    Collider2D col;


    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        GameObject bubbleClone = Instantiate(bubbleOriginal);
    }

    // Update is called once per frame
    void Update()
    {

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