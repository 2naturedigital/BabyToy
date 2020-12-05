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

    // Private Class Variables
    private float pumpTimer = 0;
    private float pumpPower;
    private bool isInflated = false;
    private Vector2 pumpDirection = Vector2.up;

    void Start() {
        InitializeFish();
        Debug.Log("BLOWFISH SAYS:");
        Debug.Log("Screen dot W: " + Screen.width + " Screen dot H: " + Screen.height);
        Debug.Log("Screen W: " + GetScreenWidth() + " Screen H: " + GetScreenHeight());
    }

    void Update() {
        PositionCheckVertical();
        PositionCheckHorizontal();
    }

    private void FixedUpdate() {
        SetAnimatorShakeTrigger();

        // Only do blowfish animations and movement when not shaking and pump timer has been reached
        if (!IsShaking()) {
            PositionCheckVertical();
            if (pumpTimer <= 0) {
                AnimateFish();
                MoveFish();
                pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
            } else {
                pumpTimer -= Time.deltaTime;
            }
        } else {
            //TODO: rotate the blowfish
            //base.MoveFish();
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
            Debug.Log("BOUNCE");
            Bounce();
        }
        */
    }
    private void PositionCheckVertical() {
        //float randomX = Random.Range(CameraPos.x - GetScreenWidth() + (GetFishWidth()/2), GetScreenWidth() - (GetFishWidth()/2));
        //float randomY = Random.Range(CameraPos.y - GetScreenHeight() + (GetFishHeight()/2), GetScreenHeight() - (GetFishHeight()/2));
        //Vector3 screenPosition = GetFishOnScreenPosition();
        Vector3 screenPosition = this.transform.position;
        if (screenPosition.y > (GetScreenHeight() - GetFishHeight()/2)) {// - GetFishHeight()/2)) {
            // Reset pump timer so fish does not pump
            pumpTimer = pumpMaxTime;
        } else if (screenPosition.y < (0f - GetScreenHeight() + GetFishHeight()/2)) { // + GetFishHeight()/2)) {
            // If floating around in puffed mode, bounce off the bottom, otherwise just pump right away near bottom
            if (IsShaking()) {
                // Clamp y to be inside the screen with a border of half the height of the fish so it never actually goes past the bottom of screen
                screenPosition.y = Mathf.Clamp(screenPosition.y, (0f - GetScreenHeight() + (GetFishHeight()/2)), GetScreenHeight());// + GetFishHeight()/2), GetScreenHeight());
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
        if (screenPosition.x < 0f - GetScreenWidth() + GetFishWidth()/2) {// - GetFishWidth()/2) {
            screenPosition.x = GetScreenWidth() - GetFishWidth()/2;// + GetFishWidth()/2;

        } else if (screenPosition.x > GetScreenWidth() - GetFishWidth()/2) {// + GetFishWidth()/2) {
            screenPosition.x = 0f - GetScreenWidth() + GetFishWidth()/2;// - GetFishWidth()/2;
        }
        SetFishOnScreenPosition(screenPosition);
    }

    // Logic for bouncing off walls
    private void Bounce() {
        Vector2 newVelocity = GetRigidbody2D().velocity * -1;
        GetRigidbody2D().velocity = newVelocity;
    }
}//end of BlowFish
