using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowFish : FishController
{
    Collider2D col;
    private FishController thisFish;
    public Animator animator;

    // Start is called before the first frame update
    void Start() {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    void Awake() {
        thisFish = GameObject.FindObjectOfType<FishController>();
    }

    // Update is called once per frame
    void Update() {
        if (isShaking) {
            animator.SetBool("isShaking", true);
        } 
        if (isShaking == false) {
            animator.SetBool("isShaking", false);
        }        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (this.tag == "Fish" && other.tag == "Fish") {
            Debug.Log("Collision of Fish!");
            FishController otherFish = other.gameObject.GetComponent<FishController>();
            Vector2 thisTarget = thisFish.GetTarget();
            Vector2 otherTarget = otherFish.GetTarget();
            thisFish.SetTarget(otherTarget);
            otherFish.SetTarget(thisTarget);
        }
    }

    
}
