using UnityEngine;

public class BlowFish : FishController
{
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;
    public float pumpPower;
    private Vector2 pumpDirection;
    Rigidbody2D blowFish;
    public AudioSource audioSrc;
    public AudioClip inflateAudio;
    public AudioClip deflateAudio;
    public AudioClip swimAudio;
    private bool isInflated = false;

    // Start is called before the first frame update
    void Start() {
        blowFish = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        pumpDirection = Vector2.up;
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void FixedUpdate() {
        SetAnimatorShakeTrigger();
        // only do blowfish animations and movement when not shaking and pump timer has been reached
        if (!IsShaking()) {
            if (pumpTimer <= 0) {
                AnimateFish();
                MoveFish();
                pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
            } else {
                pumpTimer -= Time.deltaTime;
            }
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
        audioSrc.clip = swimAudio;
        audioSrc.PlayOneShot(audioSrc.clip);
    }

    public override void MoveFish() {
        blowFish.AddForce(pumpDirection * pumpPower);
    }

    private void InflateSFX() {
        audioSrc.clip = inflateAudio;
        audioSrc.PlayOneShot(audioSrc.clip);
    }

    private void DeflateSFX() {
        audioSrc.clip = deflateAudio;
        audioSrc.PlayOneShot(audioSrc.clip);
    }

    public float GetPumpPower() {
        return pumpPower * GetShakeMultiplier();
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
