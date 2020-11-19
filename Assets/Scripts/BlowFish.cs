using UnityEngine;

public class BlowFish : FishController
{
    private Animator animator;
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        //shake reaction
        animator.SetBool("isShaking", IsShaking());
        
        //blowfish pump
        if (pumpTimer <= 0) {
            animator.SetTrigger("pumpOnce");
            pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
        } else {
            pumpTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (this.tag == "Fish" && other.tag == "Fish") {
            Debug.Log("Blowfish hit something");
            FishController otherFish = other.gameObject.GetComponent<FishController>();
            Vector2 thisTarget = this.GetTarget();
            Vector2 otherTarget = otherFish.GetTarget();
            this.SetTarget(otherTarget);
            otherFish.SetTarget(thisTarget);
        }
    }
}
