using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallSpawner : MonoBehaviour
{
    public GameObject[] Ball;
    public GameObject currentBall;

    public bool ballOnSpawner, ballSpawn;
    public float timeToSpawn;
    
    void Update()
    {
        if (ballSpawn == false)
        {
            if (ballOnSpawner == false)
            {
                currentBall = Instantiate(Ball[UnityEngine.Random.Range(0, Ball.Length)], transform.position, transform.rotation);
                currentBall.transform.Find("Smoke").gameObject.SetActive(false);
                currentBall.GetComponent<Rigidbody2D>().simulated = false;
                currentBall.transform.SetParent(this.transform);
                ballOnSpawner = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ballSpawn = true;
                currentBall.GetComponent<Rigidbody2D>().simulated = true;
                ballOnSpawner = false;
                currentBall.transform.parent = null;
                Invoke("SpawnBall", timeToSpawn);
            }
        }
    }

    public void SpawnBall()
    {
        ballSpawn = false;
    }
}
