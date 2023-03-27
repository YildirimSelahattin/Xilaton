using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public DataLists data;
    public TextAsset JSONText;
    public static GameDataManager Instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        Debug.Log(data.deckArray[0].gridValueIndexes[0]);
    }
}
