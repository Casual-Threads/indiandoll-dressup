using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[System.Serializable]
public class GamePlayElements
{
    [Header("Panels")]
    public GameObject gamePanell;
    public GameObject submitPanel, screenShotPanel, adPanel, loadingPanel;
    [Header("PopUp")]
    public GameObject enoughCoinPopUp;
    public GameObject videoAdNotAvailablePopUp;
    [Header("Scrollers")]
    public GameObject allScroller;
    public GameObject dressScroller, bangleScroller, earingScroller, necklaceScroller, mathapatiScroller, noseringScroller, bindiScroller, mehandiScroller, bagScroller, hairScroller, lipsScroller,
                      blushScroller, closedEyeshadeScroller, eyeshadeScroller;
    [Header("UI")]
    public GameObject coinSlot;
    public GameObject homeBtn, nextBtn, submitPanelbar, lastBtns, rateUsBtn;
    [Header("GamePlay Image")]
    public Image fillbar;
    public Image screenShotImage;
}

[System.Serializable]
public class GamePlayPlayerElenemts
{
    [Header("Player Character")]
    public GameObject character;
    [Header("Player Images")]
    public Image dressImage;
    public Image bangleImage, earingImage, necklaceImage, mathapatiImage, noseringImage, bindiImage, mehandiImage, bagImage, lipsImage, hairImage, blushImage, closedEyeshadeImage, eyeshadeImage;
    [Header("Player Score Text")]
    [Header("Player ScoreBox")]
    public GameObject playerScoreBox;
    public Text txtPlayerScore;
}

[System.Serializable]
public class GamePlayOpponentElenemts
{
    [Header("Opponents Character")]
    public GameObject characterLeft;
    public GameObject characterRight;
    [Header("Left Opponent Images")]
    public Image leftCharacterBody;
    public Image leftCharacterFace, leftOppodressImage, leftOppobangleImage, leftOppoearingImage, leftOpponecklaceImage, leftOppomathapatiImage, leftOpponoseringImage, leftOppobindiImage,
                 leftOppomehandiImage, leftOppobagImage, leftOppohairImage, leftOppolipsImage, leftOppoblushImage, leftOppoclosedEyeshadeImage, leftOppoeyeshadeImage;
    [Header("Right Opponent Images")]
    public Image rightCharacterBody;
    public Image rightCharacterFace, rightOppodressImage, rightOppobangleImage, rightOppoearingImage, rightOpponecklaceImage, rightOppomathapatiImage, rightOpponoseringImage,
                 rightOppobindiImage, rightOppomehandiImage, rightOppobagImage, rightOppohairImage, rightOppolipsImage, rightOppoblushImage, rightOppoclosedEyeshadeImage, rightOppoeyeshadeImage;
    [Header("Opponents ScoreBox")]
    public GameObject leftOppoScoreBox;
    public GameObject rightOppoScoreBox;
    [Header("Opponents Text")]
    public Text txtLeftOppoScore;
    public Text txtRightOppoScore;
}

[System.Serializable]
public enum GamePlaySelectedItem
{
    dress, bangle, earing, necklace, mathapati, nosering, bindi, mehandi, bag, hair, lips, blush, closedEyeshade, eyeshade
}

public class GamePlay : MonoBehaviour
{
    public static GamePlay Instance;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public GamePlaySelectedItem selectedItem;
    [FoldoutGroup("UI Elements")]
    [HideLabel]
    public GamePlayElements uIElements;
    [FoldoutGroup("Player Elements")]
    [HideLabel]
    public GamePlayPlayerElenemts playerElements;
    [FoldoutGroup("Opponent Elements")]
    [HideLabel]
    public GamePlayOpponentElenemts oppoElements;
    [Header("Mover Item")]
    public MRS_Manager playerCharacterMover;
    public MRS_Manager oppoLeftCharacterMover;
    public MRS_Manager oppoRightCharacterMover;
    public CoinsAdder coinsAdder;
    [Header("Sprites Array")]
    public Sprite[] dressSprites;
    public Sprite[] bangleSprites;
    public Sprite[] earingSprites;
    public Sprite[] necklaceSprites;
    public Sprite[] mathapatiSprites;
    public Sprite[] noseringSprites;
    public Sprite[] bindiSprites;
    public Sprite[] mehandiSprites;
    public Sprite[] bagSprites;
    public Sprite[] hairSprites;
    public Sprite[] lipsSprites;
    public Sprite[] blushSprites;
    public Sprite[] closedEyeshadeSprites;
    public Sprite[] eyeshadeSprites;
    public Sprite[] defaultBtnSprites;
    public Sprite[] selectedBtnSprites;
    [Header("Scroller Btn Image Array")]
    public Image[] categoryBtn;
    [Header("Item List")]
    private List<ItemInfo> dressList = new List<ItemInfo>();
    private List<ItemInfo> bangleList = new List<ItemInfo>();
    private List<ItemInfo> earingList = new List<ItemInfo>();
    private List<ItemInfo> necklaceList = new List<ItemInfo>();
    private List<ItemInfo> mathapatiList = new List<ItemInfo>();
    private List<ItemInfo> noseringList = new List<ItemInfo>();
    private List<ItemInfo> bindiList = new List<ItemInfo>();
    private List<ItemInfo> mehandiList = new List<ItemInfo>();
    private List<ItemInfo> bagList = new List<ItemInfo>();
    private List<ItemInfo> hairList = new List<ItemInfo>();
    private List<ItemInfo> lipsList = new List<ItemInfo>();
    private List<ItemInfo> blushList = new List<ItemInfo>();
    private List<ItemInfo> closedEyeshadeList = new List<ItemInfo>();
    private List<ItemInfo> eyeshadeList = new List<ItemInfo>();

    [Header("Sprites")]
    public Sprite selectionSelectedSprite;
    public Sprite selectionDefaultSprite;
    [Header("Text")]
    public Text totalCoins;
    public Text waitAdLoadTime;
    [Header("ItemInfo Variable")]
    private ItemInfo tempItem;
    [Header("Different Index")]
    private int selectedIndex;
    [Header("Bool Variable")]
    private bool canShowInterstitial;
    [Header("Particles")]
    public ParticleSystem taskPartical;
    public GameObject finalPartical;
    [Header("AudioSources")]
    public AudioSource categorySFX;
    public AudioSource purchaseSFX, scoreSFX;
    public AudioSource[] voiceSounds;
    public Animator eyeAnim;
    public Image characterBody;
    public Image characterFace;
    public Sprite[] characterBodys;
    public Sprite[] characterFaces;
    public Sprite[] closeEyeSprites;
    public Image closeEyesImage;

