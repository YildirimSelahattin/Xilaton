using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelSelectUIManager : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject gridParent;
    public List<GameObject> gridList;
    public GameObject levelButtonPrefab;
    public int howManyToAdd;
    public int currentGridIndex;
    public int buttonPerGrid;
    public float gridWidth;
    public GameObject leftArrow;
    public GameObject rightArrow;
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
        ControlRightLeftButton();

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ControlRightLeftButton()
    {
        if(currentGridIndex == 0)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
        }
        else if (currentGridIndex == GameDataManager.Instance.totalLevelNumber/buttonPerGrid)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
        }
    }
    public void CreateLevelPanels()
    {
        int gridCounter = 0;

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
            grid.transform.localPosition = new Vector3(gridWidth * gridCounter, 0,0);
            gridList.Add(grid);
            gridCounter++;
            for (int i = 0; i < howManyToAdd; i++)
            {
                
                GameObject levelButton = Instantiate(levelButtonPrefab, grid.transform);
                LevelButtonManager buttonScript = levelButton.GetComponent<LevelButtonManager>();
                buttonScript.levelIndex = (gridCounter-1) * 9 + i + 1;
                Debug.Log((gridCounter - 1) * 9 + i + 1);
                buttonScript.levelNumberText.text = buttonScript.levelIndex.ToString();
            }
        }
    }

    public void SlideRight()
    {
        currentGridIndex++;
        Debug.Log(-gridWidth * currentGridIndex);
        gridParent.transform.DOLocalMoveX(-gridWidth*currentGridIndex,0.4f);
        ControlRightLeftButton();
    }
    public void Slideleft()
    {
        currentGridIndex--;
        gridParent.transform.DOLocalMoveX(-gridWidth * currentGridIndex, 0.4f);
        ControlRightLeftButton();
    }

}

