using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;



public class ItemInfo : MonoBehaviour
{
    public Button itemBtn;
    public Image itemIcon;
    public Image btnBG;
    public GameObject videoIcon;
    public GameObject coinSlot;
    public Text unlockCoins;
    public bool isLocked;
    [ShowIf("isLocked")]
    public bool videoUnlock;
    [ShowIf("isLocked")]
    public bool coinsUnlock;
    [Range(0, 50000)]
    [ShowIf("coinsUnlock")]
    public int requiredCoins;
}