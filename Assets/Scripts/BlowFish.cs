using UnityEngine;

public class BlowFish : FishController
{
    private const int FISHWIDTH = 440;
    private const int FISHHEIGHT = 380;
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    private float pumpTimer = 0;
    public float pumpPowerMin;
    public float pumpPowerMax;
    private float pumpPower;
    private Vector2 pumpDirection;
    private Rigidbody2D blowFish;
    public AudioClip inflateAudio;
    public AudioClip deflateAudio;
    public AudioClip swimAudio;
    private bool isInflated = false;

    void Start() {
        Debug.Log("BlowFish Started");
        SetCameraProperties();
        SetSoundController(FindObjectOfType<SoundController>());
        SetAnimator(GetComponent<Animator>());
        SetFishSize(FISHWIDTH, FISHHEIGHT);
        blowFish = GetComponent<Rigidbody2D>();
        pumpDirection = Vector2.up;
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
        blowFish.AddForce(pumpDirection * GetPumpPower(), ForceMode2D.Impulse);
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
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (screenPosition.y > (Screen.height - FISHHEIGHT/2)) {
            // Reset pump timer so he does not pump
            pumpTimer = pumpMaxTime;
        } else if (screenPosition.y < (0f + FISHHEIGHT/2)) {
            // Set pump timer to 0 so he immediately pumps
            pumpTimer = 0;
        }
    }
    private void PositionCheckHorizontal() {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        // If fish goes off screen on one side, pop him on the other side
        if (screenPosition.x < 0f - FISHWIDTH/2) {
            screenPosition.x = Screen.width + FISHWIDTH/2;

        } else if (screenPosition.x > Screen.width + FISHWIDTH/2) {
            screenPosition.x = 0f - FISHWIDTH/2;
        }
        Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);
    }

    /* Logic for bouncing off walls
    private void Bounce() {
        Vector2 newVelocity = blowFish.velocity * -1;
        blowFish.velocity = newVelocity;
    }
    */
}//end of BlowFish
