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
    public int totalLevelNumber;
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

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadData()
    {
        data = JsonUtility.FromJson<DataLists>(JSONText.text);
        //levelToLoad = PlayerPrefs.GetInt("levelToLoad",0);
        totalLevelNumber = data.deckArray.Length;
        Debug.Log(data.deckArray[0].gridValueIndexes[0]);
    }
}
