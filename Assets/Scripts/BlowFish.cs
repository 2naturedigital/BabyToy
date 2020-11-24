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
    public AudioClip inflate;
    public AudioClip deflate;
    public AudioClip swim;
    private bool inflated = false;
    private bool deflated = true;

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
        //if (!isWaiting) {
            // if (pumpTimer <= 0) {
            //     AnimateFish();
            //     MoveFish();
            //     //waitTimer = Random.Range(pumpMinTime, pumpMinTime + 2);
            //     //isWaiting = true;
            // } else {
            //     pumpTimer -= Time.deltaTime;
            // }
        //} else {
            // if (waitTimer <= 0) {
            //     pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
            //     isWaiting = false;
            // } else {
            //     waitTimer -= Time.deltaTime;
            // }
        //}

        
    }

    private void FixedUpdate() {
        if (!isShaking) {
            if (pumpTimer <= 0) {
                AnimateFish();
                MoveFish();
                pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);            
            } else {
                pumpTimer -= Time.deltaTime;
            }
        }

        if (isShaking && !inflated) {
            Inflate();
            inflated = true;
            deflated = false;
        } 
        if (!isShaking && inflated) {
            Deflate();
            deflated = true;
            inflated = false;
        }
        
    }

    //blowfish pump animation
    public override void AnimateFish() {
        base.AnimateFish();
        animator.SetTrigger("pumpOnce");
        audioSrc.clip = swim;
        audioSrc.PlayOneShot(audioSrc.clip);
    }

    public override void MoveFish() {
        blowFish.AddForce(pumpDirection * pumpPower);
    }

    private void Inflate() {        
        audioSrc.clip = inflate;
        audioSrc.PlayOneShot(audioSrc.clip);
        animator.SetBool("isShaking", true);
    }

    private void Deflate() {
        audioSrc.clip = deflate;
        audioSrc.PlayOneShot(audioSrc.clip);
        animator.SetBool("isShaking", false);
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
