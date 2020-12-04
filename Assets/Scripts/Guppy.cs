using UnityEngine;

public class Guppy : FishController
{
    private const int FISHWIDTH = 318;
    private const int FISHHEIGHT = 342;

    void Start() {
        SetSoundController(FindObjectOfType<SoundController>());
        SetAnimator(GetComponent<Animator>());
        SetCameraProperties();
        SetFishSize(FISHWIDTH, FISHHEIGHT);
        SetFishStartingPoints();
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
