using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public string thisBallNo;
    public GameObject plusManager, ballSpawner, collisionObject;
    public float ballDestroyTime;

    void Start()
    {
        plusManager = GameObject.Find("PlusManager");
        ballSpawner = GameObject.Find("Balls Spawner");
    }

    //private void Awake()
    //{
    //    this.GetComponent<Rigidbody2D>().simulated = false;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ballSpawner.GetComponent<BallSpawner>().ballSpawn = false;

        if(collision.gameObject.tag == thisBallNo)
        {
            plusManager.GetComponent<PlusManager>().plusTransform = this.transform;
            plusManager.GetComponent<PlusManager>().BallBool(thisBallNo);
            collisionObject = collision.gameObject;
            Invoke("DestroyBall", ballDestroyTime);
        }
    }

    public void DestroyBall()
    {
        Destroy(collisionObject);
        Destroy(this.gameObject);
    }
}
