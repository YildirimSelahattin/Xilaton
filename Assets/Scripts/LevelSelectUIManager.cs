using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LevelSelectUIManager : MonoBehaviour
{
    public GameObject gridPrefab;
    public GameObject gridParent;
    public List<GameObject> gridList;
    public GameObject[] levelButtonPrefab;
    public int howManyToAdd;
    public int currentGridIndex;
    public int buttonPerGrid;
    public float gridWidth;
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject starObject;
    public Sprite filledStarSprite;
    public Sprite emptyStarSprite;

    public Sprite notInteractableButtonSprite;
    public Sprite finishedButtonSprite;

    public Image LevelSelectionBG;
    public Image LevelViewPort;
    public Sprite[] LevelViewPortSprites;

    void Start()
    {
        currentGridIndex = GameDataManager.Instance.levelToLoad / (buttonPerGrid + 1);
        gridParent.transform.DOLocalMoveY(gridWidth * currentGridIndex, 0.4f);
        CreateLevelPanels();
        ControlRightLeftButton();
    }

    public void ControlRightLeftButton()
    {
        if (currentGridIndex == 0)
        {
            leftArrow.SetActive(false);
            rightArrow.SetActive(true);
            LevelViewPort.sprite = LevelViewPortSprites[0];
            LevelSelectionBG.color = new Color(164 / 255f, 206 / 255f, 95 / 255f, 1);
        }
        else if (currentGridIndex == 1)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
            LevelViewPort.sprite = LevelViewPortSprites[1];
            LevelSelectionBG.color = new Color(255 / 255f, 181 / 255f, 71 / 255f, 1);
        }
        else if (currentGridIndex == 2)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
            LevelViewPort.sprite = LevelViewPortSprites[2];
            LevelSelectionBG.color = new Color(140 / 255f, 193 / 255f, 255 / 255f, 1);
        }
        else if (currentGridIndex == 3)
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(false);
            LevelViewPort.sprite = LevelViewPortSprites[0];
            LevelSelectionBG.color = new Color(164 / 255f, 206 / 255f, 95 / 255f, 1);
        }
        else
        {
            leftArrow.SetActive(true);
            rightArrow.SetActive(true);
            LevelViewPort.sprite = LevelViewPortSprites[0];
            LevelSelectionBG.color = new Color(164 / 255f, 206 / 255f, 95 / 255f, 1);
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
            GameObject grid = Instantiate(gridPrefab, gridParent.transform); //bg
            grid.transform.localPosition = new Vector3(0, -gridWidth * gridCounter, 0);
            gridList.Add(grid);
            gridCounter++; //gird counter

            for (int i = 0; i < howManyToAdd; i++)
            {
                if (gridCounter == 1)
                {
                    int index = (gridCounter - 1) * 9 + i + 1;
                    GameObject levelButton = Instantiate(levelButtonPrefab[0], grid.transform); //button
                    LevelButtonManager buttonScript = levelButton.GetComponent<LevelButtonManager>();
                    buttonScript.levelIndex = index;
                    buttonScript.levelNumberText.text = index.ToString();

                    if (index > GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = notInteractableButtonSprite;
                        levelButton.GetComponent<Button>().interactable = false;
                    }
                    else if (index == GameDataManager.Instance.levelToLoad)
                    {
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            star.GetComponent<Image>().sprite = emptyStarSprite;
                        }
                    }
                    else if (index < GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = finishedButtonSprite;
                        levelButton.GetComponent<LevelButtonManager>().levelNumberText.color = Color.white;
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        int earnedStarNumber = PlayerPrefs.GetInt("LevelStar" + index, 0);
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            if (earnedStarNumber > 0)
                            {
                                star.GetComponent<Image>().sprite = filledStarSprite;
                                earnedStarNumber--;
                            }
                            else
                            {
                                star.GetComponent<Image>().sprite = emptyStarSprite;
                            }
                        }
                    }
                }
                else if (gridCounter == 2)
                {
                    int index = (gridCounter - 1) * 9 + i + 1;
                    GameObject levelButton = Instantiate(levelButtonPrefab[1], grid.transform); //button
                    LevelButtonManager buttonScript = levelButton.GetComponent<LevelButtonManager>();
                    buttonScript.levelIndex = index;
                    buttonScript.levelNumberText.text = index.ToString();

                    if (index > GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = notInteractableButtonSprite;
                        levelButton.GetComponent<Button>().interactable = false;
                    }
                    else if (index == GameDataManager.Instance.levelToLoad)
                    {
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            star.GetComponent<Image>().sprite = emptyStarSprite;
                        }
                    }
                    else if (index < GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = finishedButtonSprite;
                        levelButton.GetComponent<LevelButtonManager>().levelNumberText.color = Color.white;
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        int earnedStarNumber = PlayerPrefs.GetInt("LevelStar" + index, 0);
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            if (earnedStarNumber > 0)
                            {
                                star.GetComponent<Image>().sprite = filledStarSprite;
                                earnedStarNumber--;
                            }
                            else
                            {
                                star.GetComponent<Image>().sprite = emptyStarSprite;
                            }
                        }
                    }
                }
                else if (gridCounter == 3)
                {
                    int index = (gridCounter - 1) * 9 + i + 1;
                    GameObject levelButton = Instantiate(levelButtonPrefab[2], grid.transform); //button
                    LevelButtonManager buttonScript = levelButton.GetComponent<LevelButtonManager>();
                    buttonScript.levelIndex = index;
                    buttonScript.levelNumberText.text = index.ToString();

                    if (index > GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = notInteractableButtonSprite;
                        levelButton.GetComponent<Button>().interactable = false;
                    }
                    else if (index == GameDataManager.Instance.levelToLoad)
                    {
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            star.GetComponent<Image>().sprite = emptyStarSprite;
                        }
                    }
                    else if (index < GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = finishedButtonSprite;
                        levelButton.GetComponent<LevelButtonManager>().levelNumberText.color = Color.white;
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        int earnedStarNumber = PlayerPrefs.GetInt("LevelStar" + index, 0);
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            if (earnedStarNumber > 0)
                            {
                                star.GetComponent<Image>().sprite = filledStarSprite;
                                earnedStarNumber--;
                            }
                            else
                            {
                                star.GetComponent<Image>().sprite = emptyStarSprite;
                            }
                        }
                    }
                }
                else
                {
                    int index = (gridCounter - 1) * 9 + i + 1;
                    GameObject levelButton = Instantiate(levelButtonPrefab[0], grid.transform); //button
                    LevelButtonManager buttonScript = levelButton.GetComponent<LevelButtonManager>();
                    buttonScript.levelIndex = index;
                    buttonScript.levelNumberText.text = index.ToString();

                    if (index > GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = notInteractableButtonSprite;
                        levelButton.GetComponent<Button>().interactable = false;
                    }
                    else if (index == GameDataManager.Instance.levelToLoad)
                    {
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            star.GetComponent<Image>().sprite = emptyStarSprite;
                        }
                    }
                    else if (index < GameDataManager.Instance.levelToLoad)
                    {
                        levelButton.GetComponent<Image>().sprite = finishedButtonSprite;
                        levelButton.GetComponent<LevelButtonManager>().levelNumberText.color = Color.white;
                        int starNumber = GameDataManager.Instance.data.deckArray[index - 1].starSpotIndexes.Count;
                        int earnedStarNumber = PlayerPrefs.GetInt("LevelStar" + index, 0);
                        for (int starCounter = 0; starCounter < starNumber; starCounter++)
                        {
                            GameObject star = Instantiate(starObject, buttonScript.starParent.transform);
                            if (earnedStarNumber > 0)
                            {
                                star.GetComponent<Image>().sprite = filledStarSprite;
                                earnedStarNumber--;
                            }
                            else
                            {
                                star.GetComponent<Image>().sprite = emptyStarSprite;
                            }
                        }
                    }
                }
            }
        }
    }

    public void SlideRight()
    {
        currentGridIndex++;
        gridParent.transform.DOLocalMoveY(gridWidth * currentGridIndex, 0.4f);
        ControlRightLeftButton();
        UIManager.Instance.PlayUISound();
    }
    public void Slideleft()
    {
        currentGridIndex--;
        gridParent.transform.DOLocalMoveY(gridWidth * currentGridIndex, 0.4f);
        ControlRightLeftButton();
        UIManager.Instance.PlayUISound();
    }

}

