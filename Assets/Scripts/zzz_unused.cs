using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomPatrol : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float speed;

    // Keep track of current target position
    private Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get a starting target position
        targetPosition = GetRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        // Do a movement transformation if the target position and the current position don't match
        if ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            targetPosition = GetRandomPosition();
        }

    }

    Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Reload screen when there is a collision
        if (other.tag == "Fish")
        {
            targetPosition = GetRandomPosition();
            //Reload();
        }

    }

    public void Reload()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetTarget(Vector2 newTarget)
    {
        targetPosition = newTarget;
    }

    public Vector2 GetTarget()
    {
        return targetPosition;
    }
}
