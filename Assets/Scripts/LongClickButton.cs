using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;
    [SerializeField]
    private float requiredHoldTime = 3;
    public UnityEvent onLongClick;

    [SerializeField]
    private Image fillImage = null;

    public void OnPointerDown(PointerEventData eventData) {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Reset();
    }
    // Start is called before the first frame update
    void Update()
    {
        if (pointerDown) {
            pointerDownTimer += Time.deltaTime;

            // If the pointer is held on the object long enough Invoke the correct function call
            if (pointerDownTimer >= requiredHoldTime) {
                // Ensure the event is assigned
                if (onLongClick != null) {
                    onLongClick.Invoke();
                }
                Reset();
            }
            // Fill the image over the button based on percent of time held if it is set
            if (fillImage != null) {
                fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
            }
        }
    }

    private void Reset() {
        pointerDown = false;
        pointerDownTimer = 0;
        if (fillImage != null) {
            fillImage.fillAmount = 0;
        }
    }
}//end of LongClickButton
