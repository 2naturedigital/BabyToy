﻿using UnityEngine;

public class Guppy : FishController
{
    public Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    void Awake() {
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool("isShaking", isShaking);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (this.tag == "Fish" && other.tag == "Fish") {
            Debug.Log("Guppy hit something");
            FishController otherFish = other.gameObject.GetComponent<FishController>();
            Vector2 thisTarget = this.GetTarget();
            Vector2 otherTarget = otherFish.GetTarget();
            this.SetTarget(otherTarget);
            otherFish.SetTarget(thisTarget);
        }
    }
}