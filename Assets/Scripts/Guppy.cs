using UnityEngine;

public class Guppy : FishController
{
    void Start() {
        InitializeFish();
        SetRandomTarget();
    }

    void Update() {
        MoveFish();
    }

    private void FixedUpdate() {
        SetAnimatorShakeTrigger();
        AnimateFish();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Guppy Bumpy!");
        if (other.tag == "Fish") {
            SetRandomTarget();
        }
    }
}//end of Guppy
