using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HandAnimScript : MonoBehaviour
{
    public UnityEvent MouseUp;
    public UnityEvent MouseDown;
    Spa SpaController;
    public Animator foamAnim;

    public void Start()
    {
        SpaController = FindObjectOfType<Spa>();
    }
    void OnMouseDown()
    {
        SpaController.eyesImage.sprite = SpaController.closeEyeSprites[SaveData.Instance.selectedCharacter];
        MouseDown.Invoke();
    }
    void OnMouseUp()
    {
        SpaController.eyesImage.sprite = SpaController.closeEyeSprites[5];
        MouseUp.Invoke();
        if (SpaController)
        {
            bool allDone = true;
            if (foamAnim) foamAnim.enabled = false;
            for (int i = 0; i < SpaController.foamArray.Length; i++)
            {
                if (SpaController.foamArray[i].color.a < 1)
                {
                    allDone = false;
                }
            }
            if (SpaController)
            {
                if (allDone)
                {
                    SpaController.TaskDone();
                }

            }
        }

    }
}
