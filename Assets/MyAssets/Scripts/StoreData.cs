using UnityEngine;

public static class StoreData
{
    private const string CoinsKey = "StoreCoins";
    private const string SelectedSkinKey = "StoreSelectedSkin";
    private const string OwnedPrefix = "StoreOwned_";
    private const int DefaultCoins = 120;

    public const string DefaultSkinId = "blue";

    private static readonly string[] SkinIds =
    {
        "blue",
        "green",
        "orange",
        "purple",
        "red",
        "yellow"
    };

    public static int GetCoins()
    {
        return PlayerPrefs.GetInt(CoinsKey, DefaultCoins);
    }

    public static void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        PlayerPrefs.SetInt(CoinsKey, GetCoins() + amount);
        PlayerPrefs.Save();
    }

    public static int GetWinReward(int score)
    {
        return Mathf.Clamp(20 + Mathf.RoundToInt(score * 0.25f), 20, 80);
    }

    public static bool IsOwned(string skinId)
    {
        if (string.IsNullOrEmpty(skinId))
        {
            return false;
        }

        if (skinId == DefaultSkinId)
        {
            return true;
        }

        return PlayerPrefs.GetInt(OwnedPrefix + skinId, 0) == 1;
    }

    public static bool Purchase(string skinId, int price)
    {
        if (string.IsNullOrEmpty(skinId))
        {
            return false;
        }

        if (IsOwned(skinId))
        {
            return true;
        }

        int coins = GetCoins();
        if (coins < price)
        {
            return false;
        }

        PlayerPrefs.SetInt(CoinsKey, coins - price);
        PlayerPrefs.SetInt(OwnedPrefix + skinId, 1);
        PlayerPrefs.Save();
        return true;
    }

    public static string GetSelectedSkinId()
    {
        string skinId = PlayerPrefs.GetString(SelectedSkinKey, DefaultSkinId);
        if (!IsKnownSkin(skinId))
        {
            skinId = DefaultSkinId;
        }

        if (!IsOwned(skinId))
        {
            skinId = DefaultSkinId;
        }

        return skinId;
    }

    public static void SelectSkin(string skinId)
    {
        if (!IsOwned(skinId))
        {
            return;
        }

        PlayerPrefs.SetString(SelectedSkinKey, skinId);
        PlayerPrefs.Save();
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteKey(CoinsKey);
        PlayerPrefs.DeleteKey(SelectedSkinKey);

        for (int i = 0; i < SkinIds.Length; i++)
        {
            PlayerPrefs.DeleteKey(OwnedPrefix + SkinIds[i]);
        }

        PlayerPrefs.Save();
    }

    private static bool IsKnownSkin(string skinId)
    {
        for (int i = 0; i < SkinIds.Length; i++)
        {
            if (SkinIds[i] == skinId)
            {
                return true;
            }
        }

        return false;
    }
}