    bool Isdress, Isbangle, Isearing, Isnecklace, Ismathapati, Isnosering, Isbindi, Ismehandi, Isbag, Ishair, Islips, Isblush, IsclosedEyeshade, Iseyeshade;

    private enum RewardType
    {
        none, coins, selectionItem
    }
    private RewardType rewardType;

    #region Start
    // Start is called before the first frame update
    void Start()
    {
        Usman_SaveLoad.LoadProgress();
        characterBody.sprite = characterBodys[SaveData.Instance.selectedCharacter];
        characterFace.sprite = characterFaces[SaveData.Instance.selectedCharacter];
        closeEyesImage.sprite = closeEyeSprites[SaveData.Instance.selectedCharacter];
        selectedItem = GamePlaySelectedItem.dress;
        uIElements.dressScroller.SetActive(true);
        SetInitialValues();
        GetItemsInfo();
        StartCoroutine(AdDelay(45));
        totalCoins.text = SaveData.Instance.Coins.ToString();
    }
    public void ShowInterstitial()
    {
        if (MyAdsManager.instance)
        {
            MyAdsManager.instance.ShowInterstitialAds();
        }
    }

    void OnEnable()
    {
        if (MyAdsManager.Instance != null)
        {
            MyAdsManager.Instance.onRewardedVideoAdCompletedEvent += OnRewardedVideoComplete;
        }
    }

    void OnDisable()
    {
        if (MyAdsManager.Instance != null)
        {
            MyAdsManager.Instance.onRewardedVideoAdCompletedEvent -= OnRewardedVideoComplete;
        }
    }
    #endregion

