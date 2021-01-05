using UnityEngine;

public class Bubble : MonoBehaviour
{
    public Animator animator;
    public AudioClip bubbleSound;
    public AudioClip[] popSounds;
    [SerializeField]
    private float bubblePitchMin;
    [SerializeField]
    private float bubblePitchMax;
    [SerializeField]
    private float bubbleVolMin;
    [SerializeField]
    private float bubbleVolMax;
    [SerializeField]
    private float bubbleLifetimeMin;
    [SerializeField]
    private float bubbleLifetimeMax;
    [SerializeField]
    private float lifetimer;
    [SerializeField]
    private float destroyAnimationTimer;

    private GameObject bubble;
    private SoundController sndCtrl;
    private Vector3 CameraPos;
    private float defaultWidth;
    private float defaultHeight;
    private Collider2D bubbleCollider;
    private bool isPopped = false;
    private SpriteRenderer spriteRenderer;



    // Bubbles are made and destroyed on the fly, so we are using Awake() instead of Start()
    void Awake() {
        sndCtrl = FindObjectOfType<SoundController>();
        bubbleCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        lifetimer = Random.Range(bubbleLifetimeMin, bubbleLifetimeMax);
        RandomizeBubbleSounds();
        SetCameraProperties();
        // Must set tag so only bubbles get affected by the small water currents
        this.tag = "Bubble";
    }

    void FixedUpdate() {
        HandleTouch();
    }

    void Update() {
        // Autodestruct Bubbles
        // Destroy bubbles above the screen
        if (this.transform.position.y >= defaultHeight + (spriteRenderer.bounds.size.y/2)) {
            Destroy(this.gameObject);
        }
        // Pop bubbles based on timer
        if (lifetimer > 0) {
            lifetimer -= Time.deltaTime;
        }
        if (lifetimer <= 0) {
            PopBubble(this.gameObject);
            lifetimer = Random.Range(bubbleLifetimeMin, bubbleLifetimeMax);
        }

    }

    public void HandleTouch() {
        // Get touch position when the screen is touched
        if (Input.touchCount > 0) {
            // Handle all touches
            foreach (Touch touch in Input.touches) {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                // When a touch begins, grab its location and see if it is overlaping a collider2d object
                if (bubbleCollider == Physics2D.OverlapPoint(touchPosition)) {
                    // Set the collider2d object to a gameobject & destroy it
                    if (!isPopped) {
                        bubble = bubbleCollider.gameObject;
                        PopBubble(bubble);
                    }
                }
            }
        }
    }

    public void SetCameraProperties() {
        CameraPos = Camera.main.transform.position;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        defaultHeight = Camera.main.orthographicSize;
    }

    // Triggers the pop animation, sets a random pop sound & plays, destroys bubble object
    private void PopBubble(GameObject gameObject) {
        animator.SetTrigger("Touched");
        sndCtrl.PlaySFX(popSounds[Random.Range(0, popSounds.Length)]);
        Destroy(gameObject, destroyAnimationTimer);
        isPopped = true;
    }

    // Sets the bubble sound, randomizes pitch & volume
    private void RandomizeBubbleSounds() {
        sndCtrl.PlaySFX(bubbleSound, Random.Range(bubbleVolMin, bubbleVolMax), Random.Range(bubblePitchMin, bubblePitchMax));
    }
}//end of Bubble
