using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesDup : MonoBehaviour
{
    public GameObject bubbleOriginal;
    public GameObject bubbleContainer;
    public float bubbleSpawnMinTime;
    public float bubbleSpawnMaxTime;
    public float bubblePopMinTime;
    public float bubblePopMaxTime;
    public float bubbleMinScale;
    public float bubbleMaxScale;
    float bubbleTimer = 0;

    // Update is called once per frame
    void Update() {
        bubbleTimer -= Time.deltaTime;
        CreateBubbles(); //need the number to be dynamic, reactive to the shake.
    }


    public void CreateBubbles() {
        if (bubbleTimer <= 0) {
            Vector3 bubblePosition = new Vector3(Random.Range(-540.0f, 540.0f), -1000.0f, 0f);
            GameObject bubbleClone = Instantiate(bubbleOriginal, bubblePosition, bubbleOriginal.transform.rotation);
            bubbleClone.transform.SetParent(bubbleContainer.transform);
            bubbleClone.transform.localScale *= Random.Range(bubbleMinScale, bubbleMaxScale);

            //create a self-destruct timer for the bubble just created and set it
            //bubbleDestoryTimer = Random.Range(bubblePopMinTime, bubblePopMaxTime);
            //Destroy(bubbleClone, bubbleDestoryTimer);

            bubbleTimer = Random.Range(bubbleSpawnMinTime, bubbleSpawnMaxTime);
        }
    }
}
