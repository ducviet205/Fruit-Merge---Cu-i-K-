using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasUiManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public int currentLevelNo = 1;
    public Text thisLevelNo, nextLevelNo;

    private void Awake()
    {
        Time.timeScale = 1;

        thisLevelNo.text = "Level " + PlayerPrefs.GetInt("LevelNo", 1).ToString();
        //nextLevelNo.text = PlayerPrefs.GetInt("LevelNo" + 1, 2).ToString();

        currentLevelNo = PlayerPrefs.GetInt("LevelNo", 1);
        Debug.Log(currentLevelNo);
    }

    public static int maxScore;

    public void Next()
    {
        Time.timeScale = 1;

        PlayerPrefs.SetInt("LevelNo", currentLevelNo + 1);
        
        maxScore = PlayerPrefs.GetInt("ReachScore", 30);
        PlayerPrefs.SetInt("ReachScore", maxScore + 20);

        Debug.Log(currentLevelNo);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RetryGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
