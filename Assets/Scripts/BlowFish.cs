using UnityEngine;

public class BlowFish : FishController
{
<<<<<<< HEAD
    private Animator animator;
    public float pumpMinTime = 0.3f;
    public float pumpMaxTime = 3f;
    float pumpTimer = 0;
=======
    //public Animator animator;
>>>>>>> 72fd83bb4d570f644b74596b89b3d8b5113c20f7

    // Start is called before the first frame update
    void Start() {
        //animator = GetComponent<Animator>();
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
<<<<<<< HEAD
        //shake reaction
        animator.SetBool("isShaking", IsShaking());
        
        //blowfish pump
        if (pumpTimer <= 0) {
            animator.SetTrigger("pumpOnce");
            pumpTimer = Random.Range(pumpMinTime, pumpMaxTime);
        } else {
            pumpTimer -= Time.deltaTime;
        }
=======
        AnimateFish();
        //MoveFish();
>>>>>>> 72fd83bb4d570f644b74596b89b3d8b5113c20f7
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
