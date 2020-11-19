using UnityEngine;

public class BlowFish : FishController
{
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;
    public float pumpPower;
    private Vector2 pumpDirection;
    Rigidbody2D blowFish;

    // Start is called before the first frame update
    void Start() {
        blowFish = GetComponent<Rigidbody2D>();
        pumpDirection = Vector2.up;
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        AnimateFish();
    }

    //blowfish pump animation
    public override void AnimateFish()
    {
        base.AnimateFish();
        if (pumpTimer <= 0) {
            animator.SetTrigger("pumpOnce");
            MoveFish();            
            pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
        } else {
            pumpTimer -= Time.deltaTime;
        }
    }

    public override void MoveFish()
    {
        base.MoveFish();
        //blowFish.velocity = pumpDirection * pumpPower;
        blowFish.AddForce(pumpDirection * pumpPower);
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