    #region SetInitialValues
    private void SetInitialValues()
    {
        //dress, bangle, earing, necklace, mathapati, nosering, bindi, mehandi, bag, hair, blush, closedEyeshade, eyeshade

        #region Initialing dress
        if (uIElements.dressScroller)
        {
            var dressInfo = uIElements.dressScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < dressInfo.Length; i++)
            {
                dressList.Add(dressInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.dress, dressList);
        SetItemIcon(dressList, dressSprites);
        #endregion

        #region Initialing bangle
        if (uIElements.bangleScroller)
        {
            var bangleInfo = uIElements.bangleScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < bangleInfo.Length; i++)
            {
                bangleList.Add(bangleInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.bangle, bangleList);
        SetItemIcon(bangleList, bangleSprites);
        #endregion

        #region Initialing earing
        if (uIElements.earingScroller)
        {
            var earingInfo = uIElements.earingScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < earingInfo.Length; i++)
            {
                earingList.Add(earingInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.earing, earingList);
        SetItemIcon(earingList, earingSprites);
        #endregion

        #region Initialing necklace
        if (uIElements.necklaceScroller)
        {
            var necklaceInfo = uIElements.necklaceScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < necklaceInfo.Length; i++)
            {
                necklaceList.Add(necklaceInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.necklace, necklaceList);
        SetItemIcon(necklaceList, necklaceSprites);
        #endregion

        #region Initialing mathapati
        if (uIElements.mathapatiScroller)
        {
            var mathapatiInfo = uIElements.mathapatiScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < mathapatiInfo.Length; i++)
            {
                mathapatiList.Add(mathapatiInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.mathapati, mathapatiList);
        SetItemIcon(mathapatiList, mathapatiSprites);
        #endregion

        #region Initialing nosering
        if (uIElements.noseringScroller)
        {
            var noseringInfo = uIElements.noseringScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < noseringInfo.Length; i++)
            {
                noseringList.Add(noseringInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.nosering, noseringList);
        SetItemIcon(noseringList, noseringSprites);
        #endregion

        #region Initialing bindi
        if (uIElements.bindiScroller)
        {
            var bindiInfo = uIElements.bindiScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < bindiInfo.Length; i++)
            {
                bindiList.Add(bindiInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.bindi, bindiList);
        SetItemIcon(bindiList, bindiSprites);
        #endregion

        #region Initialing mehandi
        if (uIElements.mehandiScroller)
        {
            var mehandiInfo = uIElements.mehandiScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < mehandiInfo.Length; i++)
            {
                mehandiList.Add(mehandiInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.mehandi, mehandiList);
        SetItemIcon(mehandiList, mehandiSprites);
        #endregion

        #region Initialing bag
        if (uIElements.bagScroller)
        {
            var bagInfo = uIElements.bagScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < bagInfo.Length; i++)
            {
                bagList.Add(bagInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.bag, bagList);
        SetItemIcon(bagList, bagSprites);
        #endregion

        #region Initialing hair
        if (uIElements.hairScroller)
        {
            var hairInfo = uIElements.hairScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < hairInfo.Length; i++)
            {
                hairList.Add(hairInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.hair, hairList);
        SetItemIcon(hairList, hairSprites);
        #endregion

        #region Initialing lips
        if (uIElements.lipsScroller)
        {
            var lipsInfo = uIElements.lipsScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < lipsInfo.Length; i++)
            {
                lipsList.Add(lipsInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.lips, lipsList);
        SetItemIcon(lipsList, lipsSprites);
        #endregion

        #region Initialing blush
        if (uIElements.blushScroller)
        {
            var blushInfo = uIElements.blushScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < blushInfo.Length; i++)
            {
                blushList.Add(blushInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.blush, blushList);
        SetItemIcon(blushList, blushSprites);
        #endregion

        #region Initialing closedEyeshade
        if (uIElements.closedEyeshadeScroller)
        {
            var closedEyeshadeInfo = uIElements.closedEyeshadeScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < closedEyeshadeInfo.Length; i++)
            {
                closedEyeshadeList.Add(closedEyeshadeInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.closedEyeshade, closedEyeshadeList);
        SetItemIcon(closedEyeshadeList, closedEyeshadeSprites);
        #endregion

        #region Initialing eyeshade
        if (uIElements.eyeshadeScroller)
        {
            var eyeshadeInfo = uIElements.eyeshadeScroller.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < eyeshadeInfo.Length; i++)
            {
                eyeshadeList.Add(eyeshadeInfo[i]);
            }
        }
        SetupItemData(SaveData.Instance.GamePlayModeElements.eyeshade, eyeshadeList);
        SetItemIcon(eyeshadeList, eyeshadeSprites);
        #endregion

        Usman_SaveLoad.SaveProgress();
    }
    #endregion

    #region SetupItemData
    private void SetupItemData(List<bool> unlockItems, List<ItemInfo> _ItemsInfo)
    {
        if (_ItemsInfo.Count > 0)
        {
            if (unlockItems.Count < _ItemsInfo.Count)
            {
                for (int i = 0; i < _ItemsInfo.Count; i++)
                {
                    if (unlockItems.Count <= i)
                    {
                        // Add new data to SaveData file in case the file is empty or new data is available
                        unlockItems.Add(_ItemsInfo[i].isLocked);
                    }
                }
            }
            // Setting up Hairs Properties to actual Properties from SaveData file  
            for (int i = 0; i < _ItemsInfo.Count; i++)
            {
                _ItemsInfo[i].isLocked = unlockItems[i];
            }
            //Adding Click listeners to btns 
            for (int i = 0; i < _ItemsInfo.Count; i++)
            {
                int Index = i;
                if (_ItemsInfo[i].itemBtn)
                {
                    _ItemsInfo[i].itemBtn.onClick.AddListener(() =>
                    {
                        selectedIndex = Index;
                        SelectItem(Index);
                    });
                }
            }
        }
    }
    #endregion

    #region SetItemIcon
    private void SetItemIcon(List<ItemInfo> refList, Sprite[] btnIcons)
    {
        if (refList != null)
        {
            for (int i = 0; i < refList.Count; i++)
            {
                if (btnIcons.Length > i)
                {
                    if (btnIcons[i] && refList[i].itemIcon)
                    {
                        refList[i].itemIcon.sprite = btnIcons[i];
                    }
                }
            }
        }
    }
    #endregion

    #region SelectItem
    public void SelectItem(int index)
    {
        if (selectedItem == GamePlaySelectedItem.mathapati)
        {
            CheckSelectedItem(mathapatiList, mathapatiSprites, playerElements.mathapatiImage);
        }
        else if (selectedItem == GamePlaySelectedItem.nosering)
        {
            CheckSelectedItem(noseringList, noseringSprites, playerElements.noseringImage);
        }
        else if (selectedItem == GamePlaySelectedItem.bangle)
        {
            CheckSelectedItem(bangleList, bangleSprites, playerElements.bangleImage);
        }
        else if (selectedItem == GamePlaySelectedItem.dress)
        {
            CheckSelectedItem(dressList, dressSprites, playerElements.dressImage);
        }
        else if (selectedItem == GamePlaySelectedItem.earing)
        {
            CheckSelectedItem(earingList, earingSprites, playerElements.earingImage);
        }
        else if (selectedItem == GamePlaySelectedItem.bag)
        {
            CheckSelectedItem(bagList, bagSprites, playerElements.bagImage);
        }
        else if (selectedItem == GamePlaySelectedItem.eyeshade)
        {
            CheckSelectedItem(eyeshadeList, eyeshadeSprites, playerElements.eyeshadeImage);
        }
        else if (selectedItem == GamePlaySelectedItem.necklace)
        {
            CheckSelectedItem(necklaceList, necklaceSprites, playerElements.necklaceImage);
        }
        else if (selectedItem == GamePlaySelectedItem.hair)
        {
            CheckSelectedItem(hairList, hairSprites, playerElements.hairImage);
        }
        else if (selectedItem == GamePlaySelectedItem.lips)
        {
            CheckSelectedItem(lipsList, lipsSprites, playerElements.lipsImage);
        }
        else if (selectedItem == GamePlaySelectedItem.bindi)
        {
            CheckSelectedItem(bindiList, bindiSprites, playerElements.bindiImage);
        }
        else if (selectedItem == GamePlaySelectedItem.mehandi)
        {
            CheckSelectedItem(mehandiList, mehandiSprites, playerElements.mehandiImage);
        }
        else if (selectedItem == GamePlaySelectedItem.blush)
        {
            CheckSelectedItem(blushList, blushSprites, playerElements.blushImage);
        }
        else if (selectedItem == GamePlaySelectedItem.closedEyeshade)
        {
            CheckSelectedItem(closedEyeshadeList, closedEyeshadeSprites, playerElements.closedEyeshadeImage);
        }
        GetItemsInfo();
        totalCoins.text = SaveData.Instance.Coins.ToString();
    }
    #endregion

    #region CheckSelectedItem
    private void CheckSelectedItem(List<ItemInfo> itemInfoList, Sprite[] itemSprites, Image itemImage)
    {

        rewardType = RewardType.selectionItem;
        if (itemInfoList.Count > selectedIndex)
        {
            tempItem = itemInfoList[selectedIndex];
            if (itemInfoList[selectedIndex].isLocked)
            {
                if (itemInfoList[selectedIndex].videoUnlock)
                {
                    CheckVideoStatus();
                }
                else if (itemInfoList[selectedIndex].coinsUnlock)
                {
                    if (SaveData.Instance.Coins >= itemInfoList[selectedIndex].requiredCoins)
                    {
                        itemInfoList[selectedIndex].isLocked = false;
                        SaveData.Instance.Coins -= itemInfoList[selectedIndex].requiredCoins;
                        UnlockSingleItem();
                        if (purchaseSFX) purchaseSFX.Play();
                        SelectItem(selectedIndex);
                    }
                    else
                    {
                        if (uIElements.enoughCoinPopUp)
                            uIElements.enoughCoinPopUp.SetActive(true);
                    }
                }
            }
            else
            {
                
                if (itemSprites.Length > selectedIndex)
                {
                    if (itemSprites[selectedIndex])
                    {
                        if (itemImage)
                        {
                            if (selectedItem == GamePlaySelectedItem.dress)
                            {
                                Isdress = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.bangle)
                            {
                                Isbangle = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.earing)
                            {
                                Isearing = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.necklace)
                            {
                                Isnecklace = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.mathapati)
                            {
                                Ismathapati = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.nosering)
                            {
                                Isnosering = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.bindi)
                            {
                                Isbindi = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.mehandi)
                            {
                                Ismehandi = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.bag)
                            {
                                Isbag = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.hair)
                            {
                                Ishair = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.lips)
                            {
                                Islips = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.blush)
                            {
                                Isblush = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.closedEyeshade)
                            {
                                eyeAnim.enabled = false;
                                StartCoroutine(playEyeAnim());
                                IsclosedEyeshade = true;
                            }
                            else if (selectedItem == GamePlaySelectedItem.eyeshade)
                            {
                                Iseyeshade = true;
                            }
                            if (Isdress == true && Isbangle == true && Isearing == true && Isnecklace == true && Ismathapati == true && Isnosering == true && Isbindi == true && Ismehandi == true && Isbag == true
                            && Ishair == true && Islips == true && Isblush == true && IsclosedEyeshade == true && Iseyeshade == true)
                            {
                                uIElements.nextBtn.SetActive(true);
                            }
                            if (taskPartical) taskPartical.Play();
                            voiceSounds[Random.Range(0, voiceSounds.Length)].Play();
                            itemImage.gameObject.SetActive(false);
                            itemImage.gameObject.SetActive(true);
                            itemImage.sprite = itemSprites[selectedIndex];
                            //ItemStatus((int)selectedItem);
                        }
                    }
                }
                //if (taskPartical) taskPartical.SetActive(false);
                CheckInterstitialAD();
            }
        }
    }
    #endregion

    #region ItemStatus
    //public void ItemStatus(int itemIndex)
    //{
    //    categoryBtn[itemIndex].transform.GetChild(1).GetComponent<Image>().sprite = greenSprite;
    //    categoryBtn[itemIndex].transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
    //    categoryBtn[itemIndex].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = tickSprite;
    //}
    #endregion

    #region GetItemsInfo
    private void GetItemsInfo()
    {
        if (selectedItem == GamePlaySelectedItem.mathapati)
        {
            SetItemsInfo(mathapatiList);
        }
        else if (selectedItem == GamePlaySelectedItem.nosering)
        {
            SetItemsInfo(noseringList);
        }
        else if (selectedItem == GamePlaySelectedItem.bangle)
        {
            SetItemsInfo(bangleList);
        }
        if (selectedItem == GamePlaySelectedItem.dress)
        {
            SetItemsInfo(dressList);
        }
        else if (selectedItem == GamePlaySelectedItem.earing)
        {
            SetItemsInfo(earingList);
        }
        else if (selectedItem == GamePlaySelectedItem.bag)
        {
            SetItemsInfo(bagList);
        }
        else if (selectedItem == GamePlaySelectedItem.eyeshade)
        {
            SetItemsInfo(eyeshadeList);
        }
        else if (selectedItem == GamePlaySelectedItem.necklace)
        {
            SetItemsInfo(necklaceList);
        }
        else if (selectedItem == GamePlaySelectedItem.hair)
        {
            SetItemsInfo(hairList);
        }
        else if (selectedItem == GamePlaySelectedItem.lips)
        {
            SetItemsInfo(lipsList);
        }
        else if (selectedItem == GamePlaySelectedItem.bindi)
        {
            SetItemsInfo(bindiList);
        }
        else if (selectedItem == GamePlaySelectedItem.mehandi)
        {
            SetItemsInfo(mehandiList);
        }
        else if (selectedItem == GamePlaySelectedItem.blush)
        {
            SetItemsInfo(blushList);
        }
        else if (selectedItem == GamePlaySelectedItem.closedEyeshade)
        {
            SetItemsInfo(closedEyeshadeList);
        }
    }
    #endregion

    #region SetItemsInfo
    private void SetItemsInfo(List<ItemInfo> _ItemInfo)
    {
        if (_ItemInfo == null) return;
        for (int i = 0; i < _ItemInfo.Count; i++)
        {
            //_ItemInfo[i].ItemScore.text = itemScore[i].ToString();

            if (_ItemInfo[i].btnBG)
            {
                if (i == selectedIndex)
                {
                    _ItemInfo[i].btnBG.sprite = selectionSelectedSprite;
                }
                else
                {
                    _ItemInfo[i].btnBG.sprite = selectionDefaultSprite;
                }
            }
            if (_ItemInfo[i].isLocked)
            {
                if (_ItemInfo[i].videoUnlock)
                {

                    _ItemInfo[i].videoIcon.SetActive(true);
                    _ItemInfo[i].coinSlot.SetActive(false);

                }
                else if (_ItemInfo[i].coinsUnlock)
                {
                    _ItemInfo[i].coinSlot.SetActive(true);
                    _ItemInfo[i].videoIcon.SetActive(false);
                    if (_ItemInfo[i].unlockCoins)
                    {
                        _ItemInfo[i].unlockCoins.text = _ItemInfo[i].requiredCoins.ToString();
                    }

                }
            }
            else
            {
                if (_ItemInfo[i].videoIcon) _ItemInfo[i].videoIcon.SetActive(false);
                if (_ItemInfo[i].coinSlot) _ItemInfo[i].coinSlot.SetActive(false);
            }

        }
    }
    #endregion

    #region SelectedCatagory
    private void DisableScrollers()
    {
        for (int i = 0; i < categoryBtn.Length; i++)
        {
            categoryBtn[i].transform.GetComponent<Image>().sprite = defaultBtnSprites[i];
        }

        uIElements.mathapatiScroller.SetActive(false);
        uIElements.noseringScroller.SetActive(false);
        uIElements.bangleScroller.SetActive(false);
        uIElements.dressScroller.SetActive(false);
        uIElements.earingScroller.SetActive(false);
        uIElements.bagScroller.SetActive(false);
        uIElements.eyeshadeScroller.SetActive(false);
        uIElements.necklaceScroller.SetActive(false);
        uIElements.hairScroller.SetActive(false);
        uIElements.lipsScroller.SetActive(false);
        uIElements.bindiScroller.SetActive(false);
        uIElements.mehandiScroller.SetActive(false);
        uIElements.blushScroller.SetActive(false);
        uIElements.closedEyeshadeScroller.SetActive(false);
    }
    public void SelectedCatagory(int index)
    {
        DisableScrollers();
        if (categorySFX) categorySFX.Play();
        categoryBtn[index].transform.GetComponent<Image>().sprite = selectedBtnSprites[index];

        if (index == (int)GamePlaySelectedItem.dress)
        {
            //playerCharacterMover.Move(new Vector3(0, 0, 0), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.dress;
            uIElements.dressScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.bangle)
        {
            //playerCharacterMover.Move(new Vector3(0, 0, 0), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.bangle;
            uIElements.bangleScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.earing)
        {
            //playerCharacterMover.Move(new Vector3(20, -400, -1000), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.earing;
            uIElements.earingScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.necklace)
        {
            //playerCharacterMover.Move(new Vector3(20, -400, -1000), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.necklace;
            uIElements.necklaceScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.mathapati)
        {
            //playerCharacterMover.Move(new Vector3(20, -400, -1000), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.mathapati;
            uIElements.mathapatiScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.bag)
        {
            //playerCharacterMover.Move(new Vector3(0, 0, 0), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.bag;
            uIElements.bagScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.nosering)
        {
            //playerCharacterMover.Move(new Vector3(0, 0, 0), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.nosering;
            uIElements.noseringScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.mehandi)
        {
            //playerCharacterMover.Move(new Vector3(0, 0, 0), 0.5f, true, false);
            selectedItem = GamePlaySelectedItem.mehandi;
            uIElements.mehandiScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.hair)
        {
            selectedItem = GamePlaySelectedItem.hair;
            uIElements.hairScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.lips)
        {
            selectedItem = GamePlaySelectedItem.lips;
            uIElements.lipsScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.bindi)
        {
            selectedItem = GamePlaySelectedItem.bindi;
            uIElements.bindiScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.blush)
        {
            selectedItem = GamePlaySelectedItem.blush;
            uIElements.blushScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.closedEyeshade)
        {
            selectedItem = GamePlaySelectedItem.closedEyeshade;
            uIElements.closedEyeshadeScroller.SetActive(true);
        }
        else if (index == (int)GamePlaySelectedItem.eyeshade)
        {
            selectedItem = GamePlaySelectedItem.eyeshade;
            uIElements.eyeshadeScroller.SetActive(true);
        }
        GetItemsInfo();
    }
    #endregion

    #region UnlockSingleItem
    public void UnlockSingleItem()
    {
        if (selectedItem == GamePlaySelectedItem.mathapati)
        {
            SaveData.Instance.GamePlayModeElements.mathapati[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.nosering)
        {
            SaveData.Instance.GamePlayModeElements.nosering[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.bangle)
        {
            SaveData.Instance.GamePlayModeElements.bangle[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.dress)
        {
            SaveData.Instance.GamePlayModeElements.dress[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.earing)
        {
            SaveData.Instance.GamePlayModeElements.earing[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.bag)
        {
            SaveData.Instance.GamePlayModeElements.bag[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.eyeshade)
        {
            SaveData.Instance.GamePlayModeElements.eyeshade[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.necklace)
        {
            SaveData.Instance.GamePlayModeElements.necklace[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.hair)
        {
            SaveData.Instance.GamePlayModeElements.hair[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.lips)
        {
            SaveData.Instance.GamePlayModeElements.lips[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.bindi)
        {
            SaveData.Instance.GamePlayModeElements.bindi[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.mehandi)
        {
            SaveData.Instance.GamePlayModeElements.mehandi[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.blush)
        {
            SaveData.Instance.GamePlayModeElements.blush[selectedIndex] = false;
        }
        else if (selectedItem == GamePlaySelectedItem.closedEyeshade)
        {
            SaveData.Instance.GamePlayModeElements.closedEyeshade[selectedIndex] = false;
        }
        totalCoins.text = SaveData.Instance.Coins.ToString();
        Usman_SaveLoad.SaveProgress();
    }
    #endregion

    #region BtnsTask
    public void Next()
    {
        StartCoroutine(next());
    }
    public void Submitted()
    {
        StartCoroutine(submitlook());
    }
    public void Play(string str)
    {
        uIElements.loadingPanel.SetActive(true);
        StartCoroutine(LoadingScene(str));
    }
    #endregion

    #region IEnumerator
    IEnumerator next()
    {
        uIElements.submitPanel.SetActive(true);
        uIElements.allScroller.SetActive(false);
        uIElements.coinSlot.SetActive(false);
        uIElements.homeBtn.SetActive(false);
        playerCharacterMover.Move(new Vector3(0, -800, -200), 0.7f, true, false);
        yield return new WaitForSeconds(0.68f);
        playerCharacterMover.Move(new Vector3(0, -200, -200), 0.7f, true, false);
        yield return new WaitForSeconds(0.68f);
        playerCharacterMover.Move(new Vector3(0, -592, 0), 0.7f, true, false);
    }

    IEnumerator submitlook()
    {
        uIElements.allScroller.SetActive(false);
        uIElements.coinSlot.SetActive(false);
        yield return new WaitForSeconds(1f);
        oppoElements.characterLeft.SetActive(true);
        oppoElements.characterRight.SetActive(true);
        playerElements.playerScoreBox.SetActive(true);
        oppoElements.leftOppoScoreBox.SetActive(true);
        oppoElements.rightOppoScoreBox.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        LeftOpponentDressing();
        RightOpponentDressing();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Comparing());
    }

    IEnumerator AddCoins(float delay, int coins)
    {
        yield return new WaitForSeconds(delay);
        if (coinsAdder)
        {
            coinsAdder.addCoins = coins;
            coinsAdder.addNow = true;
        }
    }

    IEnumerator AddCoinAnim(int coins)
    {
        yield return null;
        if (coinsAdder)
        {
            coinsAdder.addCoins = coins;
            coinsAdder.addNow = true;
        }
    }

    IEnumerator LoadingScene(string str)
    {
        AsyncOperation asyncLoad;
        asyncLoad = SceneManager.LoadSceneAsync(str);
        asyncLoad.allowSceneActivation = false;
        while (uIElements.fillbar.fillAmount < 1)
        {
            uIElements.fillbar.fillAmount += Time.deltaTime / 3;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator playEyeAnim()
    {
        yield return new WaitForSeconds(1f);
        eyeAnim.enabled = true;
        eyeAnim.Play(0);
    }
    #endregion

    #region GetRewardedCoins
    public void GetRewardedCoins()
    {
        rewardType = RewardType.coins;
        CheckVideoStatus();
    }
    #endregion

    #region CheckVideoStatus
    public void CheckVideoStatus()
    {
        if (MyAdsManager.Instance != null)
        {
            if (MyAdsManager.Instance.IsRewardedAvailable())
            {
                uIElements.enoughCoinPopUp.SetActive(false);
                MyAdsManager.Instance.ShowRewardedVideos();
            }
            else
            {
                uIElements.videoAdNotAvailablePopUp.SetActive(true);
                StartCoroutine(EnableOrDisable(2.2f, uIElements.videoAdNotAvailablePopUp, false));
            }
        }
        else
        {
            uIElements.videoAdNotAvailablePopUp.SetActive(true);
            StartCoroutine(EnableOrDisable(2.2f, uIElements.videoAdNotAvailablePopUp, false));
        }
    }
    #endregion

    #region RewardedVideoCompleted
    public void OnRewardedVideoComplete()
    {
        if (rewardType == RewardType.selectionItem)
        {
            if (tempItem != null) tempItem.isLocked = false;
            UnlockSingleItem();
            canShowInterstitial = false;
            StartCoroutine(AdDelay(45));
            SelectItem(selectedIndex);
        }
        else if (rewardType == RewardType.coins)
        {
            StartCoroutine(AddCoinAnim(300));
            totalCoins.text = SaveData.Instance.Coins.ToString();
            Usman_SaveLoad.SaveProgress();
        }
        GetItemsInfo();
        rewardType = RewardType.none;
    }
    #endregion

    #region ShowInterstitialAD
    private void CheckInterstitialAD()
    {
        if (MyAdsManager.Instance != null)
        {

            if (MyAdsManager.Instance.IsInterstitialAvailable() && canShowInterstitial)
            {
                canShowInterstitial = !canShowInterstitial;
                StartCoroutine(AdDelay(45));
                StartCoroutine(ShowInterstitialAD());
            }
        }
    }

    IEnumerator ShowInterstitialAD()
    {
        if (uIElements.adPanel)
        {
            uIElements.adPanel.SetActive(true);
            yield return new WaitForSeconds(1f);
            waitAdLoadTime.text = "..2";
            yield return new WaitForSeconds(1f);
            waitAdLoadTime.text = ".1";
            yield return new WaitForSeconds(1f);
            waitAdLoadTime.text = "0";
            yield return new WaitForSeconds(0.5f);
            uIElements.adPanel.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            waitAdLoadTime.text = "...3";
        }
        ShowInterstitial();
    }
    IEnumerator AdDelay(float _Delay)
    {
        yield return new WaitForSeconds(_Delay);
        canShowInterstitial = !canShowInterstitial;
    }
    #endregion

    #region EnableOrDisable
    IEnumerator EnableOrDisable(float _Delay, GameObject activateObject, bool isTrue)
    {
        yield return new WaitForSecondsRealtime(_Delay);
        activateObject.SetActive(isTrue);
    }
    IEnumerator EnableAnim(float _Delay, Animator activateObject)
    {
        yield return new WaitForSecondsRealtime(_Delay);
        activateObject.enabled = true;
    }
    #endregion

    #region LeftOpponentDressing
    private void LeftOpponentDressing()
    {
        int randomIndex = 0;
        #region body
        randomIndex = Random.Range(0, characterBodys.Length);
        if (characterBodys[randomIndex] && oppoElements.leftCharacterBody)
        {
            oppoElements.leftCharacterBody.gameObject.SetActive(true);
            oppoElements.leftCharacterBody.sprite = characterBodys[randomIndex];
            oppoElements.leftOppoclosedEyeshadeImage.sprite = closeEyeSprites[randomIndex];
        }
        #endregion
        
        #region face
        //randomIndex = Random.Range(0, characterFaces.Length);
        if (characterFaces[randomIndex] && oppoElements.leftCharacterFace)
        {
            oppoElements.leftCharacterFace.gameObject.SetActive(true);
            oppoElements.leftCharacterFace.sprite = characterFaces[randomIndex];
        }
        #endregion

        #region dress
        randomIndex = Random.Range(0, dressList.Count);
        if (dressList[randomIndex] && oppoElements.leftOppodressImage)
        {
            oppoElements.leftOppodressImage.gameObject.SetActive(true);
            oppoElements.leftOppodressImage.sprite = dressSprites[randomIndex];
        }
        #endregion

        #region mehandi
        randomIndex = Random.Range(0, mehandiList.Count);
        if (mehandiList[randomIndex] && oppoElements.leftOppomehandiImage)
        {
            oppoElements.leftOppomehandiImage.gameObject.SetActive(true);
            oppoElements.leftOppomehandiImage.sprite = mehandiSprites[randomIndex];
        }
        #endregion

        #region eyeshade
        randomIndex = Random.Range(0, eyeshadeList.Count);
        if (eyeshadeList[randomIndex] && oppoElements.leftOppoeyeshadeImage)
        {
            oppoElements.leftOppoeyeshadeImage.gameObject.SetActive(true);
            oppoElements.leftOppoeyeshadeImage.sprite = eyeshadeSprites[randomIndex];
        }
        #endregion

        #region hair
        randomIndex = Random.Range(0, hairList.Count);
        if (hairList[randomIndex] && oppoElements.leftOppohairImage)
        {
            oppoElements.leftOppohairImage.gameObject.SetActive(true);
            oppoElements.leftOppohairImage.sprite = hairSprites[randomIndex];
        }
        #endregion

        #region lips
        randomIndex = Random.Range(0, lipsList.Count);
        if (lipsList[randomIndex] && oppoElements.leftOppolipsImage)
        {
            oppoElements.leftOppolipsImage.gameObject.SetActive(true);
            oppoElements.leftOppolipsImage.sprite = lipsSprites[randomIndex];
        }
        #endregion

        #region bindi
        randomIndex = Random.Range(0, bindiList.Count);
        if (bindiList[randomIndex] && oppoElements.leftOppobindiImage)
        {
            oppoElements.leftOppobindiImage.gameObject.SetActive(true);
            oppoElements.leftOppobindiImage.sprite = bindiSprites[randomIndex];
        }
        #endregion

        #region blush
        randomIndex = Random.Range(0, blushList.Count);
        if (blushList[randomIndex] && oppoElements.leftOppoblushImage)
        {
            oppoElements.leftOppoblushImage.gameObject.SetActive(true);
            oppoElements.leftOppoblushImage.sprite = blushSprites[randomIndex];
        }
        #endregion

        #region closedEyeshade
        randomIndex = Random.Range(0, closedEyeshadeList.Count);
        if (closedEyeshadeList[randomIndex] && oppoElements.leftOppoclosedEyeshadeImage)
        {
            oppoElements.leftOppoclosedEyeshadeImage.gameObject.SetActive(true);
            oppoElements.leftOppoclosedEyeshadeImage.sprite = closedEyeshadeSprites[randomIndex];
        }
        #endregion

        #region bag
        randomIndex = Random.Range(0, bagList.Count);
        if (bagList[randomIndex] && oppoElements.leftOppobagImage)
        {
            oppoElements.leftOppobagImage.gameObject.SetActive(true);
            oppoElements.leftOppobagImage.sprite = bagSprites[randomIndex];
        }
        #endregion

        #region earing
        randomIndex = Random.Range(0, earingList.Count);
        if (earingList[randomIndex] && oppoElements.leftOppoearingImage)
        {
            oppoElements.leftOppoearingImage.gameObject.SetActive(true);
            oppoElements.leftOppoearingImage.sprite = earingSprites[randomIndex];
        }
        #endregion

        #region mathapati
        randomIndex = Random.Range(0, mathapatiList.Count);
        if (mathapatiList[randomIndex] && oppoElements.leftOppomathapatiImage)
        {
            oppoElements.leftOppomathapatiImage.gameObject.SetActive(true);
            oppoElements.leftOppomathapatiImage.sprite = mathapatiSprites[randomIndex];
        }
        #endregion

        #region bangle
        randomIndex = Random.Range(0, bangleList.Count);
        if (bangleList[randomIndex] && oppoElements.leftOppobangleImage)
        {
            oppoElements.leftOppobangleImage.gameObject.SetActive(true);
            oppoElements.leftOppobangleImage.sprite = bangleSprites[randomIndex];
        }
        #endregion

        #region nosering
        randomIndex = Random.Range(0, noseringList.Count);
        if (noseringList[randomIndex] && oppoElements.leftOpponoseringImage)
        {
            oppoElements.leftOpponoseringImage.gameObject.SetActive(true);
            oppoElements.leftOpponoseringImage.sprite = noseringSprites[randomIndex];
        }
        #endregion

        #region necklace
        randomIndex = Random.Range(0, necklaceList.Count);
        if (necklaceList[randomIndex] && oppoElements.leftOpponecklaceImage)
        {
            oppoElements.leftOpponecklaceImage.gameObject.SetActive(true);
            oppoElements.leftOpponecklaceImage.sprite = necklaceSprites[randomIndex];
        }
        #endregion
    }
    #endregion

    #region RightOpponentDressing
    private void RightOpponentDressing()
    {
        int randomIndex = 0;
        #region body
        randomIndex = Random.Range(0, characterBodys.Length);
        if (characterBodys[randomIndex] && oppoElements.rightCharacterBody)
        {
            oppoElements.rightCharacterBody.gameObject.SetActive(true);
            oppoElements.rightCharacterBody.sprite = characterBodys[randomIndex];
            oppoElements.rightOppoclosedEyeshadeImage.sprite = closeEyeSprites[randomIndex];
        }
        #endregion

        #region face
        //randomIndex = Random.Range(0, characterFaces.Length);
        if (characterFaces[randomIndex] && oppoElements.rightCharacterFace)
        {
            oppoElements.rightCharacterFace.gameObject.SetActive(true);
            oppoElements.rightCharacterFace.sprite = characterFaces[randomIndex];
        }
        #endregion

        #region dress
        randomIndex = Random.Range(0, dressList.Count);
        if (dressList[randomIndex] && oppoElements.rightOppodressImage)
        {
            oppoElements.rightOppodressImage.gameObject.SetActive(true);
            oppoElements.rightOppodressImage.sprite = dressSprites[randomIndex];
        }
        #endregion

        #region mehandi
        randomIndex = Random.Range(0, mehandiList.Count);
        if (mehandiList[randomIndex] && oppoElements.rightOppomehandiImage)
        {
            oppoElements.rightOppomehandiImage.gameObject.SetActive(true);
            oppoElements.rightOppomehandiImage.sprite = mehandiSprites[randomIndex];
        }
        #endregion

        #region eyeshade
        randomIndex = Random.Range(0, eyeshadeList.Count);
        if (eyeshadeList[randomIndex] && oppoElements.rightOppoeyeshadeImage)
        {
            oppoElements.rightOppoeyeshadeImage.gameObject.SetActive(true);
            oppoElements.rightOppoeyeshadeImage.sprite = eyeshadeSprites[randomIndex];
        }
        #endregion

        #region hair
        randomIndex = Random.Range(0, hairList.Count);
        if (hairList[randomIndex] && oppoElements.rightOppohairImage)
        {
            oppoElements.rightOppohairImage.gameObject.SetActive(true);
            oppoElements.rightOppohairImage.sprite = hairSprites[randomIndex];
        }
        #endregion

        #region lips
        randomIndex = Random.Range(0, lipsList.Count);
        if (lipsList[randomIndex] && oppoElements.rightOppolipsImage)
        {
            oppoElements.rightOppolipsImage.gameObject.SetActive(true);
            oppoElements.rightOppolipsImage.sprite = lipsSprites[randomIndex];
        }
        #endregion

        #region bindi
        randomIndex = Random.Range(0, bindiList.Count);
        if (bindiList[randomIndex] && oppoElements.rightOppobindiImage)
        {
            oppoElements.rightOppobindiImage.gameObject.SetActive(true);
            oppoElements.rightOppobindiImage.sprite = bindiSprites[randomIndex];
        }
        #endregion

        #region blush
        randomIndex = Random.Range(0, blushList.Count);
        if (blushList[randomIndex] && oppoElements.rightOppoblushImage)
        {
            oppoElements.rightOppoblushImage.gameObject.SetActive(true);
            oppoElements.rightOppoblushImage.sprite = blushSprites[randomIndex];
        }
        #endregion

        #region eybrow
        randomIndex = Random.Range(0, closedEyeshadeList.Count);
        if (closedEyeshadeList[randomIndex] && oppoElements.rightOppoclosedEyeshadeImage)
        {
            oppoElements.rightOppoclosedEyeshadeImage.gameObject.SetActive(true);
            oppoElements.rightOppoclosedEyeshadeImage.sprite = closedEyeshadeSprites[randomIndex];
        }
        #endregion

        #region bag
        randomIndex = Random.Range(0, bagList.Count);
        if (bagList[randomIndex] && oppoElements.rightOppobagImage)
        {
            oppoElements.rightOppobagImage.gameObject.SetActive(true);
            oppoElements.rightOppobagImage.sprite = bagSprites[randomIndex];
        }
        #endregion

        #region earing
        randomIndex = Random.Range(0, earingList.Count);
        if (earingList[randomIndex] && oppoElements.rightOppoearingImage)
        {
            oppoElements.rightOppoearingImage.gameObject.SetActive(true);
            oppoElements.rightOppoearingImage.sprite = earingSprites[randomIndex];
        }
        #endregion

        #region mathapati
        randomIndex = Random.Range(0, mathapatiList.Count);
        if (mathapatiList[randomIndex] && oppoElements.rightOppomathapatiImage)
        {
            oppoElements.rightOppomathapatiImage.gameObject.SetActive(true);
            oppoElements.rightOppomathapatiImage.sprite = mathapatiSprites[randomIndex];
        }
        #endregion

        #region bangle
        randomIndex = Random.Range(0, bangleList.Count);
        if (bangleList[randomIndex] && oppoElements.rightOppobangleImage)
        {
            oppoElements.rightOppobangleImage.gameObject.SetActive(true);
            oppoElements.rightOppobangleImage.sprite = bangleSprites[randomIndex];
        }
        #endregion

        #region nosering
        randomIndex = Random.Range(0, noseringList.Count);
        if (noseringList[randomIndex] && oppoElements.rightOpponoseringImage)
        {
            oppoElements.rightOpponoseringImage.gameObject.SetActive(true);
            oppoElements.rightOpponoseringImage.sprite = noseringSprites[randomIndex];
        }
        #endregion

        #region necklace
        randomIndex = Random.Range(0, necklaceList.Count);
        if (necklaceList[randomIndex] && oppoElements.rightOpponecklaceImage)
        {
            oppoElements.rightOpponecklaceImage.gameObject.SetActive(true);
            oppoElements.rightOpponecklaceImage.sprite = necklaceSprites[randomIndex];
        }
        #endregion
    }
    #endregion

    #region Comparing
    IEnumerator Comparing()
    {
        yield return new WaitForSeconds(0.5f);
        playerCharacterMover.Move(new Vector3(0, -584, 300), 0.7f, true, false);
        yield return new WaitForSeconds(0.3f);
        oppoLeftCharacterMover.Move(new Vector3(550, -400, 800), 0.7f, true, false);
        oppoRightCharacterMover.Move(new Vector3(-550, -400, 800), 0.7f, true, false);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Random.Range(10, 25); i++)
        {
            if (scoreSFX) scoreSFX.Play();
            playerElements.txtPlayerScore.gameObject.SetActive(false);
            playerElements.txtPlayerScore.gameObject.SetActive(true);
            playerElements.txtPlayerScore.text = Random.Range(99, 200).ToString();
            yield return new WaitForSeconds(0.1f);
            if (scoreSFX) scoreSFX.Stop();
        }
        for (int i = 0; i < Random.Range(10, 25); i++)
        {
            if (scoreSFX) scoreSFX.Play();
            oppoElements.txtRightOppoScore.gameObject.SetActive(false);
            oppoElements.txtRightOppoScore.gameObject.SetActive(true);
            oppoElements.txtRightOppoScore.text = Random.Range(00, 100).ToString();
            yield return new WaitForSeconds(0.1f);
            if (scoreSFX) scoreSFX.Stop();
        }
        for (int i = 0; i < Random.Range(10, 25); i++)
        {
            if (scoreSFX) scoreSFX.Play();
            oppoElements.txtLeftOppoScore.gameObject.SetActive(false);
            oppoElements.txtLeftOppoScore.gameObject.SetActive(true);
            oppoElements.txtLeftOppoScore.text = Random.Range(00, 100).ToString();
            yield return new WaitForSeconds(0.1f);
            if (scoreSFX) scoreSFX.Stop();
        }
        yield return new WaitForSeconds(2f);
        oppoLeftCharacterMover.Move(new Vector3(2000, -400, 800), 0.7f, true, false);
        oppoRightCharacterMover.Move(new Vector3(-2000, -400, 800), 0.7f, true, false);
        yield return new WaitForSeconds(0.5f);
        playerElements.playerScoreBox.SetActive(false);
        yield return new WaitForSeconds(1f);
        playerCharacterMover.Move(new Vector3(0, -538, 0), 0.7f, true, false);
        yield return new WaitForSeconds(0.5f);
        uIElements.coinSlot.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(AddCoinAnim(300));
        yield return new WaitForSeconds(0.5f);
        if (finalPartical) finalPartical.SetActive(true);
        yield return new WaitForSeconds(1f);
        if(SaveData.Instance.isRate == false)
        {
            uIElements.rateUsBtn.SetActive(true);
        }
        else
        {
            uIElements.rateUsBtn.SetActive(false);
        }
        uIElements.lastBtns.SetActive(true);

    }
    #endregion

    #region TakeScreenShot
    Texture2D _Taxture;
    public void TakeScreenShot()
    {
        uIElements.screenShotImage.transform.parent.localScale = Vector3.one;
        StartCoroutine(takeScreenShot());
    }
    IEnumerator takeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D _Texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        _Texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _Texture.Apply();
        _Texture.LoadImage(_Texture.EncodeToPNG());
        Sprite sprite = Sprite.Create(_Texture, new Rect(0, 0, _Texture.width, _Texture.height), new Vector2(_Texture.width / 2, _Texture.height / 2));
        if (uIElements.screenShotImage)
        {
            uIElements.screenShotImage.sprite = sprite;
            uIElements.screenShotPanel.SetActive(true);
            //DownloadImage();
        }
        _Taxture = _Texture;
        Invoke("DownloadImage", 0.8f);
    }
    public void DownloadImage()
    {
        string picturName = "ScreenShot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        NativeGallery.SaveImageToGallery(_Taxture, "My Pictures", picturName);
        Invoke("PictureSaved", 0.8f);

    }
    private void PictureSaved()
    {
        uIElements.screenShotPanel.SetActive(false);
        uIElements.submitPanel.SetActive(true);
        Destroy(_Taxture);
        Invoke("Submitted", 0.7f);
    }
    #endregion
}
