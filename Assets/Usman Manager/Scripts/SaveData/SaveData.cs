using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProps
{
    public string playerName;
    public int playerHealth;
    public int playerDamage;
    public int playerRange;
    public bool isLocked = true;
}

[System.Serializable]
public class Modesprops
{
    public bool isLocked;
}

[System.Serializable]
public class UbtanModeElements
{
    public List<bool> jewellery = new List<bool>();
    public List<bool> gajary = new List<bool>();

}

[System.Serializable]
public class GamePlayModeElements
{
    public List<bool> bag = new List<bool>();
    public List<bool> bindi = new List<bool>();
    public List<bool> blush = new List<bool>();
    public List<bool> bangle = new List<bool>();
    public List<bool> closedEyeshade = new List<bool>();
    public List<bool> dress = new List<bool>();
    public List<bool> earing = new List<bool>();
    public List<bool> eyeshade = new List<bool>();
    public List<bool> hair = new List<bool>();
    public List<bool> mathapati = new List<bool>();
    public List<bool> mehandi = new List<bool>();
    public List<bool> necklace = new List<bool>();
    public List<bool> nosering = new List<bool>();

}


[System.Serializable]
public class SaveData
{

    public static SaveData instance;
    public static SaveData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveData();
            }
            return instance;
        }
    }
    public bool RemoveAds = false;
    //public int vsMode;
    public int LevelsUnlocked = 1;
    public int levelIndex = 0;
    public int EventsUnlocked = 0;
    //public int SelectedAvatar = 0;
    public int selectedCharacter;
    public string ProfileName;
    public bool ProfileCreated = false;
    public bool isSound = true, isMusic = true, isVibration = true, isRightControls = true;
    public int Coins = 3000;
    public List<PlayerProps> Players = new List<PlayerProps>();
    public List<Modesprops> ModeProps = new List<Modesprops>();
    public UbtanModeElements UbtanModeElements = new UbtanModeElements();
    public GamePlayModeElements GamePlayModeElements = new GamePlayModeElements();
    public string hashOfSaveData;

    //Constructor to save actual GameData
    public SaveData() { }

    //Constructor to check any tampering with the SaveData
    public SaveData(bool ads, int levelsUnlocked, int eventsUnlocked, int coins, bool soundOn, bool musicOn, bool vibrationOn, bool rightControls, List<PlayerProps> _players,
                    List<Modesprops> _modeProps, UbtanModeElements _UbtanModeElements, GamePlayModeElements _GamePlayModeElements)
    {
        RemoveAds = ads;
        LevelsUnlocked = levelsUnlocked;
        EventsUnlocked = eventsUnlocked;
        Coins = coins;
        isSound = soundOn;
        isMusic = musicOn;
        isVibration = vibrationOn;
        isRightControls = rightControls;
        Players = _players;
        ModeProps = _modeProps;
        UbtanModeElements = _UbtanModeElements;
        GamePlayModeElements = _GamePlayModeElements;
    }
}