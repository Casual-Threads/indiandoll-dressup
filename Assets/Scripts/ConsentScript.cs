using UnityEngine.SceneManagement;
using UnityEngine;

public class ConsentScript : MonoBehaviour
{
	public string PrivacyPolicyLink;
	//public string TermConditionsLink;
	public string StartScreen = "StartScreen";

	public GameObject dialog;
	public void Start()
	{
		if (!PlayerPrefs.HasKey("IsFirstTime"))
		{
			PlayerPrefs.SetInt("IsFirstTime", 0);
		}
		if (PlayerPrefs.GetInt("IsFirstTime") == 0)
		{
			dialog.SetActive(true);
		}
		else
		{
			No();
		}
	}

	public void PrivacyPolicy()
	{
		Application.OpenURL(PrivacyPolicyLink);
	}

	//public void openTermPolicy()
	//{
	//	Application.OpenURL(TermConditionsLink);
	//}

	public void Accept()
	{

		PlayerPrefs.SetInt("IsFirstTime", 1);
		PlayerPrefs.Save();
		SceneManager.LoadScene(StartScreen);
		dialog.SetActive(false);
	}

	public void No()
	{
		dialog.SetActive(false);
		SceneManager.LoadScene(StartScreen);
	}
}