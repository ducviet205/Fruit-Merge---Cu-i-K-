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
    private bool winScheduled;
    private bool hasWon;

    void Start()
    {
        maxScore = PlayerPrefs.GetInt("ReachScore", 50);
        PlayerPrefs.SetInt("ReachScore", maxScore);
        maxScoreText.text = "Target " + maxScore.ToString();

        Debug.Log("score" + maxScore);
    }

    void Update()
    {
        scoreText.text = score.ToString();
        AchievementManager.Instance.RecordScore(score);

        if (!winScheduled && !hasWon && score >= maxScore)
        {
            winScheduled = true;
            Invoke("Win", 1);
        }
    }

    public void Win()
    {
        if (hasWon)
        {
            return;
        }

        hasWon = true;
        winScheduled = false;
        StoreData.AddCoins(StoreData.GetWinReward(score));
        AchievementManager.Instance.RecordWin();
        Time.timeScale = 0;
        winPanel.SetActive(true);
    }

    public void Score()
    {
        score = 2;
    }
}
