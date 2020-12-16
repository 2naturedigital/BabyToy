using UnityEngine;

public class DragAndDrop : MonoBehaviour {
    public bool moveAllowed;
    private Collider2D col;

    private FishController thisFish;

    void Start() {
        col = GetComponent<Collider2D>();
    }

    void Awake() {
        thisFish = GameObject.FindObjectOfType<FishController>();
    }

    void Update() {
        if (Input.touchCount > 0) {
            foreach (Touch touch in Input.touches) {
                // Translates the position on the screen that has been touched to the scene world position
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

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
                        transform.position = new Vector3(touchPosition.x, touchPosition.y);
                    }
                }

                // Turn off movable when touch is ended
                if (touch.phase == TouchPhase.Ended) {
                    moveAllowed = false;
                }
            }
        }
    }
}//end of DragAndDrop
