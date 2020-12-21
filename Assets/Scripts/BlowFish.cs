using UnityEngine;

public class BlowFish : FishController
{
    // User/Unity Adjustable Public Class Variables
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    public float pumpPowerMin;
    public float pumpPowerMax;
    public AudioClip inflateAudio;
    public AudioClip deflateAudio;
    public AudioClip swimAudio;
    public float rotationSpeed;

    // Private Class Variables
    private float pumpTimer = 0;
    private float pumpPower;
    private bool isInflated = false;
    private Vector3 pumpDirection = Vector3.up;

    void Start() {
        InitializeFish();
    }

    void OnEnable() {
        // Grab user options
        SetUserSpriteSize(PlayerPrefs.GetFloat("blowfishsize"));
    }

    void Update() {
        SetCameraProperties();
        PositionCheckVertical();
        PositionCheckHorizontal();
    }

    private void FixedUpdate() {
        HandleTouch();
        SetAnimatorShakeTrigger();

        // Only do blowfish animations and movement when not shaking and pump timer has been reached
        if (!IsShaking()) {
            PositionCheckVertical();
            // Head back to reset position
            if (IsResetTime()) {
                Rotate(rotationSpeed + 2);
                if (transform.rotation.z <= 0.02f && transform.rotation.z >= -0.02f) {
                    //Debug.Log("Blowfish - Reset Complete");
                    Vector3 currentPos = transform.position;
                    transform.position = new Vector3(currentPos.x, currentPos.y, 0);
                    SetResetTime(false);
                }
            }
            // Initiate a pump or count down the timer
            if (pumpTimer <= 0) {
                AnimateFish();
                MoveFish();
                // Change rotation direction once in a while if not inflated and not reset
                if (!isInflated && !IsResetTime()) {
                    FlipRotationDirection();
                }
                pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
            } else {
                pumpTimer -= Time.deltaTime;
            }
        } else {
            // Rotate while device is shaking
            Rotate(rotationSpeed);
        }

        // Play inflating and deflating sounds based on fish state
        if (IsShaking() && !isInflated) {
            PlaySFX(inflateAudio);
            isInflated = true;
        }
        if (!IsShaking() && isInflated) {
            PlaySFX(deflateAudio);
            isInflated = false;
        }
    }

    public void HandleTouch() {
        // Get touch position when the screen is touched
        if (Input.touchCount > 0) {
            // Handle all touches
            foreach (Touch touch in Input.touches) {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                // When a touch begins, grab its location and see if it is overlaping a collider2d object
                if (touch.phase == TouchPhase.Began && GetCollider2D() == Physics2D.OverlapPoint(touchPosition)) {
                    // Blowfish pump animation
                    GetAnimator().SetTrigger("isTapped");
                }
            }
        }
    }

    public override void AnimateFish() {
        // Blowfish pump animation
        GetAnimator().SetTrigger("pumpOnce");
        PlaySFX(swimAudio);
    }

    public override void MoveFish() {
        SetPumpPower();
        GetRigidbody2D().AddForce(pumpDirection * GetPumpPower(), ForceMode2D.Impulse);
    }

    public float GetPumpPower() {
        return pumpPower;
    }

    public void SetPumpPower() {
        pumpPower = Random.Range(pumpPowerMin, pumpPowerMax);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        /* logic for collisions with blowfish
        if (this.tag == "Fish" && other.tag == "Wall") {
            Debug.Log("Blowfish - BOUNCE");
            Bounce();
        }
        */
    }
    private void PositionCheckVertical() {
        //Vector3 screenPosition = GetFishOnScreenPosition();
        Vector3 screenPosition = this.transform.position;
        if (screenPosition.y > (GetScreenHeight() - GetFishHeight()/2)) {
            // Reset pump timer so fish does not pump
            pumpTimer = pumpMaxTime;
            // Clamp y to be inside the screen with a border of the height of the fish so it never actually goes too far off the top of screen
            if (screenPosition.y >= GetScreenHeight() + (GetFishHeight())) {
                screenPosition.y = Mathf.Clamp(screenPosition.y, (GetScreenHeight()), GetScreenHeight() + (GetFishHeight()));
                SetFishOnScreenPosition(screenPosition);
            }
        } else if (screenPosition.y <= (-GetScreenHeight() + GetFishHeight()/2)) {
            // If floating around in puffed mode, bounce off the bottom, otherwise just pump right away near bottom
            if (IsShaking()) {
                // Clamp y to be inside the screen with a border of half the height of the fish so it never actually goes past the bottom of screen
                screenPosition.y = Mathf.Clamp(screenPosition.y, (-GetScreenHeight() + (GetFishHeight()/2)), GetScreenHeight());
                SetFishOnScreenPosition(screenPosition);
                Bounce();
            } else {
                // Set pump timer to 0 so fish immediately pumps
                pumpTimer = 0;
            }
        }
    }
    private void PositionCheckHorizontal() {
        //Vector3 screenPosition = GetFishOnScreenPosition();
        Vector3 screenPosition = this.transform.position;
        // If fish goes off screen on one side, pop him on the other side
        // Adjust width for inflated or not
        float width;
        if (!isInflated) {
            width = GetFishWidth()/2/2;
        } else {
            width = GetFishWidth()/2;
        }

        if (screenPosition.x < (-GetScreenWidth() - width)) {
            screenPosition.x = GetScreenWidth() + width;

        } else if (screenPosition.x > (GetScreenWidth() + width)) {
            screenPosition.x = -GetScreenWidth() - width;
        }
        SetFishOnScreenPosition(screenPosition);
    }

    // Logic for bouncing off walls
    private void Bounce() {
        Vector3 newVelocity = GetRigidbody2D().velocity * -1;
        GetRigidbody2D().velocity = newVelocity;
    }
}//end of BlowFish
