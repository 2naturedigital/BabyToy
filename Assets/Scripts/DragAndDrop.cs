using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    bool moveAllowed;
    Collider2D col;

    private RandomPatrol randomPatrol;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void Awake()
    {
        randomPatrol = GameObject.FindObjectOfType<RandomPatrol>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // When a touch begins, rab its location and see if it is overlaping a collider2d object then set that object to moveable
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if (col == touchedCollider)
                {
                    moveAllowed = true;
                }
            }

            // Move the object to where the touch is moving
            if (touch.phase == TouchPhase.Moved)
            {
                if (moveAllowed)
                {
                    transform.position = new Vector2(touchPosition.x, touchPosition.y);
                }
            }

            // Turn off movable when touch is ended
            if (touch.phase == TouchPhase.Ended)
            {
                moveAllowed = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        RandomPatrol rp = other.gameObject.GetComponent<RandomPatrol>();
        Vector2 target = randomPatrol.GetCurrentTarget();
        randomPatrol.UpdateTarget(rp.GetCurrentTarget());
        rp.UpdateTarget(target);
    }
}
