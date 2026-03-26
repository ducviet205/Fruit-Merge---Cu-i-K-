using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //public int currentLevelNo = 1;

    public void Play()
    {
        //currentLevelNo = PlayerPrefs.GetInt("LevelNo", 1);

        if (Application.CanStreamedLevelBeLoaded("Game"))
        {
            SceneManager.LoadScene("Game");
            return;
        }

        SceneManager.LoadScene(1);
    }

    public void MoreGames()
    {
        Application.OpenURL("");
    }

    public void RateUs()
    {
        Application.OpenURL("");
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteAll();
            AchievementManager.Instance.ReloadStateFromPlayerPrefs();

            MenuStoreUI storeUi = GetComponent<MenuStoreUI>();
            if (storeUi != null)
            {
                storeUi.RefreshUi();
            }
        }
    }
}
