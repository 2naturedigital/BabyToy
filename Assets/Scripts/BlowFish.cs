using UnityEngine;

public class BlowFish : FishController
{
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;

    // Start is called before the first frame update
    void Start() {
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        AnimateFish();
        MoveFish();
    }

    public override void AnimateFish()
    {
        base.AnimateFish();
        //blowfish pump
        if (pumpTimer <= 0) {
            animator.SetTrigger("pumpOnce");
            pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
        } else {
            pumpTimer -= Time.deltaTime;
        }
    }

    public override void MoveFish()
    {
        //base.MoveFish();
    }
    private void OnTriggerEnter2D(Collider2D other) {

    }
}
