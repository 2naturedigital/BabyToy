using UnityEngine;

public class Guppy : FishController
{
    void Start() {
        InitializeFish();
        SetRandomTarget();
        Debug.Log("GUPPY SAYS:");
        Debug.Log("Screen dot W: " + Screen.width + " Screen dot H: " + Screen.height);
        Debug.Log("Screen W: " + GetScreenWidth() + " Screen H: " + GetScreenHeight());
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
