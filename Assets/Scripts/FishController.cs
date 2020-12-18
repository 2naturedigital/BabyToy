using UnityEngine;
//using UnityEngine.SceneManagement;

public class FishController : MonoBehaviour {
    // User/Unity Adjustable Public Class Variables
    public float minSpeed;
    public float maxSpeed;

    // Private Class Variables
    private float fishWidth;
    private float fishHeight;
    private float speed;
    private bool isFacingLeft = true;
    private bool isShaking = false;
    private bool isResetTime = false;
    private Animator animator;
    private float magnitudeMult = 1;
    private float shakeForceMultiplier = 1;
    private SoundController sndCtrl;
    private Vector3 CameraPos;
    private float screenWidth;
    private float screenHeight;
    // Keep track of current target position
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rBody2D;
    private Collider2D col2D;
    private CameraScreenScale cameraScreenScale;
    private float spriteAdjustmentRatio;
    public float userSpriteSize;
    private int rotationDirection = -1;


    // Start(), Update(), FixedUpdate() etc. are handled in the subclasses
    public void InitializeFish() {
        SetSoundController();
        SetAnimator();
        SetRigidbody2D();
        SetCollider2D();
        SetSpriteRenderer();
        SetCameraProperties();
        SetFishSize();
        SetFishStartingPoints();
    }

    // POSITION & TARGET RELATED
    // Do a movement transformation if the target position and the current position don't match, otherwise fetch new target
    public virtual void MoveFish() {
        SetSpeed();
        if ((Vector3)transform.position != targetPosition) {
            // Flip the sprite to face the right direction when swimming
            if (transform.position.x > targetPosition.x && !isFacingLeft) {
                FlipHorizontal();
            } else if (transform.position.x < targetPosition.x && isFacingLeft) {
                FlipHorizontal();
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * GetSpeed());
        } else {
            SetRandomTarget();
        }
    }
    public void SetTarget(Vector3 newTarget) {
        //Debug.Log("FishController - " + this + " Is changing to x: " + newTarget.x + " " + newTarget.y);
        targetPosition = newTarget;
    }
    public Vector3 GetFishOnScreenPosition() {
        return Camera.main.WorldToScreenPoint(this.transform.position);
    }
    public void SetFishOnScreenPosition(Vector3 pos) {
        this.transform.position = pos; //Camera.main.ScreenToWorldPoint(pos);
    }
    public Vector3 GetTarget() {
        //Debug.Log("FishController - " + this + " Is going towards: " + targetPosition.x + " " + targetPosition.y);
        return targetPosition;
    }
    public void SetRandomTarget() {
        float randomX = Random.Range(CameraPos.x - screenWidth + (fishWidth/2), screenWidth - (fishWidth/2));
        float randomY = Random.Range(CameraPos.y - screenHeight + (fishHeight/2), screenHeight - (fishHeight/2));
        //Debug.Log("FishController - Min: " + (CameraPos.x - defaultWidth) + " Max: " + defaultWidth);
        //Debug.Log("FishController - Min: " + (CameraPos.y - defaultHeight) + " Max: " + defaultHeight);
        targetPosition = new Vector3(randomX, randomY);
    }
    public float GetSpeed() {
        return speed * magnitudeMult;
    }
    public void SetSpeed() {
        speed = Random.Range(minSpeed, maxSpeed);
    }


    // ANIMATION RELATED
    // Animate the object for shake
    public void SetAnimatorShakeTrigger() {
        animator.SetBool("isShaking", isShaking);
    }
    public virtual void AnimateFish() {
        // Animations implemented in each fish subclass
    }
    public void PlaySFX(AudioClip audioClip, float vol = 1f, float pitch = 1f) {
        sndCtrl.PlaySFX(audioClip, vol, pitch);
    }
    public void FlipHorizontal() {
        if (this.tag == "Fish") {
            isFacingLeft = !isFacingLeft;
            transform.Rotate(0, 180, 0);
        }
    }
    public int GetRotationDirection() {
        return rotationDirection;
    }
    public void Rotate(float speed) {
        float rotation = 0;
        rotation = rotationDirection * speed * GetShakeMultiplier();
        transform.Rotate(0, 0, rotation);
    }
    public void FlipRotationDirection() {
        rotationDirection *= -1;
    }


    // SHAKE RELATED
    public void StartShake(Vector3 mult, float shakeForceMult) {
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
        SetIsShaking(true);
    }
    public void ContinueShake(Vector3 mult, float shakeForceMult) {
        // Do anything needed on a continued shake
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }
    public void EndShake() {
        magnitudeMult = 1;
        shakeForceMultiplier = 1;
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
    public float GetShakeMultiplier() {
        return magnitudeMult;
    }


    // COLLISION RELATED
    private void OnTriggerEnter2D(Collider2D other) {
        /* Used for swapping targets on collision
        if (this.tag == "Fish" && other.tag == "Fish") {
            //Debug.Log("FishController - Collision");
            FishController otherFish = other.gameObject.GetComponent<FishController>();
            if (otherFish != null) {
                Vector3 thisTarget = this.GetTarget();
                Vector3 otherTarget = otherFish.GetTarget();
                this.SetTarget(otherTarget);
                otherFish.SetTarget(thisTarget);
            }
        }
        */
    }


    // STARTING OVER
    public void Reload() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // Initializing Setters
    public void SetSoundController() {
        sndCtrl = FindObjectOfType<SoundController>();
    }
    public void SetAnimator() {
        animator = GetComponent<Animator>();
    }
    public void SetRigidbody2D() {
        rBody2D = GetComponent<Rigidbody2D>();
    }
    public void SetCollider2D() {
        col2D = GetComponent<Collider2D>();
    }
    public void SetSpriteRenderer() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetCameraProperties() {
        CameraPos = Camera.main.transform.position;
        //Debug.Log("FishController - Ortho Size Is: " + Camera.main.orthographicSize);
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize;
        cameraScreenScale = FindObjectOfType<CameraScreenScale>();
    }
    public void SetFishSize() {
        spriteAdjustmentRatio = cameraScreenScale.GetSpriteAdjustmentRatio();
        spriteRenderer.transform.localScale *= spriteAdjustmentRatio;
        spriteRenderer.transform.localScale *= userSpriteSize;
        fishWidth = spriteRenderer.bounds.size.x;
        fishHeight = spriteRenderer.bounds.size.y;
    }
    public void SetUserSpriteSize(float size) {
        userSpriteSize = size;
    }
    public void SetFishStartingPoints() {
        // Clamp x and y to inside the screen for starting positions
        // This sets a randome target for each fish but then moves the fish to that target at the start of the game
        float randomX = Random.Range(CameraPos.x - screenWidth + (fishWidth/2), screenWidth - (fishWidth/2));
        float randomY = Random.Range(CameraPos.y - screenHeight + (fishHeight/2), screenHeight - (fishHeight/2));
        SetFishOnScreenPosition(new Vector3(randomX, randomY));
    }


    // Initializing Getters
    public SoundController GetSoundController() {
        return sndCtrl;
    }
    public Animator GetAnimator() {
        return animator;
    }
    public Rigidbody2D GetRigidbody2D() {
        return rBody2D;
    }
    public Collider2D GetCollider2D() {
        return col2D;
    }
    public SpriteRenderer GetSpriteRenderer() {
        return spriteRenderer;
    }
    public float GetScreenWidth() {
        return screenWidth;
    }
    public float GetScreenHeight() {
        return screenHeight;
    }
    public float GetFishWidth() {
        return fishWidth;
    }
    public float GetFishHeight() {
        return fishHeight;
    }
}//end of FishController
