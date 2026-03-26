using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AchievementSceneUI : MonoBehaviour
{
    [SerializeField] private List<Text> rowTexts = new List<Text>();
    [SerializeField] private List<Image> rowBackgrounds = new List<Image>();
    [SerializeField] private GameObject toggleButtonObject;
    [SerializeField] private GameObject panelOverlay;
    [SerializeField] private GameObject popupObject;
    [SerializeField] private Text popupText;
    [SerializeField] private Text toggleButtonText;
    [SerializeField] private Text summaryText;
    [SerializeField] private Text winPanelAchievementText;
    private readonly Queue<AchievementDefinition> popupQueue = new Queue<AchievementDefinition>();

    private AchievementManager manager;
    private Font defaultFont;
    private bool isMenuScene;
    private Coroutine popupRoutine;
    private float previousTimeScale = 1f;

    public bool IsInitialized { get; private set; }

    public void Initialize(AchievementManager achievementManager, bool menuScene)
    {
        if (IsInitialized)
        {
            return;
        }

        manager = achievementManager;
        isMenuScene = menuScene;
        defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        manager.RegisterSceneUi(this);
        manager.StateChanged += RefreshUi;
        manager.AchievementUnlocked += HandleAchievementUnlocked;

        TryBindExistingUiReferences();

        if (!HasBuiltUi())
        {
            BuildUi();
        }

        RefreshUi();
        IsInitialized = true;
    }

    public void BuildGeneratedUi(bool menuScene)
    {
        isMenuScene = menuScene;
        defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        IsInitialized = false;

        ClearGeneratedChildren();
        rowTexts.Clear();
        rowBackgrounds.Clear();
        popupQueue.Clear();

        BuildUi();

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }

    private void OnDestroy()
    {
        if (manager != null)
        {
            manager.StateChanged -= RefreshUi;
            manager.AchievementUnlocked -= HandleAchievementUnlocked;
            manager.UnregisterSceneUi(this);
        }
    }

    private void Update()
    {
        if (panelOverlay != null && panelOverlay.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            SetPanelVisible(false);
        }
    }

    private void BuildUi()
    {
        Canvas canvas = FindOrCreateCanvas();
        EnsureEventSystemExists();

        transform.SetParent(canvas.transform, false);
        transform.SetAsLastSibling();

        CreateToggleButton();
        CreateAchievementPanel();

        if (!isMenuScene)
        {
            CreatePopup();
            CreateWinPanelSummary();
        }
    }

    private bool HasBuiltUi()
    {
        return toggleButtonObject != null && panelOverlay != null && rowTexts.Count > 0;
    }

    private void ClearGeneratedChildren()
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).gameObject);
        }

        foreach (GameObject child in children)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(child);
            }
            else
