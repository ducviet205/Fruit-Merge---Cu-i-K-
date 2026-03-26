using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuStoreUI : MonoBehaviour
{
    private const string GeneratedRootName = "StoreGeneratedUI";

    [Serializable]
    private sealed class SkinCardView
    {
        public string skinId;
        public Image backgroundImage;
        public Image accentImage;
        public Image iconImage;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI priceText;
        public Button actionButton;
        public Image actionButtonImage;
        public TextMeshProUGUI actionButtonText;
    }

    private sealed class SkinItem
    {
        public SkinItem(string id, string title, string description, int price, string resourcePath, Color accent)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            ResourcePath = resourcePath;
            Accent = accent;
        }

        public string Id { get; }
        public string Title { get; }
        public string Description { get; }
        public int Price { get; }
        public string ResourcePath { get; }
        public Color Accent { get; }
        public Sprite Icon { get; set; }
    }

    [SerializeField] private RectTransform generatedRoot;
    [SerializeField] private GameObject overlayObject;
    [SerializeField] private Image coinsBadgeImage;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private Button storeButton;
    [SerializeField] private Image storeButtonImage;
    [SerializeField] private TextMeshProUGUI storeButtonText;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image selectedIconImage;
    [SerializeField] private TextMeshProUGUI selectedTitleText;
    [SerializeField] private TextMeshProUGUI selectedDescriptionText;
    [SerializeField] private List<SkinCardView> cardViews = new List<SkinCardView>();

    private TMP_FontAsset sharedFont;
    private Material sharedFontMaterial;
    private Sprite buttonSprite;
    private Sprite backgroundSprite;
    private Sprite homeSprite;
    private Sprite sunRaysSprite;
    private bool listenersBound;

    private readonly SkinItem[] items =
    {
        new SkinItem("blue", "Blue Berry", "Mascot mac dinh, tuoi va de thuong.", 0, "Store/Skins/BlueBerry", new Color(0.22f, 0.67f, 0.98f, 1f)),
        new SkinItem("green", "Mint Melon", "Tong xanh mat mat, rat hop menu.", 45, "Store/Skins/MintMelon", new Color(0.27f, 0.77f, 0.38f, 1f)),
        new SkinItem("orange", "Sun Orange", "Tong cam nong va noi bat.", 60, "Store/Skins/SunOrange", new Color(0.98f, 0.59f, 0.24f, 1f)),
        new SkinItem("purple", "Berry Plum", "Tong tim ngot, tao diem nhan.", 75, "Store/Skins/BerryPlum", new Color(0.59f, 0.42f, 0.88f, 1f)),
        new SkinItem("red", "Ruby Peach", "Tong hong do dam, rat vui mat.", 95, "Store/Skins/RubyPeach", new Color(0.95f, 0.33f, 0.46f, 1f)),
        new SkinItem("yellow", "Golden Lemon", "Skin vang sang va hiem hon.", 120, "Store/Skins/GoldenLemon", new Color(1f, 0.83f, 0.17f, 1f))
    };

    private void Start()
    {
        RefreshUi();
    }

    private void Update()
    {
        if (overlayObject != null && overlayObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseStore();
        }
    }

    public void BuildGeneratedUi()
    {
        LoadAssets();
        ResolveTextStyle();
        ClearGeneratedUi();

        RectTransform rootRect = transform as RectTransform;
        if (rootRect == null)
        {
            return;
        }

        generatedRoot = CreateRect(GeneratedRootName, rootRect, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Vector2.zero);
        generatedRoot.SetAsLastSibling();

        BuildCoinsBadge();
        BuildStoreButton();
        BuildOverlay();

        listenersBound = false;
        RefreshUi();

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(this);
        }
