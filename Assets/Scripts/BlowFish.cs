using UnityEngine;

public class BlowFish : FishController
{
    //public Animator animator;

    // Start is called before the first frame update
    void Start() {
        //animator = GetComponent<Animator>();
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
    }

    public override void MoveFish()
    {
        //base.MoveFish();
    }

    private void OnTriggerEnter2D(Collider2D other) {

    }
}
