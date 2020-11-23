using UnityEngine;

public class BlowFish : FishController
{
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;
    public float waitTimer = 0;
    private bool isWaiting = false;
    public float pumpPower;
    public float gravity;
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
        if (!isWaiting) {
            if (pumpTimer <= 0) {
                AnimateFish();
                MoveFish();
                waitTimer = Random.Range(pumpMinTime, pumpMinTime + 2);
                isWaiting = true;
            } else {
                pumpTimer -= Time.deltaTime;
            }
        } else {
            if (waitTimer <= 0) {
                pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
                isWaiting = false;
            } else {
                waitTimer -= Time.deltaTime;
            }
        }
    }

    //blowfish pump animation
    public override void AnimateFish()
    {
        base.AnimateFish();
        animator.SetTrigger("pumpOnce");
    }

    public override void MoveFish()
    {
        //base.MoveFish();
        blowFish.AddForce(pumpDirection * pumpPower);
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