#endif
            {
                Destroy(child);
            }
        }

        toggleButtonObject = null;
        toggleButtonText = null;
        summaryText = null;
        panelOverlay = null;
        popupObject = null;
        popupText = null;
        winPanelAchievementText = null;
        popupRoutine = null;
    }

    private void TryBindExistingUiReferences()
    {
        if (toggleButtonObject == null)
        {
            Transform toggleTransform = transform.Find("AchievementToggleButton");
            if (toggleTransform != null)
            {
                toggleButtonObject = toggleTransform.gameObject;
            }
        }

        if (toggleButtonText == null && toggleButtonObject != null)
        {
            Transform labelTransform = toggleButtonObject.transform.Find("Label");
            if (labelTransform != null)
            {
                toggleButtonText = labelTransform.GetComponent<Text>();
            }
        }

        if (panelOverlay == null)
        {
            Transform overlayTransform = transform.Find("AchievementOverlay");
            if (overlayTransform != null)
            {
                panelOverlay = overlayTransform.gameObject;
            }
        }

        if (summaryText == null && panelOverlay != null)
        {
            Transform summaryTransform = panelOverlay.transform.Find("AchievementCard/SummaryText");
            if (summaryTransform != null)
            {
                summaryText = summaryTransform.GetComponent<Text>();
            }
        }

        if (rowTexts.Count == 0 || rowBackgrounds.Count == 0)
        {
            rowTexts.Clear();
            rowBackgrounds.Clear();

            if (panelOverlay != null)
            {
                Transform contentRoot = panelOverlay.transform.Find("AchievementCard/ContentRoot");
                if (contentRoot != null)
                {
                    for (int i = 0; i < contentRoot.childCount; i++)
                    {
                        Transform rowTransform = contentRoot.GetChild(i);
                        Image rowImage = rowTransform.GetComponent<Image>();
                        Text rowText = rowTransform.GetComponentInChildren<Text>(true);

                        if (rowImage != null)
                        {
                            rowBackgrounds.Add(rowImage);
                        }

                        if (rowText != null)
                        {
                            rowTexts.Add(rowText);
                        }
                    }
                }
            }
        }

        if (popupObject == null)
        {
            Transform popupTransform = transform.Find("AchievementPopup");
            if (popupTransform != null)
            {
                popupObject = popupTransform.gameObject;
            }
        }

        if (popupText == null && popupObject != null)
        {
            Transform popupTextTransform = popupObject.transform.Find("PopupText");
            if (popupTextTransform != null)
            {
                popupText = popupTextTransform.GetComponent<Text>();
            }
        }

        if (winPanelAchievementText == null)
        {
            GameObject winPanelTextObject = GameObject.Find("WinPanelAchievementText");
            if (winPanelTextObject != null)
            {
                winPanelAchievementText = winPanelTextObject.GetComponent<Text>();
            }
        }
    }

    private void CreateToggleButton()
    {
        Button toggleButton = CreateButton("AchievementToggleButton", transform, isMenuScene ? "Achievements" : "Achievements 0/0");
        toggleButtonObject = toggleButton.gameObject;
        RectTransform rectTransform = toggleButton.GetComponent<RectTransform>();

        if (isMenuScene)
        {
            rectTransform.anchorMin = new Vector2(0f, 0f);
            rectTransform.anchorMax = new Vector2(0f, 0f);
            rectTransform.pivot = new Vector2(0f, 0f);
            rectTransform.anchoredPosition = new Vector2(30f, 30f);
            rectTransform.sizeDelta = new Vector2(240f, 80f);
        }
        else
        {
            rectTransform.anchorMin = new Vector2(1f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(1f, 1f);
            rectTransform.anchoredPosition = new Vector2(-30f, -30f);
            rectTransform.sizeDelta = new Vector2(260f, 70f);
        }

        toggleButtonText = toggleButton.GetComponentInChildren<Text>();
        toggleButton.onClick.AddListener(TogglePanel);
    }

    private void CreateAchievementPanel()
    {
        panelOverlay = CreateImageObject("AchievementOverlay", transform, new Color(0f, 0f, 0f, 0.78f));
        RectTransform overlayRect = panelOverlay.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.offsetMin = Vector2.zero;
        overlayRect.offsetMax = Vector2.zero;
        panelOverlay.SetActive(false);

        GameObject card = CreateImageObject("AchievementCard", panelOverlay.transform, new Color(0.21f, 0.13f, 0.07f, 0.97f));
        RectTransform cardRect = card.GetComponent<RectTransform>();
        cardRect.anchorMin = new Vector2(0.08f, 0.08f);
        cardRect.anchorMax = new Vector2(0.92f, 0.92f);
        cardRect.offsetMin = Vector2.zero;
        cardRect.offsetMax = Vector2.zero;

        Text titleText = CreateText("TitleText", card.transform, "Achievements", 34, TextAnchor.MiddleLeft);
        RectTransform titleRect = titleText.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.05f, 0.88f);
        titleRect.anchorMax = new Vector2(0.65f, 0.97f);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        summaryText = CreateText("SummaryText", card.transform, string.Empty, 22, TextAnchor.MiddleLeft);
        RectTransform summaryRect = summaryText.GetComponent<RectTransform>();
        summaryRect.anchorMin = new Vector2(0.05f, 0.82f);
        summaryRect.anchorMax = new Vector2(0.65f, 0.88f);
        summaryRect.offsetMin = Vector2.zero;
        summaryRect.offsetMax = Vector2.zero;

        Button closeButton = CreateButton("CloseButton", card.transform, "Close");
        RectTransform closeRect = closeButton.GetComponent<RectTransform>();
        closeRect.anchorMin = new Vector2(0.72f, 0.87f);
        closeRect.anchorMax = new Vector2(0.95f, 0.96f);
        closeRect.offsetMin = Vector2.zero;
        closeRect.offsetMax = Vector2.zero;
        closeButton.onClick.AddListener(() => SetPanelVisible(false));

        GameObject contentRoot = CreateUiObject("ContentRoot", card.transform);
        RectTransform contentRect = contentRoot.GetComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.05f, 0.05f);
        contentRect.anchorMax = new Vector2(0.95f, 0.79f);
        contentRect.offsetMin = Vector2.zero;
        contentRect.offsetMax = Vector2.zero;

        VerticalLayoutGroup layoutGroup = contentRoot.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 10f;
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.padding = new RectOffset(0, 0, 0, 0);

        foreach (AchievementDefinition definition in GetDefinitions())
        {
            GameObject rowObject = CreateImageObject(definition.Id + "_Row", contentRoot.transform, new Color(0.35f, 0.22f, 0.12f, 0.95f));
            LayoutElement layoutElement = rowObject.AddComponent<LayoutElement>();
            layoutElement.preferredHeight = 88f;

            Image rowImage = rowObject.GetComponent<Image>();
            rowBackgrounds.Add(rowImage);

            Text rowText = CreateText(definition.Id + "_Text", rowObject.transform, string.Empty, 21, TextAnchor.MiddleLeft);
            RectTransform rowRect = rowText.GetComponent<RectTransform>();
            rowRect.anchorMin = Vector2.zero;
            rowRect.anchorMax = Vector2.one;
            rowRect.offsetMin = new Vector2(18f, 10f);
            rowRect.offsetMax = new Vector2(-18f, -10f);

            rowTexts.Add(rowText);
        }
    }

    private List<AchievementDefinition> GetDefinitions()
    {
        if (manager != null)
        {
            return new List<AchievementDefinition>(manager.Definitions);
        }

        return new List<AchievementDefinition>
        {
            new AchievementDefinition("first_merge", "First Merge", "Complete your first merge.", 1),
            new AchievementDefinition("merge_16", "Reach 16", "Create a level 16 fruit.", 1),
            new AchievementDefinition("merge_64", "Reach 64", "Create a level 64 fruit.", 1),
            new AchievementDefinition("merge_128", "Reach 128", "Create a level 128 fruit.", 1),
            new AchievementDefinition("merge_512", "Touch 512", "Create a level 512 fruit.", 1),
            new AchievementDefinition("score_50", "50 Points", "Reach 50 points in a run.", 50),
            new AchievementDefinition("score_100", "100 Points", "Reach 100 points in a run.", 100),
            new AchievementDefinition("first_win", "First Win", "Win your first level.", 1),
            new AchievementDefinition("win_5", "Five Wins", "Win a total of 5 levels.", 5),
            new AchievementDefinition("collector", "Collector", "Create all merge values from 4 to 512.", 8)
        };
    }

    private void CreatePopup()
    {
        popupObject = CreateImageObject("AchievementPopup", transform, new Color(0.19f, 0.11f, 0.06f, 0.95f));
        RectTransform popupRect = popupObject.GetComponent<RectTransform>();
        popupRect.anchorMin = new Vector2(0.5f, 1f);
        popupRect.anchorMax = new Vector2(0.5f, 1f);
        popupRect.pivot = new Vector2(0.5f, 1f);
        popupRect.anchoredPosition = new Vector2(0f, -120f);
        popupRect.sizeDelta = new Vector2(520f, 120f);
        popupObject.GetComponent<Image>().raycastTarget = false;

        popupText = CreateText("PopupText", popupObject.transform, string.Empty, 28, TextAnchor.MiddleCenter);
        popupText.color = Color.white;
        popupText.raycastTarget = false;
        RectTransform popupTextRect = popupText.GetComponent<RectTransform>();
        popupTextRect.anchorMin = Vector2.zero;
        popupTextRect.anchorMax = Vector2.one;
        popupTextRect.offsetMin = new Vector2(18f, 18f);
        popupTextRect.offsetMax = new Vector2(-18f, -18f);

        popupObject.SetActive(false);
    }

    private void CreateWinPanelSummary()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null || gameManager.winPanel == null)
        {
            return;
        }

        winPanelAchievementText = CreateText("WinPanelAchievementText", gameManager.winPanel.transform, string.Empty, 20, TextAnchor.UpperCenter);
        RectTransform summaryRect = winPanelAchievementText.GetComponent<RectTransform>();
        summaryRect.anchorMin = new Vector2(0.12f, 0.18f);
        summaryRect.anchorMax = new Vector2(0.88f, 0.40f);
        summaryRect.offsetMin = Vector2.zero;
        summaryRect.offsetMax = Vector2.zero;
        winPanelAchievementText.gameObject.SetActive(false);
    }

    private void RefreshUi()
    {
        if (toggleButtonText != null)
        {
            if (isMenuScene)
            {
                toggleButtonText.text = "Achievements\n" + manager.GetUnlockedCount() + "/" + manager.GetTotalCount();
            }
            else
            {
                toggleButtonText.text = manager.GetSummaryText();
            }
        }

        if (summaryText != null)
        {
            summaryText.text = "Unlocked " + manager.GetUnlockedCount() + " of " + manager.GetTotalCount();
        }

        if (winPanelAchievementText != null)
        {
            string winPanelSummary = manager.GetSessionUnlockSummary(3);
            bool hasSummary = !string.IsNullOrEmpty(winPanelSummary);
            winPanelAchievementText.gameObject.SetActive(hasSummary);
            if (hasSummary)
            {
                winPanelAchievementText.text = winPanelSummary;
            }
        }

        for (int i = 0; i < manager.Definitions.Count; i++)
        {
            AchievementDefinition definition = manager.Definitions[i];
            bool unlocked = manager.IsUnlocked(definition);

            rowTexts[i].text = definition.Title + " - " + (unlocked ? "Unlocked" : "Locked") +
                               "\n" + definition.Description + "  Progress: " + manager.GetProgressText(definition);
            rowTexts[i].color = unlocked ? Color.white : new Color(1f, 0.93f, 0.87f, 0.75f);
            rowBackgrounds[i].color = unlocked
                ? new Color(0.28f, 0.44f, 0.22f, 0.96f)
                : new Color(0.35f, 0.22f, 0.12f, 0.95f);
        }
    }

    private void HandleAchievementUnlocked(AchievementDefinition definition)
    {
        RefreshUi();

        if (isMenuScene || popupObject == null)
        {
            return;
        }

        popupQueue.Enqueue(definition);
        if (popupRoutine == null)
        {
            popupRoutine = StartCoroutine(ShowPopupQueue());
        }
    }

    private IEnumerator ShowPopupQueue()
    {
        while (popupQueue.Count > 0)
        {
            AchievementDefinition definition = popupQueue.Dequeue();
            popupObject.SetActive(true);
            popupText.text = "Achievement Unlocked\n" + definition.Title;

            yield return new WaitForSecondsRealtime(2.25f);

            popupObject.SetActive(false);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        popupRoutine = null;
    }

    private void TogglePanel()
    {
        SetPanelVisible(!panelOverlay.activeSelf);
    }

    private void SetPanelVisible(bool visible)
    {
        if (panelOverlay == null)
        {
            return;
        }

        panelOverlay.SetActive(visible);

        if (isMenuScene)
        {
            return;
        }

        if (visible)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = previousTimeScale;
        }
    }

    private Canvas FindOrCreateCanvas()
    {
        Canvas existingCanvas = FindObjectOfType<Canvas>();
        if (existingCanvas != null)
        {
            return existingCanvas;
        }

        GameObject canvasObject = new GameObject("AchievementCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1f;

        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private void EnsureEventSystemExists()
    {
        if (FindObjectOfType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
    }

    private GameObject CreateUiObject(string objectName, Transform parent)
    {
        GameObject uiObject = new GameObject(objectName, typeof(RectTransform));
        uiObject.transform.SetParent(parent, false);
        return uiObject;
    }

    private GameObject CreateImageObject(string objectName, Transform parent, Color color)
    {
        GameObject uiObject = CreateUiObject(objectName, parent);
        Image image = uiObject.AddComponent<Image>();
        image.color = color;
        return uiObject;
    }

    private Text CreateText(string objectName, Transform parent, string value, int fontSize, TextAnchor textAnchor)
    {
        GameObject textObject = CreateUiObject(objectName, parent);
        Text text = textObject.AddComponent<Text>();
        text.font = defaultFont;
        text.text = value;
        text.fontSize = fontSize;
        text.alignment = textAnchor;
        text.color = Color.white;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        return text;
    }

    private Button CreateButton(string objectName, Transform parent, string label)
    {
        GameObject buttonObject = CreateImageObject(objectName, parent, new Color(0.51f, 0.31f, 0.16f, 0.98f));
        Button button = buttonObject.AddComponent<Button>();

        ColorBlock colors = button.colors;
        colors.normalColor = new Color(1f, 1f, 1f, 1f);
        colors.highlightedColor = new Color(0.94f, 0.88f, 0.76f, 1f);
        colors.pressedColor = new Color(0.84f, 0.75f, 0.58f, 1f);
        colors.selectedColor = colors.highlightedColor;
        button.colors = colors;

        Text labelText = CreateText("Label", buttonObject.transform, label, 24, TextAnchor.MiddleCenter);
        RectTransform labelRect = labelText.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = new Vector2(10f, 8f);
        labelRect.offsetMax = new Vector2(-10f, -8f);

        return button;
    }
}
