using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum SpaActionTrigger
{
   none, PimplePoper, WormPoper, Serum, FaceWash, Foam, Mask, Remover
}

public class Spa : MonoBehaviour
{
    private static Spa instance;
    public static Spa Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Spa();
            }
            return instance;
        }
    }

    public SpaActionTrigger action;
    [Header("Dragable Items")]
    public GameObject pimplePoper;
    public GameObject wormPoper, serumBottel, facewashBottel, handAnim, remover;
    [Header("Spa Items")]
    public GameObject pimples;
    public GameObject worms, serums, facewash, foam, dust;
    [Header("Spa Arrays")]
    public Image[] foamArray;
    public Sprite [] lipsSprites;
    //public Sprite[] eyeSprites;
    public Sprite[] closeEyeSprites;
    public Image characterBody;
    public Image characterFace;
    public Sprite[] characterBodys;
    public Sprite[] characterFaces;
    public Image LipsImage;
    public Image eyesImage;
    public Image closeEyesImage;
    public Animator eyesAnimator;
    public MRS_Manager nextBtn;

    public ParticleSystem taskParticle;
    public GameObject loadingPanel;
    public Image fillBar;
    [Header("SPA AudioSource")]
    public AudioSource ohNoSFX;
    public AudioSource facewashSFX;
    public AudioSource removerSFX;
    public AudioSource clappingSFX;

    // Start is called before the first frame update
    #region Start Function
    void Start()
    {
        characterBody.sprite = characterBodys[SaveData.Instance.selectedCharacter];
        characterFace.sprite = characterFaces[SaveData.Instance.selectedCharacter];
        //eyesImage.sprite = closeEyeSprites[SaveData.Instance.selectedCharacter];
        closeEyesImage.sprite = closeEyeSprites[SaveData.Instance.selectedCharacter];
        action = SpaActionTrigger.PimplePoper;
        //StartCoroutine(AdDelay(60));
    }
    public void ShowInterstitial()
    {
        if (MyAdsManager.instance)
        {
            MyAdsManager.instance.ShowInterstitialAds();
        }
    }
    #endregion

    #region NextButtonMovement
    public void NextTask()
    {
            if (action == SpaActionTrigger.PimplePoper)
            {
                nextBtn.Move(new Vector3(800, -244, 0), 0.3f, true, false);
                action = SpaActionTrigger.WormPoper;
                pimplePoper.SetActive(false);
                pimples.SetActive(false);
                worms.SetActive(true);
                wormPoper.SetActive(true);
            }
            else if (action == SpaActionTrigger.WormPoper)
            {
                //ShowInterstitial();
                nextBtn.Move(new Vector3(800, -244, 0), 0.5f, true, false);
                action = SpaActionTrigger.Serum;
                worms.SetActive(false);
                wormPoper.SetActive(false);
                serums.SetActive(true);
                serumBottel.SetActive(true);
            }
            else if (action == SpaActionTrigger.Serum)
            {
                ShowInterstitial();
                nextBtn.Move(new Vector3(800, -244, 0), 0.5f, true, false);
                action = SpaActionTrigger.FaceWash;
                serums.SetActive(false);
                serumBottel.SetActive(false);
                facewash.SetActive(true);
                facewashBottel.SetActive(true);
            }
            else if (action == SpaActionTrigger.FaceWash)
            {
                
                nextBtn.Move(new Vector3(800, -244, 0), 0.5f, true, false);
                action = SpaActionTrigger.Foam;
                facewashBottel.SetActive(false);
                foam.SetActive(true);
                handAnim.SetActive(true);
            }
            else if (action == SpaActionTrigger.Foam)
            {
                nextBtn.Move(new Vector3(800, -244, 0), 0.5f, true, false);
                action = SpaActionTrigger.Remover;
                facewash.SetActive(false);
                handAnim.SetActive(false);
                remover.SetActive(true);
            }
            else if (action == SpaActionTrigger.FaceWash)
            {
                facewashBottel.SetActive(false);
                nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);
            }
            else if (action == SpaActionTrigger.none)
            {
                remover.SetActive(false);
                nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);
                ShowInterstitial();
                play();
            }
    }
    #endregion

    #region LoadScene
    public void play()
    {
        loadingPanel.SetActive(true);
        StartCoroutine(LoadingScene());
    }
    #endregion

    IEnumerator LoadingScene()
    {
        yield return new WaitForSeconds(1f);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Ubtan");
        asyncLoad.allowSceneActivation = false;
        while (fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += Time.deltaTime / 3;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

    #region TaskDone
    public void TaskDone()
    {
        if (action == SpaActionTrigger.PimplePoper)
        {
            nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);
        }
        else if (action == SpaActionTrigger.WormPoper)
        {
            nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);
        }
        else if (action == SpaActionTrigger.Serum)
        {
            nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);
            serums.SetActive(false);
            serumBottel.SetActive(false);
        }
        else if (action == SpaActionTrigger.FaceWash)
        {
            nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);
        }
        else if (action == SpaActionTrigger.Foam)
        {
            nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);

        }
        else if (action == SpaActionTrigger.Remover)
        {
            if (clappingSFX) clappingSFX.Play();
            nextBtn.Move(new Vector3(530, -244, 0), 0.5f, true, false);
            action = SpaActionTrigger.none;
            remover.SetActive(false);
            foam.SetActive(false);
            
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
