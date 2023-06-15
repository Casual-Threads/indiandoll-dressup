using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

[System.Serializable]
public enum UbtanActionTrigger
{
    Mehndi, Water, Cleaning
}

[System.Serializable]
public enum UbtanSelectedItem
{
    jewellery, gajra
}

[System.Serializable]
public class UbtanElenemts
{
    public GameObject jewelleryObject;
    public GameObject gajraObject;
    
}

    [System.Serializable]
public class UbtanPlayerElenemts
{
    [Header("Player Character")]
    public GameObject character;
    [Header("Player Images")]
    public Image jewelleryImage;
    public Image gajraImage, eyes/*Open, eyesClose*/;

}
public class Ubtan : MonoBehaviour
{
    private static Ubtan instance;
    public static Ubtan Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Ubtan();
            }
            return instance;
        }
    }
    public UbtanActionTrigger action;
    public UbtanSelectedItem selectedItem;
    public UbtanElenemts uIElements;
    [HideLabel]
    [FoldoutGroup("Player Elements")]
    public UbtanPlayerElenemts playerElements;
    [Header("Dragable Items")]
    public GameObject mehndiPot;
    public GameObject waterJar, cleaner;
    [Header("Ubtan Items")]
    public GameObject yellowLayer;
    public GameObject waterDrops;
    public Image[] waterArray;
    public Sprite[] jewellerySprites;
    public Sprite[] jewelleryIconSprites;
    public Sprite[] gajraSprites;
    public Sprite[] eyeSprites;
    [Header("Item List")]
    private List<ItemInfo> jewelleryList = new List<ItemInfo>();
    private List<ItemInfo> gajraList = new List<ItemInfo>();
    private int selectedIndex;
    [Header("ItemInfo Variable")]
    private ItemInfo tempItem;

    public ParticleSystem taskParticle;
    public MRS_Manager nextBtn;
    [Header("Ubtan AudioSource")]
    public AudioSource itemChangeSFX;
    public AudioSource removerSFX;
    //public AudioSource clappingSFX;

    // Start is called before the first frame update
    #region Start Function
    void Start()
    {
        SetInitialValues();
        action = UbtanActionTrigger.Mehndi;
        GetItemsInfo();
    }
    public void ShowInterstitial()
    {
        //if (MyAdsManager.instance)
        //{
        //    MyAdsManager.instance.ShowInterstitialAds();
        //}
    }
    #endregion

    #region SetInitialValues
    private void SetInitialValues()
    {
        #region Initialing jewellery
        if (uIElements.jewelleryObject)
        {
            var jewelleryInfo = uIElements.jewelleryObject.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < jewelleryInfo.Length; i++)
            {
                jewelleryList.Add(jewelleryInfo[i]);
            }
        }
        SetupItemData(jewelleryList);
        SetItemIcon(jewelleryList, jewelleryIconSprites);
        #endregion

        #region Initialing gajra
        if (uIElements.gajraObject)
        {
            var gajraInfo = uIElements.gajraObject.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < gajraInfo.Length; i++)
            {
                gajraList.Add(gajraInfo[i]);
            }
        }
        SetupItemData(gajraList);
        SetItemIcon(gajraList, gajraSprites);
        #endregion
    }
    #endregion

    #region SetupItemData
    private void SetupItemData(List<ItemInfo> _ItemsInfo)
    {
        if (_ItemsInfo.Count > 0)
        {
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
        if (selectedItem == UbtanSelectedItem.jewellery)
        {
            CheckSelectedItem(jewelleryList, jewellerySprites, playerElements.jewelleryImage);
        }
        else if (selectedItem == UbtanSelectedItem.gajra)
        {
            CheckSelectedItem(gajraList, gajraSprites, playerElements.gajraImage);
        }
    }
    #endregion

    #region CheckSelectedItem
    private void CheckSelectedItem(List<ItemInfo> itemInfoList, Sprite[] itemSprites, Image itemImage)
    {
        //rewardType = RewardType.selectionItem;
        if (itemInfoList.Count > selectedIndex)
        {
            tempItem = itemInfoList[selectedIndex];

            if (itemSprites.Length > selectedIndex)
            {
                if (itemSprites[selectedIndex])
                {
                    if (itemImage)
                    {
                        if (itemChangeSFX) itemChangeSFX.Play();
                        itemImage.gameObject.SetActive(false);
                        itemImage.gameObject.SetActive(true);
                        itemImage.sprite = itemSprites[selectedIndex];
                    }
                }
            }
            //CheckInterstitialAD();
        }
    }
    #endregion

    #region GetItemsInfo
    private void GetItemsInfo()
    {
        if (selectedItem == UbtanSelectedItem.jewellery)
        {
            SetItemsInfo(jewelleryList);
        }
        else if (selectedItem == UbtanSelectedItem.gajra)
        {
            SetItemsInfo(gajraList);
        }
    }
    #endregion

    #region SetItemsInfo
    private void SetItemsInfo(List<ItemInfo> _ItemInfo)
    {
        if (_ItemInfo == null) return;
    }
    #endregion

    #region NextButtonMovement
    public void NextTask()
    {
        if (selectedItem == UbtanSelectedItem.gajra)
        {
            nextBtn.Move(new Vector3(800, -138, 0), 0.5f, true, false);
            selectedItem = UbtanSelectedItem.jewellery;
            uIElements.gajraObject.SetActive(false);
            uIElements.jewelleryObject.SetActive(true);
        }
    }
    #endregion

    #region TaskDone
    public void TaskDone()
    {
        if (action == UbtanActionTrigger.Mehndi)
        {
            action = UbtanActionTrigger.Water;
            waterJar.SetActive(true);
        }
        else if(action == UbtanActionTrigger.Water)
        {
            action = UbtanActionTrigger.Cleaning;
            cleaner.SetActive(true);
        }
        else if(action == UbtanActionTrigger.Cleaning)
        {
            selectedItem = UbtanSelectedItem.gajra;
            uIElements.gajraObject.SetActive(true);
            nextBtn.gameObject.SetActive(true);
        }
    }
    #endregion

    #region Coroutines
    IEnumerator ObjectActivation(GameObject Obj, float _Delay, bool activateNow)
    {
        yield return new WaitForSeconds(_Delay);
        if (Obj) Obj.SetActive(activateNow);
    }
    #endregion
}
