using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubbles2 : MonoBehaviour
{
    public GameObject bubbleOriginal;
    public GameObject bubbleContainer;
    public Animator animator;
    Collider2D col;
    readonly float bubbleSpawnMinTime = 1;
    readonly float bubbleSpawnMaxTime = 8;
    readonly float bubblePopMinTime = 5;
    readonly float bubblePopMaxTime = 20;
    float bubbleTimer = 0;
    float bubbleDestoryTimer = 0;


    // Start is called before the first frame update
    void Start()
    {        
        col = GetComponent<Collider2D>();
    }

    public void CreateBubbles() {          
        if (bubbleTimer <= 0) {
            Vector3 bubblePosition = new Vector3(Random.Range(-540.0f, 540.0f), -1000.0f, 0f);
            GameObject bubbleClone = Instantiate(bubbleOriginal, bubblePosition, bubbleOriginal.transform.rotation);
            bubbleClone.transform.parent = bubbleContainer.transform;

            //create a self-destruct timer for the bubble just created and set it
            bubbleDestoryTimer = Random.Range(bubblePopMinTime, bubblePopMaxTime);
            Destroy(bubbleClone, bubbleDestoryTimer);

            bubbleTimer = Random.Range(bubbleSpawnMinTime, bubbleSpawnMaxTime);
        } else {
            bubbleTimer -= Time.deltaTime;
        }        
    }

    
    // Update is called once per frame
    void Update()
    {

        CreateBubbles(); //need the number to be dynamic, reactive to the shake. 

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            // When a touch begins, grab its location and see if it is overlaping a collider2d object
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(touchPosition);
                if (col == touchedCollider)
                {
                    animator.SetTrigger("Touched");
                }
            }            
        }
    }

    

}
