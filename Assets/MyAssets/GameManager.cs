using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int score;
    public static int maxScore = 30;
    public Text scoreText, maxScoreText;
    public GameObject winPanel;
    public CanvasUiManager canvasUiManagerS;

    void Start()
    {
        int currentLevelNo;

        currentLevelNo = PlayerPrefs.GetInt("LevelNo", 1);

      
            maxScore = PlayerPrefs.GetInt("ReachScore", 50);
            PlayerPrefs.SetInt("ReachScore", maxScore);
            maxScoreText.text = "Target " + maxScore.ToString();

            Debug.Log("score"+ maxScore);
        
    }

    void Update()
    {
        scoreText.text = score.ToString();

        if (score >= maxScore)
        {
            Invoke("Win", 1);
        }
    }

    public void Win()
    {
        Time.timeScale = 0;
        winPanel.SetActive(true);
    }

    public void Score()
    {
        score = 2;
    }
}
