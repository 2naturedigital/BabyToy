using UnityEngine;

public class MovementController : MonoBehaviour {
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float speed;
    private bool isFacingLeft = true;
    private bool isShaking = false;

    // Keep track of current target position
    private Vector2 targetPosition;

    // Start is called before the first frame update
    void Start() {
        // Get a starting target position
        targetPosition = GetRandomTarget();
    }

    // Update is called once per frame
    void Update() {
        // Do a movement transformation if the target position and the current position don't match
        if ((Vector2)transform.position != targetPosition) {
            // flip the sprite to face the right direction when swimming
            if (transform.position.x > targetPosition.x && !isFacingLeft) {
                FlipHorizontal();
            } else if (transform.position.x < targetPosition.x && isFacingLeft) {
                FlipHorizontal();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        } else {
            targetPosition = GetRandomTarget();
        }
    }


    // TARGET RELATED
    public void SetTarget(Vector2 newTarget) {
        //Debug.Log(this + " Is changing to x: " + newTarget.x + " " + newTarget.y);
        targetPosition = newTarget;
    }

    public Vector2 GetTarget() {
        //Debug.Log(this + " Is going towards: " + targetPosition.x + " " + targetPosition.y);
        return targetPosition;
    }

    Vector2 GetRandomTarget() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }


    // ANIMATION RELATED
    void FlipHorizontal() {
        if (this.tag == "Fish") {
            isFacingLeft = !isFacingLeft;
            //animator.transform.Rotate(0, 180, 0);
            transform.Rotate(0, 180, 0);
        }
    }

    void Rotate() {
    }


    // COLLISION RELATED
    private void OnTriggerEnter2D(Collider2D other) {
        // Reload screen when there is a collision
        if (this.tag == "Fish" && other.tag == "Fish") {
            //Reload();
        }
    }


    // SHAKE RELATED
    public bool IsShaking() {
        return isShaking;
    }

    public void StartShake() {
        isShaking = true;
    }

    public void EndShake() {
        isShaking = false;
    }


    // STARTING OVER
    public void Reload() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}//end of MovementController
