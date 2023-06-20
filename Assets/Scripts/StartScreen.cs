using System.Collections;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class StartScreenProperties
{
    public Image fillBar;
    public Scenes nextScene;
    [Range(3, 10)]
    public float waitTime;
}

public class StartScreen : MonoBehaviour
{
    [FoldoutGroup("Splash Properties")]
    [HideLabel]
    //public GameObject[] cpIcons;
    public StartScreenProperties uIElements;
    void Start()
    {
        //if (GAManager.Instance != null)
        //{
        //    GAManager.Instance.LogDesignEvent("Splash:Start");
        //}
        //CrossPromotionManager.onCpLoadedEvent += EnableCP;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Time.timeScale = 1;
        AudioListener.pause = false;
        StartCoroutine(LoadNextScene());
    }

    //public void EnableCP(int index)
    //{
    //    cpIcons[index].SetActive(true);
    //}

    IEnumerator LoadNextScene()
    {

        AsyncOperation asyncLoad;
        asyncLoad = SceneManager.LoadSceneAsync(uIElements.nextScene.ToString());
        //if (GAManager.Instance != null)
        //{
        //    GAManager.Instance.LogDesignEvent("Splash:MainMenu");
        //}
        asyncLoad.allowSceneActivation = false;
        while (uIElements.fillBar.fillAmount < 1)
        {
            uIElements.fillBar.fillAmount += Time.deltaTime / uIElements.waitTime;
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }

    //private void OnDestroy()
    //{
    //    CrossPromotionManager.onCpLoadedEvent -= EnableCP;
    //}
}
