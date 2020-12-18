using UnityEngine;

public class BubblesDup : MonoBehaviour
{
    public GameObject bubbleOriginal;
    public GameObject bubbleContainer;
    public float bubbleSpawnMinTime;
    public float bubbleSpawnMaxTime;
    public float bubbleMinScale;
    public float bubbleMaxScale;
    private bool isShaking = false;
    private float bubbleTimer = 0;
    public float shakeBubbleTimer;
    public int shakeBubbleCount;
    private Vector3 CameraPos;
    private float defaultWidth;
    private float defaultHeight;
    public AudioClip shake1;
    public AudioClip shake2;
    private SoundController sndCtrl;
    private float magnitudeMult = 1;
    private float shakeForceMultiplier = 1;
    private SpriteRenderer spriteRenderer;
    private CameraScreenScale cameraScreenScale;
    private float spriteAdjustmentRatio;
    private float userSpriteSize;


    void Start() {
        sndCtrl = FindObjectOfType<SoundController>();
        SetCameraProperties();
    }

    void OnEnable() {
        // Grab user options
        float bubblefrequency = PlayerPrefs.GetFloat("bubblefrequency");
        bubbleSpawnMinTime = bubblefrequency - 2;
        bubbleSpawnMaxTime = bubblefrequency + 2;
        // Min spawn time adjustmentsust
        if (bubbleSpawnMinTime == 0) {
            bubbleSpawnMinTime = 0.75f;
        } else if (bubbleSpawnMinTime == -1) {
            bubbleSpawnMinTime = 0.5f;
        }
        shakeBubbleTimer = PlayerPrefs.GetFloat("shakenbubblefrequency");
        shakeBubbleCount = (int)PlayerPrefs.GetFloat("bubblecount");
        float bubblevariation = PlayerPrefs.GetFloat("bubblesizevariation");
        bubbleMinScale =  1 - bubblevariation;
        bubbleMaxScale = 1 + bubblevariation;
        userSpriteSize = PlayerPrefs.GetFloat("bubblesize");
    }

    void Update() {
        SetCameraProperties();
        bubbleTimer -= Time.deltaTime;
        CreateBubbles();
    }

    public void SetCameraProperties() {
        CameraPos = Camera.main.transform.position;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
        defaultHeight = Camera.main.orthographicSize;
        cameraScreenScale = FindObjectOfType<CameraScreenScale>();
        spriteAdjustmentRatio = cameraScreenScale.GetSpriteAdjustmentRatio();
    }

    public void StartShake(Vector3 mult, float shakeForceMult) {
        isShaking = true;
        bubbleTimer = shakeBubbleTimer;
        sndCtrl.PlaySFX(shake1, 1f, 1f, true);
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void ContinueShake(Vector3 mult, float shakeForceMult) {
        // Do anything needed on a continued shake
        bubbleTimer = shakeBubbleTimer;
        sndCtrl.PlaySFX(shake2, 1f, 1f, true);
        magnitudeMult = mult.sqrMagnitude;
        shakeForceMultiplier = shakeForceMult;
    }

    public void EndShake() {
        isShaking = false;
        magnitudeMult = 1;
        shakeForceMultiplier = 1;
    }

    public void CreateBubbles() {
        if (isShaking) {
            if (bubbleTimer <= 0) {
                MakeBubble(shakeBubbleCount);
                bubbleTimer = shakeBubbleTimer;
            }
        } else if (bubbleTimer <= 0) {
            MakeBubble(1);
            bubbleTimer = Random.Range(bubbleSpawnMinTime, bubbleSpawnMaxTime);
        }
    }

    public void MakeBubble(int count) {
        for (int i = 0; i < count; i++) {
            // Instantiate first
            GameObject bubbleClone = Instantiate(bubbleOriginal, new Vector3(0, 0, 0), bubbleOriginal.transform.rotation);
            spriteRenderer = bubbleClone.GetComponent<SpriteRenderer>();
            // Move to new position after finding size of bubble
            Vector3 bubblePosition = new Vector3(Random.Range(CameraPos.x - defaultWidth, defaultWidth), CameraPos.y - defaultHeight - (spriteRenderer.bounds.size.y/4), 0f);
            bubbleClone.transform.position = bubblePosition;
            bubbleClone.transform.SetParent(bubbleContainer.transform);
            bubbleClone.transform.localScale *= Random.Range(bubbleMinScale, bubbleMaxScale) * spriteAdjustmentRatio * userSpriteSize;
        }
    }
}//end of BubblesDup
