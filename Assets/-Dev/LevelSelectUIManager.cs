using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUIManager : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject gridParent;
    public List<GameObject> gridList;
    public GameObject levelButtonPrefab;
    public int howManyToAdd;
    public int currentGridIndex;
    public int buttonPerGrid;
    // Start is called before the first frame update
    void Start()
    {
        if(buttonPerGrid == 0)
        {
            currentGridIndex = 0;
        }
        else
        {
            currentGridIndex=GameDataManager.Instance.levelToLoad / buttonPerGrid;
        }
        CreateLevelPanels();


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CreateLevelPanels()
    {
        int temp = GameDataManager.Instance.totalLevelNumber;
        while (temp != 0)
        {
            if (temp >= 9)
            {
                temp -= 9;
                howManyToAdd = 9;
            }
            else
            {
                howManyToAdd = temp;
                temp = 0;
            }
            GameObject grid = Instantiate(gridPrefab, gridParent.transform);
            gridList.Add(grid);
            for (int i = 0; i < howManyToAdd; i++)
            {
                Instantiate(levelButtonPrefab, grid.transform);
            }
        }
    }
}
