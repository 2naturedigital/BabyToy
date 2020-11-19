using UnityEngine;

public class Guppy : FishController
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

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Guppy Bumpy!");
        if (other.tag == "Fish") {
            SetRandomTarget();
        }
    }
}
