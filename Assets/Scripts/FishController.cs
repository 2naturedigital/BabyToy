﻿using UnityEngine;

public class FishController : MonoBehaviour {
    private int fishWidth;
    private int fishHeight;
    public float minSpeed;
    public float maxSpeed;
    private float speed;
    private bool isFacingLeft = true;
    private bool isShaking = false;
    private bool isResetTime = false;
    private Animator animator;
    private float shakeMultiplier = 1;
    private SoundController sndCtrl;
    private Vector3 CameraPos;
    private float defaultWidth;
    private float defaultHeight;
    // Keep track of current target position
    private Vector2 targetPosition;

    void Start() {
        Debug.Log("FishController Started");
        // Get Camera info
        SetCameraProperties();
        // Get a starting target position
        sndCtrl = FindObjectOfType<SoundController>();
        SetRandomTarget();
        animator = GetComponent<Animator>();
    }

    void Update() {
        MoveFish();
    }

    private void FixedUpdate() {
        SetAnimatorShakeTrigger();
        AnimateFish();
    }

    public void SetCameraProperties() {
        CameraPos = Camera.main.transform.position;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        defaultHeight = Camera.main.orthographicSize;
    }

    public float GetDefaultWidth() {
        return defaultWidth;
    }

    public float GetDefaultHeight() {
        return defaultHeight;
    }

    public SoundController GetSoundController() {
        return sndCtrl;
    }

    public void SetSoundController(SoundController sc) {
        sndCtrl = sc;
    }

    public void PlaySFX(AudioClip audioClip, float vol = 1f, float pitch = 1f) {
        sndCtrl.PlaySFX(audioClip, vol, pitch);
    }

    public Animator GetAnimator() {
        return animator;
    }

    public void SetAnimator(Animator a) {
        animator = a;
    }

    public void SetFishSize(int w, int h) {
        fishWidth = w;
        fishHeight = h;
    }

    public int GetFishWidth() {
        return fishWidth;
    }

    public int GetFishHeight() {
        return fishHeight;
    }


    // TARGET RELATED
    // Do a movement transformation if the target position and the current position don't match, otherwise fetch new target
    public virtual void MoveFish() {
        SetSpeed();
        if ((Vector2)transform.position != targetPosition) {
            // Flip the sprite to face the right direction when swimming
            if (transform.position.x > targetPosition.x && !isFacingLeft) {
                FlipHorizontal();
            } else if (transform.position.x < targetPosition.x && isFacingLeft) {
                FlipHorizontal();
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * GetSpeed());
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
        float randomX = Random.Range(CameraPos.x - defaultWidth + (GetFishWidth() / 2), defaultWidth - (GetFishWidth() / 2));
        float randomY = Random.Range(CameraPos.y - defaultHeight + (GetFishHeight() / 2), defaultHeight - (GetFishHeight() / 2));
        //Debug.Log("Min: " + (CameraPos.x - defaultWidth) + " Max: " + defaultWidth);
        //Debug.Log("Min: " + (CameraPos.y - defaultHeight) + " Max: " + defaultHeight);
        targetPosition = new Vector2(randomX, randomY);
    }

    public float GetSpeed() {
        return speed * shakeMultiplier;
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

    public void FlipHorizontal() {
        if (this.tag == "Fish") {
            isFacingLeft = !isFacingLeft;
            transform.Rotate(0, 180, 0);
        }
    }

    public void Rotate() {
    }


    // COLLISION RELATED
    private void OnTriggerEnter2D(Collider2D other) {
        /* Used for swapping targets on collision
        if (this.tag == "Fish" && other.tag == "Fish") {
            //Debug.Log("Collision");
            FishController otherFish = other.gameObject.GetComponent<FishController>();
            if (otherFish != null) {
                Vector2 thisTarget = this.GetTarget();
                Vector2 otherTarget = otherFish.GetTarget();
                this.SetTarget(otherTarget);
                otherFish.SetTarget(thisTarget);
            }
        }
        */
    }


    // SHAKE RELATED
    public void StartShake(Vector3 mult) {
        shakeMultiplier = mult.sqrMagnitude;
        //Debug.Log("Magnintude: " + shakeMultiplier);
        SetIsShaking(true);
    }
    public void ContinueShake(Vector3 mult) {
        // Do anything needed on a continued shake
        shakeMultiplier = mult.sqrMagnitude;
    }
    public void EndShake() {
        shakeMultiplier = 1;
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
        return shakeMultiplier;
    }


    // STARTING OVER
    public void Reload() {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}//end of FishController
