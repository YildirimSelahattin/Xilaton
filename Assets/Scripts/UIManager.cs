using System.Runtime.InteropServices;
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
    public TextMeshProUGUI hintAmount;
    public Button hintButton;

    public TextMeshProUGUI writenWordText;

    public GameObject HexGridParent;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (goStartPage == true)
        {
            startScreen.SetActive(true);
        }
        else // load level directly
        {
            StartInGameLevelUI();
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
                    for (int i = 0; i < HexGridParent.transform.childCount; i++)
                    {
                        HexGridParent.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void OnLevelsButtonClicked()
    {
        levelSelectionPanel.SetActive(true);
        startScreen.SetActive(false);
        PlayUISound();
    }

    public void OnStartButtonClicked()
    {
        HexGrid.Instance.CreateLevelByIndex(GameDataManager.Instance.levelToLoad);
        PlayUISound();
        StartInGameLevelUI();
    }

    public void OnNextLevelButtonClicked()
    {
        if (GameDataManager.Instance.currentlevel < 5)
        {
            PlayerPrefs.SetInt("HaveEverPlayedLevel" + GameDataManager.Instance.currentlevel, 1);
        }
        GameDataManager.Instance.currentlevel++;

        if (GameDataManager.Instance.currentlevel > GameDataManager.Instance.totalLevelNumber)//if game has to go level 1
        {
            GameDataManager.Instance.currentlevel = 1;
            GameDataManager.Instance.levelToLoad = 1;
        }
        else if (GameDataManager.Instance.currentlevel > GameDataManager.Instance.levelToLoad)
        {
            GameDataManager.Instance.levelToLoad = GameDataManager.Instance.currentlevel;
        }

        GameDataManager.Instance.hintAmount++;
        GameDataManager.Instance.SaveData();
        PlayUISound();
        HexGrid.loadDeckDirectly = true;
        UIManager.goStartPage = false;
        SceneManager.LoadScene(0);
    }
    public void StartInGameLevelUI()
    {
        startScreen.SetActive(false);
        inGameScreen.SetActive(true);
        starParent.SetActive(true);
        UIManager.Instance.themeText.text = GameDataManager.Instance.data.deckArray[GameDataManager.Instance.currentlevel - 1].themeName;
        hintAmount.text = GameDataManager.Instance.hintAmount.ToString();
        if (GameDataManager.Instance.hintAmount == 0)
        {
            hintButton.interactable = false;
        }
        for (int i = 0; i < GameDataManager.Instance.data.deckArray[GameDataManager.Instance.currentlevel - 1].starSpotIndexes.Count; i++)
        {
            Instantiate(UIManager.Instance.levelHeaderStar, UIManager.Instance.starParent.transform);
        }
    }

    public void OnRestartButtonClicked()
    {
        HexGrid.loadDeckDirectly = true;
        UIManager.goStartPage = false;
        PlayUISound();
        SceneManager.LoadScene(0);

    }
    public void OnHomeButtonClicked()
    {
        HexGrid.loadDeckDirectly = false;
        UIManager.goStartPage = true;
        PlayUISound();
        SceneManager.LoadScene(0);
    }

    public void GiveHint()
    {
        if (HexGrid.Instance.buttonPressTutorialHand != null)
        {
            Destroy(HexGrid.Instance.buttonPressTutorialHand.gameObject);
        }
        if (HexGrid.Instance.PaintHintHexa())
        {
            Debug.Log("aasdasdasd");
            GameDataManager.Instance.hintAmount--;
            hintAmount.text = GameDataManager.Instance.hintAmount.ToString();
            GameDataManager.Instance.SaveData();
            if (GameDataManager.Instance.hintAmount == 0)
            {
                hintButton.interactable = false;
            }
            PlayUISound();
        }
        if (HexGrid.Instance.GetNextClueIndex() == -1)
        {
            hintButton.interactable = false;
        }
    }

    public void PlayUISound()
    {
        if (GameDataManager.Instance.playSound == 1)
        {
            GameObject sound = new GameObject("sound");
            sound.AddComponent<AudioSource>();
            sound.GetComponent<AudioSource>().volume = 0.5f;
            sound.GetComponent<AudioSource>().PlayOneShot(GameDataManager.Instance.clickSound);
            Destroy(sound, GameDataManager.Instance.clickSound.length); // Creates new object, add to it audio source, play sound, destroy this object after playing is done
        }
    }

    public void OnClickOpenSettingButton()
    {
        optionsPanel.SetActive(true);

        for (int i = 0; i < HexGridParent.transform.childCount; i++)
        {
            HexGridParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void OnClickCloseSettingPanel()
    {
        optionsPanel.SetActive(false);

        for (int i = 0; i < HexGridParent.transform.childCount; i++)
        {
            HexGridParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void OnLevelPageToHome()
    {
        levelSelectionPanel.SetActive(false);
        startScreen.SetActive(true);
    }
}
