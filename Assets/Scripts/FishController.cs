using UnityEngine;

public class FishController : MonoBehaviour {
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float speed;
    private bool isFacingLeft = true;
    public bool isShaking = false;
    public bool isResetTime = false;
    public Animator animator;

    // Keep track of current target position
    private Vector2 targetPosition;

    // Start is called before the first frame update
    void Start() {
        // Get a starting target position
        SetRandomTarget();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        MoveFish();
    }

    private void FixedUpdate() {
        SetAnimatorShakeTrigger();
        AnimateFish();
    }


    // TARGET RELATED
    // Do a movement transformation if the target position and the current position don't match
    public virtual void MoveFish() {
        if ((Vector2)transform.position != targetPosition) {
            // flip the sprite to face the right direction when swimming
            if (transform.position.x > targetPosition.x && !isFacingLeft) {
                FlipHorizontal();
            } else if (transform.position.x < targetPosition.x && isFacingLeft) {
                FlipHorizontal();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        } else {
            SetRandomTarget();
        }
    }

    public void SetTarget(Vector2 newTarget) {
        //Debug.Log(this + " Is changing to x: " + newTarget.x + " " + newTarget.y);
        targetPosition = newTarget;
    }

    public Vector2 GetTarget() {
        //Debug.Log(this + " Is going towards: " + targetPosition.x + " " + targetPosition.y);
        return targetPosition;
    }

    public void SetRandomTarget() {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        targetPosition = new Vector2(randomX, randomY);
    }


    // ANIMATION RELATED
    // Animate the object for shake
    public void SetAnimatorShakeTrigger() {
        animator.SetBool("isShaking", isShaking);
    }
    public virtual void AnimateFish() {
        // animations implemented in each fish subclass
    }

    public void FlipHorizontal() {
        if (this.tag == "Fish") {
            isFacingLeft = !isFacingLeft;
            //animator.transform.Rotate(0, 180, 0);
            transform.Rotate(0, 180, 0);
        }
    }

    public void Rotate() {
    }


    // COLLISION RELATED
    private void OnTriggerEnter2D(Collider2D other) {
        if (this.tag == "Fish" && other.tag == "Fish") {
            Debug.Log("Collision");
            FishController otherFish = other.gameObject.GetComponent<FishController>();
            if (otherFish != null) {
                Vector2 thisTarget = this.GetTarget();
                Vector2 otherTarget = otherFish.GetTarget();
                this.SetTarget(otherTarget);
                otherFish.SetTarget(thisTarget);
            }
        }
    }


    // SHAKE RELATED
    public void StartShake() {
        SetIsShaking(true);
    }

    public void EndShake() {
        SetIsShaking(false);
        SetResetTime(true);
    }

    public bool IsShaking() {
        return isShaking;
    }

    public void SetIsShaking(bool b) {
        isShaking = b;
    }

    public bool IsResetTime() {
        return isResetTime;
    }

    public void SetResetTime(bool b) {
        isResetTime = b;
    }


    // STARTING OVER
    public void Reload() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}//end of FishController
