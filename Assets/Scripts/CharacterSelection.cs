using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [Header("Arrays")]
    public ItemInfo[] itemInfo;
    public MRS_Manager[] characters;
    public Button nextBtn,BackBtn;
    public GameObject loadingPanel;
    public GameObject videoAdNotAvailablePopUp;
    public Image fillBar;
    private int counter = 0;
    private int selectedIndex;
    //public AudioSource itemSelectSFX;
    [Header("ItemInfo Variable")]
    private ItemInfo tempItem;
    [Header("Bool Variable")]
    private bool canShowInterstitial;

    private enum RewardType
    {
        none,selectionItem
    }
    private RewardType rewardType;

    private void Start()
    {
        Usman_SaveLoad.LoadProgress();
        SetInitialProps();
        GetItemsInfo();
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

    public void ShowInterstitial()
    {
        if (MyAdsManager.instance)
        {
            MyAdsManager.instance.ShowInterstitialAds();
        }
    }

    #region SetInitialProps
    private void SetInitialProps()
    {
        #region Initialing 
        if (SaveData.Instance.ModeProps.Count < itemInfo.Length)
        {
            for (int i = 0; i < itemInfo.Length; i++)
            {
                if (SaveData.Instance.ModeProps.Count <= i)
                {
                    // Add new data to SaveData file in case the file is empty or new data is available
                    Modesprops modeProps = new Modesprops();
                    modeProps.isLocked = itemInfo[i].isLocked;
                    SaveData.Instance.ModeProps.Add(modeProps);
                }
            }
        }
        // Setting up Dresses Properties to actual Properties from SaveData file  
        for (int i = 0; i < itemInfo.Length; i++)
        {
            itemInfo[i].isLocked = SaveData.Instance.ModeProps[i].isLocked;
        }
        //Adding Click listeners to btns 
        for (int i = 0; i < itemInfo.Length; i++)
        {
            int Index = i;
            if (itemInfo[i].itemBtn)
            {
                itemInfo[i].itemBtn.onClick.AddListener(() =>
                {
                    selectedIndex = Index;
                    SelectItem(Index);
                });
            }
        }
        #endregion
        Usman_SaveLoad.SaveProgress();
    }
    #endregion

    #region MoveFunction
    public void Movenext(bool moveNext)
    {
        nextBtn.interactable = true;
        BackBtn.interactable = true;
        BackBtn.GetComponent<ScalePingPong>().enabled = true;
        nextBtn.GetComponent<ScalePingPong>().enabled = true;
        if (moveNext)
        {
            if (counter < characters.Length - 1)
            {
                characters[counter].Move(new Vector3(-1800, 545, 0), 0.7f, true, false);
                characters[counter + 1].Move(new Vector3(0, 545, 0), 0.7f, true, false);
                counter++;
                if (counter == characters.Length - 1)
                {
                    nextBtn.interactable = false;
                    nextBtn.GetComponent<ScalePingPong>().enabled = false;
                }
            }
            else
            {
                nextBtn.interactable = false;
                nextBtn.GetComponent<ScalePingPong>().enabled = false;
            }
        }
        else
        {
            if (counter > 0)
            {
                characters[counter].Move(new Vector3(1800, 545, 0), 0.7f, true, false);
                characters[counter - 1].Move(new Vector3(0, 545, 0), 0.7f, true, false);
                if (counter == 1)
                {
                    BackBtn.interactable = false;
                    BackBtn.GetComponent<ScalePingPong>().enabled = false;
                }
                counter--;
            }
            else
            {
                BackBtn.interactable = false;
                BackBtn.GetComponent<ScalePingPong>().enabled = false;

            }
        }
        SaveData.Instance.selectedCharacter = counter;
    }
    #endregion

    #region GetItemsInfo
    private void GetItemsInfo()
    {
        for (int i = 0; i < itemInfo.Length; i++)
        {
            if (itemInfo[i].isLocked)
            {
                if (itemInfo[i].videoIcon) itemInfo[i].videoIcon.SetActive(true);
            }
            else
            {
                if (itemInfo[i].videoIcon) itemInfo[i].videoIcon.SetActive(false);
            }
        }
    }
    #endregion

    #region SelectItem
    private void SelectItem(int selectedIndex)
    {
        rewardType = RewardType.selectionItem;
        if (itemInfo[selectedIndex].isLocked)
        {
            if (itemInfo[selectedIndex].videoUnlock)
            {
                CheckVideoStatus();
            }
        }
        GetItemsInfo();
    }
    #endregion

    #region CheckVideoStatus
    public void CheckVideoStatus()
    {
        if (MyAdsManager.Instance != null)
        {
            if (MyAdsManager.Instance.IsRewardedAvailable())
            {
                MyAdsManager.Instance.ShowRewardedVideos();
            }
            else
            {
                videoAdNotAvailablePopUp.SetActive(true);
                StartCoroutine(EnableOrDisable(2.2f, videoAdNotAvailablePopUp, false));
            }
        }
        else
        {
            videoAdNotAvailablePopUp.SetActive(true);
            StartCoroutine(EnableOrDisable(2.2f, videoAdNotAvailablePopUp, false));
        }
    }
    #endregion

    #region RewardedVideoCompleted
    public void OnRewardedVideoComplete()
    {
        if (rewardType == RewardType.selectionItem)
        {
            if (tempItem != null) tempItem.isLocked = false;
            itemInfo[selectedIndex].isLocked = false;
            SaveData.Instance.ModeProps[selectedIndex].isLocked = false;
            SelectItem(selectedIndex);
        }
        GetItemsInfo();
        rewardType = RewardType.none;
    }
    #endregion

    IEnumerator EnableOrDisable(float _Delay, GameObject activateObject, bool isTrue)
    {
        yield return new WaitForSecondsRealtime(_Delay);
        activateObject.SetActive(isTrue);
    }

    public void Play(string modeName)
    {
        StartCoroutine(LoadingPanel(modeName));
    }
    IEnumerator LoadingPanel(string mode)
    {

        AsyncOperation asyncLoad;
        asyncLoad = SceneManager.LoadSceneAsync(mode);
        asyncLoad.allowSceneActivation = false;
        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += Time.deltaTime / 3;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

}
