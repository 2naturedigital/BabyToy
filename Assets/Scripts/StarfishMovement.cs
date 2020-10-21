using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishMovement : MonoBehaviour
{
    public float starfish_velocity = 0;
    public int starfish_speed = 1000;
    public int direction = 0;
    public float elapsed_time = 0;
    public int movement_period = 0;
    // Start is called before the first frame update
    void Start()
    {
        movement_period = Random.Range(1, 5);
        direction = Random.Range(1, 5);
        starfish_speed = Random.Range(1000, 3000);
        starfish_velocity = starfish_speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        elapsed_time += Time.deltaTime;


        if (elapsed_time > movement_period)
        {
            switch (direction)
            {
                case 1:
                    Debug.Log("Starfish moving UP");
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * starfish_velocity * 3;
                    break;
                case 2:
                    Debug.Log("Starfish moving RIGHT");
                    GetComponent<Rigidbody2D>().velocity = Vector2.right * starfish_velocity * 2;
                    break;
                case 3:
                    Debug.Log("Starfish moving DOWN");
                    GetComponent<Rigidbody2D>().velocity = Vector2.down * starfish_velocity;
                    break;
                case 4:
                    Debug.Log("Starfish moving LEFT");
                    GetComponent<Rigidbody2D>().velocity = Vector2.left * starfish_velocity * 5;
                    break;
                default:
                    Debug.Log("Default case reached for starfish velocity");
                    break;
            }

            // recauculate values for next movement
            elapsed_time = 0;
            Debug.Log("time passed: " + movement_period + " seconds");
            movement_period = Random.Range(1, 5);
            direction = Random.Range(1, 5);
            starfish_speed = Random.Range(2000, 4000);
            starfish_velocity = starfish_speed * Time.deltaTime;
        }
    }
}