#endif
    }

    public void DeleteGeneratedUi()
    {
        ClearGeneratedUi();
    }

    public void RefreshUi()
    {
        if (!EnsureReady())
        {
            return;
        }

        string selectedSkinId = StoreData.GetSelectedSkinId();
        SkinItem selectedItem = GetItem(selectedSkinId) ?? items[0];

        if (coinsText != null)
        {
            coinsText.text = "Xu " + StoreData.GetCoins();
        }

        if (storeButtonImage != null)
        {
            storeButtonImage.color = selectedItem.Accent;
        }

        if (selectedIconImage != null)
        {
            selectedIconImage.sprite = selectedItem.Icon;
        }

        if (selectedTitleText != null)
        {
            selectedTitleText.text = selectedItem.Title;
        }

        if (selectedDescriptionText != null)
        {
            selectedDescriptionText.text = "Skin dang dung cho mascot menu. Thang game de nhan them xu va mo skin moi.";
        }

        for (int i = 0; i < cardViews.Count; i++)
        {
            RefreshCard(cardViews[i], selectedSkinId);
        }
    }

    private bool EnsureReady()
    {
        LoadAssets();
        ResolveTextStyle();

        if (generatedRoot == null)
        {
            generatedRoot = transform.Find(GeneratedRootName) as RectTransform;
        }

        if (generatedRoot == null)
        {
            return false;
        }

        if (coinsBadgeImage == null)
        {
            BindReferences();
        }

        if (!listenersBound)
        {
            HookListeners();
        }

        return true;
    }

    private void BindReferences()
    {
        Transform coinsTransform = generatedRoot.Find("CoinsBadge");
        if (coinsTransform != null)
        {
            coinsBadgeImage = coinsTransform.GetComponent<Image>();
            coinsText = coinsTransform.Find("Label")?.GetComponent<TextMeshProUGUI>();
        }

        Transform buttonTransform = generatedRoot.Find("StoreButton");
        if (buttonTransform != null)
        {
            storeButton = buttonTransform.GetComponent<Button>();
            storeButtonImage = buttonTransform.GetComponent<Image>();
            storeButtonText = buttonTransform.Find("Label")?.GetComponent<TextMeshProUGUI>();
        }

        Transform overlayTransform = generatedRoot.Find("StoreOverlay");
        if (overlayTransform != null)
        {
            overlayObject = overlayTransform.gameObject;
            closeButton = overlayTransform.Find("StorePanel/Header/CloseButton")?.GetComponent<Button>();
            selectedIconImage = overlayTransform.Find("StorePanel/SelectedCard/IconPlate/Icon")?.GetComponent<Image>();
            selectedTitleText = overlayTransform.Find("StorePanel/SelectedCard/Title")?.GetComponent<TextMeshProUGUI>();
            selectedDescriptionText = overlayTransform.Find("StorePanel/SelectedCard/Description")?.GetComponent<TextMeshProUGUI>();
        }

        if (cardViews.Count == 0 && overlayObject != null)
        {
            Transform contentTransform = overlayObject.transform.Find("StorePanel/ScrollView/Viewport/Content");
            if (contentTransform != null)
            {
                cardViews.Clear();
                for (int i = 0; i < contentTransform.childCount; i++)
                {
                    Transform cardTransform = contentTransform.GetChild(i);
                    cardViews.Add(new SkinCardView
                    {
                        skinId = cardTransform.name.Replace("Card_", string.Empty).ToLowerInvariant(),
                        backgroundImage = cardTransform.GetComponent<Image>(),
                        accentImage = cardTransform.Find("Accent")?.GetComponent<Image>(),
                        iconImage = cardTransform.Find("IconPlate/Icon")?.GetComponent<Image>(),
                        titleText = cardTransform.Find("Title")?.GetComponent<TextMeshProUGUI>(),
                        descriptionText = cardTransform.Find("Description")?.GetComponent<TextMeshProUGUI>(),
                        priceText = cardTransform.Find("PricePill/Label")?.GetComponent<TextMeshProUGUI>(),
                        actionButton = cardTransform.Find("ActionButton")?.GetComponent<Button>(),
                        actionButtonImage = cardTransform.Find("ActionButton")?.GetComponent<Image>(),
                        actionButtonText = cardTransform.Find("ActionButton/Label")?.GetComponent<TextMeshProUGUI>()
                    });
                }
            }
        }
    }

    private void HookListeners()
    {
        if (storeButton != null)
        {
            storeButton.onClick.RemoveAllListeners();
            storeButton.onClick.AddListener(OpenStore);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(CloseStore);
        }

        for (int i = 0; i < cardViews.Count; i++)
        {
            SkinCardView cardView = cardViews[i];
            if (cardView.actionButton == null)
            {
                continue;
            }

            string skinId = cardView.skinId;
            cardView.actionButton.onClick.RemoveAllListeners();
            cardView.actionButton.onClick.AddListener(() => HandleCardAction(skinId));
        }

        listenersBound = true;
    }

    private void BuildCoinsBadge()
    {
        RectTransform badge = CreateRect("CoinsBadge", generatedRoot, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-30f, -34f), new Vector2(220f, 72f));
        coinsBadgeImage = badge.gameObject.AddComponent<Image>();
        coinsBadgeImage.sprite = buttonSprite;
        coinsBadgeImage.color = new Color(0.33f, 0.25f, 0.2f, 0.96f);
        coinsText = CreateText("Label", badge, "Xu 0", 28f, FontStyles.Bold, Color.white, Vector2.zero, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-16f, -10f), TextAlignmentOptions.Center);
    }

    private void BuildStoreButton()
    {
        RectTransform buttonRect = CreateRect("StoreButton", generatedRoot, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -350f), new Vector2(320f, 96f));
        storeButtonImage = buttonRect.gameObject.AddComponent<Image>();
        storeButtonImage.sprite = buttonSprite;
        storeButtonImage.color = items[0].Accent;
        storeButton = buttonRect.gameObject.AddComponent<Button>();
        storeButton.targetGraphic = storeButtonImage;
        storeButtonText = CreateText("Label", buttonRect, "CUA HANG", 34f, FontStyles.Bold, Color.white, Vector2.zero, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-20f, -14f), TextAlignmentOptions.Center);
    }

    private void BuildOverlay()
    {
        RectTransform overlayRect = CreateRect("StoreOverlay", generatedRoot, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Vector2.zero);
        overlayObject = overlayRect.gameObject;
        Image overlayImage = overlayObject.AddComponent<Image>();
        overlayImage.color = new Color(0.08f, 0.07f, 0.08f, 0.8f);
        overlayObject.SetActive(false);

        RectTransform panel = CreateRect("StorePanel", overlayRect, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(650f, 1020f));
        Image panelImage = panel.gameObject.AddComponent<Image>();
        panelImage.color = new Color(0.95f, 0.98f, 1f, 0.99f);
        CreateImage("PanelBg", panel, backgroundSprite, new Color(1f, 1f, 1f, 0.05f), Vector2.zero, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

        RectTransform header = CreateRect("Header", panel, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -28f), new Vector2(580f, 126f));
        Image headerImage = header.gameObject.AddComponent<Image>();
        headerImage.color = new Color(0.2f, 0.58f, 0.88f, 0.98f);
        CreateText("Title", header, "FRUIT STORE", 42f, FontStyles.Bold, Color.white, Vector2.zero, new Vector2(0f, 0.5f), new Vector2(1f, 1f), new Vector2(26f, -18f), new Vector2(-140f, 18f), TextAlignmentOptions.Left);
        CreateText("SubTitle", header, "Mua skin va doi mascot menu", 21f, FontStyles.Normal, new Color(0.92f, 0.97f, 1f, 1f), Vector2.zero, new Vector2(0f, 0f), new Vector2(1f, 0.5f), new Vector2(26f, 16f), new Vector2(-150f, -16f), TextAlignmentOptions.Left);

        RectTransform closeRect = CreateRect("CloseButton", header, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-52f, 0f), new Vector2(72f, 72f));
        Image closeImage = closeRect.gameObject.AddComponent<Image>();
        closeImage.sprite = buttonSprite;
        closeImage.color = new Color(0.99f, 0.74f, 0.24f, 1f);
        closeButton = closeRect.gameObject.AddComponent<Button>();
        closeButton.targetGraphic = closeImage;
        CreateText("Label", closeRect, "X", 34f, FontStyles.Bold, new Color(0.37f, 0.19f, 0.06f, 1f), Vector2.zero, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-8f, -8f), TextAlignmentOptions.Center);

        RectTransform selectedCard = CreateRect("SelectedCard", panel, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -182f), new Vector2(580f, 132f));
        Image selectedCardImage = selectedCard.gameObject.AddComponent<Image>();
        selectedCardImage.color = new Color(1f, 1f, 1f, 0.98f);
        CreateImage("Glow", selectedCard, sunRaysSprite, new Color(1f, 0.88f, 0.32f, 0.05f), Vector2.zero, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(78f, 0f), new Vector2(130f, 130f));
        RectTransform iconPlate = CreateRect("IconPlate", selectedCard, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(78f, 0f), new Vector2(100f, 100f));
        Image iconPlateImage = iconPlate.gameObject.AddComponent<Image>();
        iconPlateImage.color = items[0].Accent;
        selectedIconImage = CreateImage("Icon", iconPlate, items[0].Icon, Color.white, Vector2.zero, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(72f, 72f));
        selectedTitleText = CreateText("Title", selectedCard, items[0].Title, 30f, FontStyles.Bold, new Color(0.34f, 0.19f, 0.08f, 1f), Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(146f, -18f), new Vector2(390f, 34f), TextAlignmentOptions.Left);
        selectedTitleText.rectTransform.pivot = new Vector2(0f, 1f);
        selectedDescriptionText = CreateText("Description", selectedCard, "Skin dang dung cho mascot menu.", 18f, FontStyles.Normal, new Color(0.42f, 0.29f, 0.16f, 1f), Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(146f, -54f), new Vector2(390f, 48f), TextAlignmentOptions.Left);
        selectedDescriptionText.rectTransform.pivot = new Vector2(0f, 1f);
        selectedDescriptionText.enableWordWrapping = true;

        RectTransform scrollView = CreateRect("ScrollView", panel, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, -118f), new Vector2(580f, 480f));
        Image scrollBg = scrollView.gameObject.AddComponent<Image>();
        scrollBg.color = new Color(1f, 1f, 1f, 0.38f);
        ScrollRect scrollRect = scrollView.gameObject.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;

        RectTransform viewport = CreateRect("Viewport", scrollView, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero, Vector2.zero);
        Image viewportImage = viewport.gameObject.AddComponent<Image>();
        viewportImage.color = new Color(1f, 1f, 1f, 0.02f);
        Mask mask = viewport.gameObject.AddComponent<Mask>();
        mask.showMaskGraphic = false;

        RectTransform content = CreateRect("Content", viewport, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), Vector2.zero, new Vector2(-16f, 0f));
        VerticalLayoutGroup layoutGroup = content.gameObject.AddComponent<VerticalLayoutGroup>();
        layoutGroup.spacing = 16f;
        layoutGroup.padding = new RectOffset(10, 10, 12, 12);
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childForceExpandHeight = false;
        ContentSizeFitter fitter = content.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        scrollRect.viewport = viewport;
        scrollRect.content = content;

        cardViews.Clear();
        for (int i = 0; i < items.Length; i++)
        {
            cardViews.Add(BuildSkinCard(content, items[i]));
        }

        RectTransform footer = CreateRect("Footer", panel, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 30f), new Vector2(580f, 78f));
        Image footerImage = footer.gameObject.AddComponent<Image>();
        footerImage.color = new Color(0.98f, 0.54f, 0.2f, 1f);
        CreateText("Label", footer, "Them skin moi se duoc cap nhat tiep", 24f, FontStyles.Bold, Color.white, Vector2.zero, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-16f, -12f), TextAlignmentOptions.Center);
    }

    private SkinCardView BuildSkinCard(RectTransform parent, SkinItem item)
    {
        RectTransform card = CreateRect("Card_" + item.Id.ToUpperInvariant(), parent, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, 148f));
        LayoutElement layoutElement = card.gameObject.AddComponent<LayoutElement>();
        layoutElement.preferredHeight = 148f;

        Image cardImage = card.gameObject.AddComponent<Image>();
        cardImage.color = new Color(1f, 1f, 1f, 0.96f);

        Image accent = CreateImage("Accent", card, null, item.Accent, Vector2.zero, new Vector2(0f, 0f), new Vector2(0f, 1f), Vector2.zero, new Vector2(18f, 0f));
        RectTransform iconPlate = CreateRect("IconPlate", card, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(78f, 0f), new Vector2(86f, 86f));
        Image iconPlateImage = iconPlate.gameObject.AddComponent<Image>();
        iconPlateImage.color = new Color(0.96f, 0.97f, 1f, 1f);
        Image icon = CreateImage("Icon", iconPlate, item.Icon, Color.white, Vector2.zero, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(68f, 68f));
        icon.preserveAspect = true;

        RectTransform infoRoot = CreateRect("InfoRoot", card, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        infoRoot.offsetMin = new Vector2(132f, 14f);
        infoRoot.offsetMax = new Vector2(-196f, -14f);

        TextMeshProUGUI title = CreateText("Title", infoRoot, item.Title, 27f, FontStyles.Bold, new Color(0.34f, 0.2f, 0.08f, 1f), Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, -2f), new Vector2(250f, 34f), TextAlignmentOptions.Left);
        title.rectTransform.pivot = new Vector2(0f, 1f);
        TextMeshProUGUI description = CreateText("Description", infoRoot, item.Description, 18f, FontStyles.Normal, new Color(0.42f, 0.29f, 0.16f, 1f), Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, -42f), new Vector2(250f, 48f), TextAlignmentOptions.Left);
        description.rectTransform.pivot = new Vector2(0f, 1f);
        description.enableWordWrapping = true;

        RectTransform pricePill = CreateRect("PricePill", infoRoot, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(110f, 38f));
        Image pricePillImage = pricePill.gameObject.AddComponent<Image>();
        pricePillImage.sprite = buttonSprite;
        pricePillImage.color = LerpColor(item.Accent, Color.white, 0.2f);
        TextMeshProUGUI price = CreateText("Label", pricePill, item.Price == 0 ? "FREE" : item.Price + " XU", 19f, FontStyles.Bold, item.Accent, Vector2.zero, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-10f, -8f), TextAlignmentOptions.Center);

        RectTransform actionArea = CreateRect("ActionArea", card, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(1f, 0.5f), new Vector2(-20f, 0f), new Vector2(164f, -24f));
        RectTransform actionButtonRect = CreateRect("ActionButton", actionArea, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(158f, 56f));
        Image actionButtonImage = actionButtonRect.gameObject.AddComponent<Image>();
        actionButtonImage.sprite = buttonSprite;
        actionButtonImage.color = item.Accent;
        Button actionButton = actionButtonRect.gameObject.AddComponent<Button>();
        actionButton.targetGraphic = actionButtonImage;
        TextMeshProUGUI actionButtonText = CreateText("Label", actionButtonRect, "MUA", 21f, FontStyles.Bold, Color.white, Vector2.zero, Vector2.zero, Vector2.one, Vector2.zero, new Vector2(-18f, -12f), TextAlignmentOptions.Center);

        return new SkinCardView
        {
            skinId = item.Id,
            backgroundImage = cardImage,
            accentImage = accent,
            iconImage = icon,
            titleText = title,
            descriptionText = description,
            priceText = price,
            actionButton = actionButton,
            actionButtonImage = actionButtonImage,
            actionButtonText = actionButtonText
        };
    }

    private void HandleCardAction(string skinId)
    {
        SkinItem item = GetItem(skinId);
        if (item == null)
        {
            return;
        }

        if (!StoreData.IsOwned(skinId) && !StoreData.Purchase(skinId, item.Price))
        {
            FlashCoinsBadge(new Color(0.92f, 0.34f, 0.32f, 1f));
            return;
        }

        StoreData.SelectSkin(skinId);
        FlashCoinsBadge(item.Accent);
        RefreshUi();
    }

    private void RefreshCard(SkinCardView cardView, string selectedSkinId)
    {
        SkinItem item = GetItem(cardView.skinId);
        if (item == null)
        {
            return;
        }

        bool owned = StoreData.IsOwned(item.Id);
        bool selected = selectedSkinId == item.Id;

        if (cardView.backgroundImage != null)
        {
            cardView.backgroundImage.color = selected ? new Color(0.93f, 0.99f, 0.94f, 0.98f) : new Color(1f, 1f, 1f, 0.96f);
        }

        if (cardView.accentImage != null)
        {
            cardView.accentImage.color = selected ? LerpColor(item.Accent, Color.white, 0.12f) : item.Accent;
        }

        if (cardView.priceText != null)
        {
            cardView.priceText.text = item.Price == 0 ? "FREE" : item.Price + " XU";
            cardView.priceText.color = selected ? new Color(0.23f, 0.57f, 0.31f, 1f) : item.Accent;
        }

        if (cardView.actionButtonImage != null)
        {
            cardView.actionButtonImage.color = selected ? new Color(0.28f, 0.7f, 0.36f, 1f) : owned ? new Color(0.97f, 0.66f, 0.24f, 1f) : item.Accent;
        }

        if (cardView.actionButtonText != null)
        {
            cardView.actionButtonText.text = selected ? "DANG DUNG" : owned ? "CHON" : "MUA";
        }
    }

    private void OpenStore()
    {
        if (overlayObject != null)
        {
            overlayObject.SetActive(true);
            RefreshUi();
        }
    }

    private void CloseStore()
    {
        if (overlayObject != null)
        {
            overlayObject.SetActive(false);
        }
    }

    private void FlashCoinsBadge(Color color)
    {
        if (coinsBadgeImage == null)
        {
            return;
        }

        coinsBadgeImage.color = color;
        CancelInvoke(nameof(RefreshUi));
        Invoke(nameof(RefreshUi), 0.18f);
    }

    private void LoadAssets()
    {
        if (buttonSprite == null)
        {
            buttonSprite = Resources.Load<Sprite>("Store/Button");
            backgroundSprite = Resources.Load<Sprite>("Store/BG");
            homeSprite = Resources.Load<Sprite>("Store/Home");
            sunRaysSprite = Resources.Load<Sprite>("Store/SunRays");
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Icon == null)
            {
                items[i].Icon = Resources.Load<Sprite>(items[i].ResourcePath);
            }
        }
    }

    private void ResolveTextStyle()
    {
        if (sharedFont != null)
        {
            return;
        }

        TextMeshProUGUI existingText = GetComponentInChildren<TextMeshProUGUI>(true);
        if (existingText != null)
        {
            sharedFont = existingText.font;
            sharedFontMaterial = existingText.fontSharedMaterial;
        }

        if (sharedFont == null)
        {
            sharedFont = TMP_Settings.defaultFontAsset;
        }
    }

    private void ClearGeneratedUi()
    {
        if (generatedRoot != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(generatedRoot.gameObject);
            }
            else
#endif
            {
                Destroy(generatedRoot.gameObject);
            }
        }

        generatedRoot = null;
        overlayObject = null;
        coinsBadgeImage = null;
        coinsText = null;
        storeButton = null;
        storeButtonImage = null;
        storeButtonText = null;
        closeButton = null;
        selectedIconImage = null;
        selectedTitleText = null;
        selectedDescriptionText = null;
        cardViews.Clear();
        listenersBound = false;
    }

    private RectTransform CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        GameObject gameObject = new GameObject(name, typeof(RectTransform));
        gameObject.transform.SetParent(parent, false);
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;
        rect.localScale = Vector3.one;
        return rect;
    }

    private Image CreateImage(string name, RectTransform parent, Sprite sprite, Color color, Vector2 scale, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        RectTransform rect = CreateRect(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), anchoredPosition, sizeDelta);
        rect.localScale = scale == Vector2.zero ? Vector3.one : new Vector3(scale.x, scale.y, 1f);
        Image image = rect.gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        return image;
    }

    private TextMeshProUGUI CreateText(string name, RectTransform parent, string value, float fontSize, FontStyles style, Color color, Vector2 scale, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta, TextAlignmentOptions alignment)
    {
        RectTransform rect = CreateRect(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), anchoredPosition, sizeDelta);
        rect.localScale = scale == Vector2.zero ? Vector3.one : new Vector3(scale.x, scale.y, 1f);

        TextMeshProUGUI text = rect.gameObject.AddComponent<TextMeshProUGUI>();
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.alignment = alignment;
        text.color = color;
        text.raycastTarget = false;

        if (sharedFont != null)
        {
            text.font = sharedFont;
        }

        if (sharedFontMaterial != null)
        {
            text.fontSharedMaterial = sharedFontMaterial;
        }

        return text;
    }

    private SkinItem GetItem(string skinId)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Id == skinId)
            {
                return items[i];
            }
        }

        return null;
    }

    private static Color LerpColor(Color from, Color to, float t)
    {
        return Color.Lerp(from, to, Mathf.Clamp01(t));
    }
}
