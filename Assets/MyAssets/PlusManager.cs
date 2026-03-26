using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlusManager : MonoBehaviour
{
    public Transform plusTransform;
    public GameObject plusBall4, plusBall8, plusBall16, plusBall32, plusBall64, plusBall128, plusBall256, plusBall512;
    public bool plusBallSpawn2, plusBallSpawn4, plusBallSpawn8, plusBallSpawn16, plusBallSpawn32, plusBallSpawn64, plusBallSpawn128, plusBallSpawn256, plusBallSpawn512;
    public GameManager gameManagerS;
    void Start()
    {
       
    }
    
    void Update()
    {
        Plus();
    }

    public void Plus()
    {
        if (plusBallSpawn2)
        {
            gameManagerS.score += 1;
            AchievementManager.Instance.RecordMerge(4, gameManagerS.score);

            plusBall4.GetComponent<Rigidbody2D>().simulated = true;
            plusBall4.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall4, plusTransform.position, plusTransform.rotation);
            plusBallSpawn2 = false;
        }

        if (plusBallSpawn4)
        {
            gameManagerS.score += 2;
            AchievementManager.Instance.RecordMerge(8, gameManagerS.score);

            plusBall8.GetComponent<Rigidbody2D>().simulated = true;
            plusBall8.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall8, plusTransform.position, plusTransform.rotation);
            plusBallSpawn4 = false;
        }

        if (plusBallSpawn8)
        {
            gameManagerS.score += 3;
            AchievementManager.Instance.RecordMerge(16, gameManagerS.score);

            plusBall16.GetComponent<Rigidbody2D>().simulated = true;
            plusBall16.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall16, plusTransform.position, plusTransform.rotation);
            plusBallSpawn8 = false;
        }

        if (plusBallSpawn16)
        {
            gameManagerS.score += 4;
            AchievementManager.Instance.RecordMerge(32, gameManagerS.score);

            plusBall32.GetComponent<Rigidbody2D>().simulated = true;
            plusBall32.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall32, plusTransform.position, plusTransform.rotation);
            plusBallSpawn16 = false;
        }

        if (plusBallSpawn32)
        {
            gameManagerS.score += 5;
            AchievementManager.Instance.RecordMerge(64, gameManagerS.score);

            plusBall64.GetComponent<Rigidbody2D>().simulated = true;
            plusBall64.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall64, plusTransform.position, plusTransform.rotation);
            plusBallSpawn32 = false;
        }

        if (plusBallSpawn64)
        {
            gameManagerS.score += 6;
            AchievementManager.Instance.RecordMerge(128, gameManagerS.score);

            plusBall128.GetComponent<Rigidbody2D>().simulated = true;
            plusBall128.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall128, plusTransform.position, plusTransform.rotation);
            plusBallSpawn64 = false;
        }

        if (plusBallSpawn128)
        {
            gameManagerS.score += 7;
            AchievementManager.Instance.RecordMerge(256, gameManagerS.score);

            plusBall256.GetComponent<Rigidbody2D>().simulated = true;
            plusBall256.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall256, plusTransform.position, plusTransform.rotation);
            plusBallSpawn128 = false;
        }

        if (plusBallSpawn256)
        {
            gameManagerS.score += 8;
            AchievementManager.Instance.RecordMerge(512, gameManagerS.score);

            plusBall512.GetComponent<Rigidbody2D>().simulated = true;
            plusBall512.transform.Find("Smoke").gameObject.SetActive(true);
            Instantiate(plusBall512, plusTransform.position, plusTransform.rotation);
            plusBallSpawn256 = false;
        }

        //if (plusBallSpawn512)
        //{
        //    plusBall.GetComponent<Rigidbody2D>().simulated = true;
        //    Instantiate(plusBall256, plusTransform.position, plusTransform.rotation);
        //    plusBallSpawn512 = false;
        //}
    }

    public void BallBool(string ballId)
    {
        if(ballId == "2")
        {
            plusBallSpawn2 = true;
        }

        if (ballId == "4")
        {
            plusBallSpawn4 = true;
        }

        if (ballId == "8")
        {
            plusBallSpawn8 = true;
        }

        if (ballId == "16")
        {
            plusBallSpawn16 = true;
        }

        if (ballId == "32")
        {
            plusBallSpawn32 = true;
        }

        if (ballId == "64")
        {
            plusBallSpawn64 = true;
        }

        if (ballId == "128")
        {
            plusBallSpawn128 = true;
        }

        if (ballId == "256")
        {
            plusBallSpawn256 = true;
        }

        if (ballId == "512")
        {
            plusBallSpawn512 = true;
        }
    }
}
