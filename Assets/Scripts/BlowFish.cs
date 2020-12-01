using UnityEngine;

public class BlowFish : FishController
{
    private const int FISHWIDTH = 440;
    private const int FISHHEIGHT = 380;
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;
    public float pumpPowerMin;
    public float pumpPowerMax;
    private float pumpPower;
    private Vector2 pumpDirection;
    Rigidbody2D blowFish;
    public AudioSource audioSrc;
    public AudioClip inflateAudio;
    public AudioClip deflateAudio;
    public AudioClip swimAudio;
    private bool isInflated = false;

    // Start is called before the first frame update
    void Start() {
        sndCtrl = FindObjectOfType<SoundController>();
        blowFish = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        pumpDirection = Vector2.up;
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        PositionCheckVertical();
        PositionCheckHorizontal();
    }

    private void FixedUpdate() {
        SetAnimatorShakeTrigger();
        // only do blowfish animations and movement when not shaking and pump timer has been reached
        if (!IsShaking()) {
            if (pumpTimer <= 0) {
                AnimateFish();
                MoveFish();
                //if 
                pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
            } else {
                pumpTimer -= Time.deltaTime;
            }
        } else {
            //base.MoveFish();
        }

        if (IsShaking() && !isInflated) {
            InflateSFX();
            isInflated = true;
        }
        if (!IsShaking() && isInflated) {
            DeflateSFX();
            isInflated = false;
        }
    }

    //blowfish pump animation
    public override void AnimateFish() {
        animator.SetTrigger("pumpOnce");
        //audioSrc.clip = swimAudio;
        //audioSrc.PlayOneShot(audioSrc.clip);
        PlaySFX(swimAudio);
    }

    public override void MoveFish() {
        SetPumpPower();
        blowFish.AddForce(pumpDirection * pumpPower, ForceMode2D.Impulse);
    }

    private void InflateSFX() {
        // audioSrc.clip = inflateAudio;
        // audioSrc.PlayOneShot(audioSrc.clip);
        PlaySFX(inflateAudio);
    }

    private void DeflateSFX() {
        // audioSrc.clip = deflateAudio;
        // audioSrc.PlayOneShot(audioSrc.clip);
        PlaySFX(deflateAudio);
    }

    public float GetPumpPower() {
        return pumpPower * GetShakeMultiplier();
    }

    public void SetPumpPower() {
        pumpPower = Random.Range(pumpPowerMin, pumpPowerMax) * GetShakeMultiplier();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (this.tag == "Fish" && other.tag == "Wall") {
            //Debug.Log("BOUNCE");
            //Bounce();
        }
    }

    private void PositionCheckVertical() {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (screenPosition.y > (Screen.height - FISHHEIGHT/2) || screenPosition.y < (0f + FISHHEIGHT/2)) {
            // x is clamped outside the bounds of the screen so the fish can go off screen for wrapping purposes
            //screenPosition.x = Mathf.Clamp(screenPosition.x, 0f - FISHWIDTH*2, Screen.width + FISHWIDTH*2);
            // y is clamped inside the screen with a border of half the height of the fish so it never actually goes past the screen
            screenPosition.y = Mathf.Clamp(screenPosition.y, 0f, Screen.height);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);
            Bounce();
        }
    }

    private void PositionCheckHorizontal() {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (screenPosition.x < 0f - FISHWIDTH/2) {
            // if fish goes off screen on one side, pop him on the other side
            screenPosition.x = Screen.width;
            //screenPosition.y = Mathf.Clamp(screenPosition.y, 0f + 380/2, Screen.height - 380/2);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);
        } else if (screenPosition.x > Screen.width + FISHWIDTH/2) {
            // if fish goes off screen on one side, pop him on the other side
            screenPosition.x = 0f;
            //screenPosition.y = Mathf.Clamp(screenPosition.y, 0f + 380/2, Screen.height - 380/2);
            Vector3 newWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            this.transform.position = new Vector2(newWorldPosition.x, newWorldPosition.y);
        }
    }

    private void Bounce() {
        Vector2 newVelocity = blowFish.velocity * -1;
        blowFish.velocity = newVelocity;
    }
}
