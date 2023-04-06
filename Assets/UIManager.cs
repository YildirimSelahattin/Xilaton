using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;
    public GameObject levelSelectionPanel;
    public GameObject startScreen;
    public GameObject winPanel;
    public GameObject inGameScreen;
    public TextMeshProUGUI levelText;
    public int levelIndex = 1;
    public static bool goStartPage = true;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI themeText;
    public GameObject starParent;
    public GameObject levelHeaderStar;
    public GameObject canvas;

    public Sprite filledStar;
    public Sprite emptyStar;
    public GameObject optionsPanel;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        if(goStartPage == true)
        {
            startScreen.SetActive(true);
        }
        else // load level directly
        {
            startScreen.SetActive(false);
            inGameScreen.SetActive(true);
            starParent.SetActive(true);
            UIManager.Instance.levelText.text = "LEVEL " + GameDataManager.Instance.levelToLoad.ToString();
            UIManager.Instance.themeText.text = GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].themeName;
            for (int i = 0; i < GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].starSpotIndexes.Count; i++)
            {
                Instantiate(UIManager.Instance.levelHeaderStar, UIManager.Instance.starParent.transform);
            }
        }
    }

    private void Update()
    {
        if (optionsPanel.activeSelf)
        {
            // Make sure user is on Android platform
            if (Application.platform == RuntimePlatform.Android) 
            {
                // Check if Back was pressed this frame
                if (Input.GetKeyDown(KeyCode.Escape)) 
                {
                    optionsPanel.SetActive(false);
                }
            }
        }
    }

    public void OnLevelsButtonClicked()
    {
        levelSelectionPanel.SetActive(true);
        startScreen.SetActive(false);
    }

    public void OnStartButtonClicked()
    {
        HexGrid.Instance.CreateLevelByIndex(GameDataManager.Instance.levelToLoad);
        
        startScreen.SetActive(false);
        inGameScreen.SetActive(true);
        starParent.SetActive(true);
        UIManager.Instance.levelText.text = "LEVEL " + GameDataManager.Instance.levelToLoad.ToString();
        UIManager.Instance.themeText.text = GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].themeName;
        for (int i = 0; i < GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad-1].starSpotIndexes.Count; i++)
        {
            Instantiate(UIManager.Instance.levelHeaderStar, UIManager.Instance.starParent.transform);
        }
    }

    public void OnNextLevelButtonClicked()
    {
        GameDataManager.Instance.levelToLoad++;
        HexGrid.loadDeckDirectly = true;
        UIManager.goStartPage = false;
        SceneManager.LoadScene(0);
    }

    public void OnRestartButtonClicked()
    {
        HexGrid.loadDeckDirectly = true;
        UIManager.goStartPage = false;
        SceneManager.LoadScene(0);
    }
    public void OnHomeButtonClicked()
    {
        HexGrid.loadDeckDirectly = false;
        UIManager.goStartPage = true;
        SceneManager.LoadScene(0);
    }

    public void GiveHint()
    {
        HexGrid.Instance.PaintHintHexa();
    }

    public void OnClickOpenSettingButton()
    {
        optionsPanel.SetActive(true);
    }

    public void OnClickCloseSettingPanel()
    {
        optionsPanel.SetActive(false);
    }
    
    public void OnLevelPageToHome()
    {
        levelSelectionPanel.SetActive(false);
        startScreen.SetActive(true);
    }
}
