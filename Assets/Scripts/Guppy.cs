using UnityEngine;

public class Guppy : FishController
{
    private const int FISHWIDTH = 318;
    private const int FISHHEIGHT = 342;

    void Start() {
        SetCameraProperties();
        SetSoundController(FindObjectOfType<SoundController>());
        SetAnimator(GetComponent<Animator>());
        SetFishSize(FISHWIDTH, FISHHEIGHT);
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
