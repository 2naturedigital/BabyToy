using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour {
    bool moveAllowed;
    Collider2D col;

    private MovementController thisFish;

    // Start is called before the first frame update
    void Start() {
        col = GetComponent<Collider2D>();
    }

    void Awake() {
        thisFish = GameObject.FindObjectOfType<MovementController>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.touchCount > 0) {
            foreach (Touch touch in Input.touches) {
                // translates the position on the screen that has been touched to the scene world position
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // When a touch begins, grab its location and see if it is overlaping a collider2d object then set that object to moveable
                if (touch.phase == TouchPhase.Began) {
                    Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                    if (col == touchedCollider) {
                        moveAllowed = true;
                    }
                }

                // Move the object to where the touch is moving
                if (touch.phase == TouchPhase.Moved) {
                    if (moveAllowed) {
                        transform.position = new Vector2(touchPosition.x, touchPosition.y);
                    }
                }

                // Turn off movable when touch is ended
                if (touch.phase == TouchPhase.Ended) {
                    moveAllowed = false;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (this.tag == "Fish" && other.tag == "Fish") {
            Debug.Log("Collision of Fish!");
            MovementController otherFish = other.gameObject.GetComponent<MovementController>();
            Vector2 thisTarget = thisFish.GetTarget();
            Vector2 otherTarget = otherFish.GetTarget();
            thisFish.SetTarget(otherTarget);
            otherFish.SetTarget(thisTarget);
        }
    }
}
