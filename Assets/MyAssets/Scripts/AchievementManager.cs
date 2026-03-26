using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
    private const string UnlockedPrefix = "AchievementUnlocked_";
    private const string TotalWinsKey = "AchievementStat_TotalWins";
    private const string TotalMergesKey = "AchievementStat_TotalMerges";
    private const string HighestCreatedValueKey = "AchievementStat_HighestCreatedValue";
    private const string CollectorMaskKey = "AchievementStat_CollectorMask";
    private const string BestScoreKey = "AchievementStat_BestScore";
    private const int CollectorCompleteMask = 255;

    private static AchievementManager instance;

    private readonly List<AchievementDefinition> definitions = new List<AchievementDefinition>();
    private readonly Dictionary<string, AchievementDefinition> definitionLookup = new Dictionary<string, AchievementDefinition>();
    private readonly HashSet<string> unlockedIds = new HashSet<string>();
    private readonly List<AchievementDefinition> sessionUnlocks = new List<AchievementDefinition>();

    private int totalWins;
    private int totalMerges;
    private int highestCreatedValue;
    private int collectorMask;
    private int bestScore;
    private AchievementSceneUI currentSceneUi;

    public static AchievementManager Instance
    {
        get
        {
            if (instance == null)
            {
                CreateInstance();
            }

            return instance;
        }
    }

    public IReadOnlyList<AchievementDefinition> Definitions => definitions;

    public event Action<AchievementDefinition> AchievementUnlocked;
    public event Action StateChanged;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoCreate()
    {
        if (instance == null)
        {
            CreateInstance();
        }
    }

    private static void CreateInstance()
    {
        GameObject managerObject = new GameObject("AchievementManager");
        instance = managerObject.AddComponent<AchievementManager>();
        DontDestroyOnLoad(managerObject);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        BuildDefinitions();
        LoadStateFromPlayerPrefs();
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void Start()
    {
        HandleSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }
    }

    public void RecordMerge(int createdValue, int currentScore)
    {
        bool hasChanges = false;

        totalMerges++;
        PlayerPrefs.SetInt(TotalMergesKey, totalMerges);
        hasChanges = true;

        if (createdValue > highestCreatedValue)
        {
            highestCreatedValue = createdValue;
            PlayerPrefs.SetInt(HighestCreatedValueKey, highestCreatedValue);
        }

        int collectorBit = GetCollectorBit(createdValue);
        if (collectorBit != 0 && (collectorMask & collectorBit) == 0)
        {
            collectorMask |= collectorBit;
            PlayerPrefs.SetInt(CollectorMaskKey, collectorMask);
        }

        if (TryUnlock("first_merge"))
        {
            hasChanges = true;
        }

        if (createdValue == 16 && TryUnlock("merge_16"))
        {
            hasChanges = true;
        }

        if (createdValue == 64 && TryUnlock("merge_64"))
        {
            hasChanges = true;
        }

        if (createdValue == 128 && TryUnlock("merge_128"))
        {
            hasChanges = true;
        }

        if (createdValue == 512 && TryUnlock("merge_512"))
        {
            hasChanges = true;
        }

        if (collectorMask == CollectorCompleteMask && TryUnlock("collector"))
        {
            hasChanges = true;
        }

        if (UpdateBestScore(currentScore))
        {
            hasChanges = true;
        }

        if (hasChanges)
        {
            PlayerPrefs.Save();
            NotifyStateChanged();
        }
    }

    public void RecordScore(int currentScore)
    {
        if (!UpdateBestScore(currentScore))
        {
            return;
        }

        PlayerPrefs.Save();
        NotifyStateChanged();
    }

    public void RecordWin()
    {
        bool hasChanges = false;

        totalWins++;
        PlayerPrefs.SetInt(TotalWinsKey, totalWins);
        hasChanges = true;

        if (TryUnlock("first_win"))
        {
            hasChanges = true;
        }

        if (totalWins >= 5 && TryUnlock("win_5"))
        {
            hasChanges = true;
        }

        if (hasChanges)
        {
            PlayerPrefs.Save();
            NotifyStateChanged();
        }
    }

    public void ReloadStateFromPlayerPrefs()
    {
        LoadStateFromPlayerPrefs();
        NotifyStateChanged();
    }

    public void RegisterSceneUi(AchievementSceneUI sceneUi)
    {
        currentSceneUi = sceneUi;
    }

    public void UnregisterSceneUi(AchievementSceneUI sceneUi)
    {
        if (currentSceneUi == sceneUi)
        {
            currentSceneUi = null;
        }
    }

    public bool IsUnlocked(AchievementDefinition definition)
    {
        return definition != null && unlockedIds.Contains(definition.Id);
    }

    public int GetUnlockedCount()
    {
        return unlockedIds.Count;
    }

    public int GetTotalCount()
    {
        return definitions.Count;
    }

    public string GetSummaryText()
    {
        return "Achievements " + GetUnlockedCount() + "/" + GetTotalCount();
    }

    public int GetCurrentProgress(AchievementDefinition definition)
    {
        switch (definition.Id)
        {
            case "first_merge":
                return Mathf.Min(totalMerges, definition.TargetValue);
            case "merge_16":
                return HasCreatedValue(16) ? 1 : 0;
            case "merge_64":
                return HasCreatedValue(64) ? 1 : 0;
            case "merge_128":
                return HasCreatedValue(128) ? 1 : 0;
            case "merge_512":
                return HasCreatedValue(512) ? 1 : 0;
            case "score_50":
            case "score_100":
                return Mathf.Min(bestScore, definition.TargetValue);
            case "first_win":
                return Mathf.Min(totalWins, definition.TargetValue);
            case "win_5":
                return Mathf.Min(totalWins, definition.TargetValue);
            case "collector":
                return Mathf.Min(CountCollectorBits(), definition.TargetValue);
            default:
                return 0;
        }
    }

    public string GetProgressText(AchievementDefinition definition)
    {
        return GetCurrentProgress(definition) + "/" + definition.TargetValue;
    }

    public string GetSessionUnlockSummary(int maxCount)
    {
        if (sessionUnlocks.Count == 0)
        {
            return string.Empty;
        }

        int startIndex = Mathf.Max(0, sessionUnlocks.Count - maxCount);
        StringBuilder builder = new StringBuilder();
        builder.Append("New achievements");

        for (int i = startIndex; i < sessionUnlocks.Count; i++)
        {
            builder.Append('\n');
            builder.Append("- ");
            builder.Append(sessionUnlocks[i].Title);
        }

        return builder.ToString();
    }

    private void BuildDefinitions()
    {
        if (definitions.Count > 0)
        {
            return;
        }

        AddDefinition(new AchievementDefinition("first_merge", "First Merge", "Complete your first merge.", 1));
        AddDefinition(new AchievementDefinition("merge_16", "Reach 16", "Create a level 16 fruit.", 1));
        AddDefinition(new AchievementDefinition("merge_64", "Reach 64", "Create a level 64 fruit.", 1));
        AddDefinition(new AchievementDefinition("merge_128", "Reach 128", "Create a level 128 fruit.", 1));
        AddDefinition(new AchievementDefinition("merge_512", "Touch 512", "Create a level 512 fruit.", 1));
        AddDefinition(new AchievementDefinition("score_50", "50 Points", "Reach 50 points in a run.", 50));
        AddDefinition(new AchievementDefinition("score_100", "100 Points", "Reach 100 points in a run.", 100));
        AddDefinition(new AchievementDefinition("first_win", "First Win", "Win your first level.", 1));
        AddDefinition(new AchievementDefinition("win_5", "Five Wins", "Win a total of 5 levels.", 5));
        AddDefinition(new AchievementDefinition("collector", "Collector", "Create all merge values from 4 to 512.", 8));
    }

    private void AddDefinition(AchievementDefinition definition)
    {
        definitions.Add(definition);
        definitionLookup.Add(definition.Id, definition);
    }

    private void LoadStateFromPlayerPrefs()
    {
        unlockedIds.Clear();

        foreach (AchievementDefinition definition in definitions)
        {
            if (PlayerPrefs.GetInt(GetUnlockedKey(definition.Id), 0) == 1)
            {
                unlockedIds.Add(definition.Id);
            }
        }

        totalWins = PlayerPrefs.GetInt(TotalWinsKey, 0);
        totalMerges = PlayerPrefs.GetInt(TotalMergesKey, 0);
        highestCreatedValue = PlayerPrefs.GetInt(HighestCreatedValueKey, 0);
        collectorMask = PlayerPrefs.GetInt(CollectorMaskKey, 0);
        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    private bool UpdateBestScore(int currentScore)
    {
        bool hasChanges = false;

        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            hasChanges = true;
        }

        if (bestScore >= 50 && TryUnlock("score_50"))
        {
            hasChanges = true;
        }

        if (bestScore >= 100 && TryUnlock("score_100"))
        {
            hasChanges = true;
        }

        return hasChanges;
    }

    private bool TryUnlock(string achievementId)
    {
        if (unlockedIds.Contains(achievementId))
        {
            return false;
        }

        unlockedIds.Add(achievementId);
        PlayerPrefs.SetInt(GetUnlockedKey(achievementId), 1);
        AchievementDefinition definition = definitionLookup[achievementId];
        sessionUnlocks.Add(definition);
        AchievementUnlocked?.Invoke(definition);
        return true;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sessionUnlocks.Clear();

        if (string.Equals(scene.name, "Menu", StringComparison.OrdinalIgnoreCase))
        {
            DisableUiInScene(scene);
            currentSceneUi = null;
            return;
        }

        EnsureSceneUi(scene);
        NotifyStateChanged();
    }

    private void EnsureSceneUi(Scene scene)
    {
        if (currentSceneUi == null)
        {
            currentSceneUi = FindObjectOfType<AchievementSceneUI>(true);
        }

        if (currentSceneUi != null && currentSceneUi.IsInitialized)
        {
            if (!currentSceneUi.gameObject.activeSelf)
            {
                currentSceneUi.gameObject.SetActive(true);
            }
            return;
        }

        if (currentSceneUi != null && !currentSceneUi.IsInitialized)
        {
            if (!currentSceneUi.gameObject.activeSelf)
            {
                currentSceneUi.gameObject.SetActive(true);
            }
            currentSceneUi.Initialize(this, string.Equals(scene.name, "Menu", StringComparison.OrdinalIgnoreCase));
            return;
        }
    }

    private void DisableUiInScene(Scene scene)
    {
        AchievementSceneUI[] sceneUis = FindObjectsOfType<AchievementSceneUI>(true);
        foreach (AchievementSceneUI sceneUi in sceneUis)
        {
            if (sceneUi != null && sceneUi.gameObject.scene == scene)
            {
                sceneUi.gameObject.SetActive(false);
            }
        }
    }

    private int GetCollectorBit(int createdValue)
    {
        switch (createdValue)
        {
            case 4:
                return 1;
            case 8:
                return 2;
            case 16:
                return 4;
            case 32:
                return 8;
            case 64:
                return 16;
            case 128:
                return 32;
            case 256:
                return 64;
            case 512:
                return 128;
            default:
                return 0;
        }
    }

    private bool HasCreatedValue(int createdValue)
    {
        int collectorBit = GetCollectorBit(createdValue);
        return collectorBit != 0 && (collectorMask & collectorBit) != 0;
    }

    private int CountCollectorBits()
    {
        int count = 0;
        int mask = collectorMask;

        while (mask > 0)
        {
            count += mask & 1;
            mask >>= 1;
        }

        return count;
    }

    private string GetUnlockedKey(string achievementId)
    {
        return UnlockedPrefix + achievementId;
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke();
    }
}
