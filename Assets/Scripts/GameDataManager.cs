using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public DataLists data;
    public TextAsset JSONText;
    public static GameDataManager Instance;
    public int levelToLoad;
    public int currentlevel;
    public int totalLevelNumber;
    public int hintAmount;
    public int playSound;
    public int playMusic;
    public int playVibrate;
    public AudioClip forwardMoveSound;
    public AudioClip backwardMoveSound;
    public AudioClip successSound;
    public AudioClip failSound;
    public AudioClip levelSuccessSound;
    public AudioClip clickSound;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        LoadData();
    }

    public void LoadData()
    {
        playSound = PlayerPrefs.GetInt("PlaySound", 1);
        playMusic = PlayerPrefs.GetInt("PlayMusic", 1);
        playVibrate = PlayerPrefs.GetInt("PlayVibrate", 1);
        data = JsonUtility.FromJson<DataLists>(JSONText.text);
        levelToLoad = PlayerPrefs.GetInt("levelToLoad", 1);
        currentlevel = levelToLoad;
        totalLevelNumber = data.deckArray.Length;
        Debug.Log(data.deckArray[0].gridValueIndexes[0]);
        hintAmount = PlayerPrefs.GetInt("HintAmount", 3);
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt("PlaySound", playSound);
        PlayerPrefs.SetInt("PlayMusic", playMusic);
        PlayerPrefs.SetInt("PlayVibrate", playVibrate);
        PlayerPrefs.SetInt("HintAmount", hintAmount);
        PlayerPrefs.SetInt("levelToLoad", levelToLoad);
    }
}
