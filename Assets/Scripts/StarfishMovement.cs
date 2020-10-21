using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishMovement : MonoBehaviour
{
    private float starfishVelocity = 0;
    public int starfishSpeed = 1000;
    private int direction = 0;
    private float elapsedTime = 0;
    public int movementPeriod = 3;
    // Start is called before the first frame update

    //TODO: factor out parts that will happen all at the same time into their own methods to be called
    //TODO: figure out how to make the starfish bounce off the borders
    void Start()
    {
        //movementPeriod = Random.Range(1, 5);
        direction = Random.Range(1, 5);
        //starfishSpeed = Random.Range(1000, 3000);
        starfishVelocity = starfishSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        //TODO: find a way to smooth out movement between directions (currently has abrupt changes)
        if (elapsedTime > movementPeriod)
        {
            switch (direction)
            {
                case 1:
                    Debug.Log("Starfish moving UP");
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * starfishVelocity;
                    break;
                case 2:
                    Debug.Log("Starfish moving RIGHT");
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * starfishVelocity;
                    break;
                case 3:
                    Debug.Log("Starfish moving DOWN");
                    GetComponent<Rigidbody2D>().velocity = Vector2.down * starfishVelocity;
                    break;
                case 4:
                    Debug.Log("Starfish moving LEFT");
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * starfishVelocity;
                    break;
                default:
                    Debug.Log("Default case reached for starfish velocity");
                    break;
            }

            // recauculate values for next movement
            elapsedTime = 0;
            Debug.Log("time passed: " + movementPeriod + " seconds");
            //movementPeriod = Random.Range(1, 5);
            direction = Random.Range(1, 5);
            //starfishSpeed = Random.Range(2000, 4000);
            starfishVelocity = starfishSpeed * Time.deltaTime;
        }
    }
}
