using UnityEngine;

public class BlowFish : FishController
{
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;
    public float pumpPower;
    private Vector2 pumpDirection;
    // public float waitTimer = 0;
    // private bool isWaiting = false;
    //public float gravity;
    Rigidbody2D blowFish;
    public AudioSource audioSrc;
    public AudioClip inflateAudio;
    public AudioClip deflateAudio;
    public AudioClip swimAudio;
    private bool isInflated = false;

    // Start is called before the first frame update
    void Start() {
        blowFish = GetComponent<Rigidbody2D>();
        pumpDirection = Vector2.up;
        audioSrc = GetComponent<AudioSource>();
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        MoveFish();    
    }

    private void FixedUpdate() {
        AnimateFish();

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
        base.AnimateFish();        
        if (!IsShaking()) {
            if (pumpTimer <= 0) {
                animator.SetTrigger("pumpOnce");
                audioSrc.clip = swimAudio;
                audioSrc.PlayOneShot(audioSrc.clip);
                pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);            
            } else {
                pumpTimer -= Time.deltaTime;
            }
        }
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

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
