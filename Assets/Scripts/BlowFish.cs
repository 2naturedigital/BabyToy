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
        AnimateFish();
        MoveFish();
    }

    //blowfish pump animation
    public override void AnimateFish()
    {
        base.AnimateFish();

        if (!isWaiting) {
            if (pumpTimer <= 0) {
                animator.SetTrigger("pumpOnce");
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

    public override void MoveFish()
    {
        //base.MoveFish();
        //blowFish.velocity = pumpDirection * pumpPower;
        if (pumpTimer <= 0) {
            blowFish.GetComponent<Rigidbody2D>().gravityScale = pumpPower * -1;
        } else {
            blowFish.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }
        //blowFish.AddForce(pumpDirection * pumpPower);

    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
