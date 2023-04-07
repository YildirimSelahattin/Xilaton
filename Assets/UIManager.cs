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

    public Button musicOn;
    public Button musicOff;
    public Button soundOn;
    public Button soundOff;
    public Button vibrationOn;
    public Button vibrationOff;
    public GameObject GameMusic;
    
    public Sprite filledStar;
    public Sprite emptyStar;
    public GameObject optionsPanel;
    public TextMeshProUGUI hintAmount;
    public Button hintButton;

    public int isSoundOn;
    public int isMusicOn;
    public int isVibrateOn;
    
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
            StartInGameLevelUI();
        }
        UpdateSound();
        UpdateMusic();
        UpdateVibrate();
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

        StartInGameLevelUI();
    }

    public void OnNextLevelButtonClicked()
    {
        GameDataManager.Instance.levelToLoadWhenNextPressed++;
        if (GameDataManager.Instance.levelToLoadWhenNextPressed>GameDataManager.Instance.levelToLoad)
        {
            GameDataManager.Instance.levelToLoad = GameDataManager.Instance.levelToLoadWhenNextPressed;
        }
        GameDataManager.Instance.hintAmount++;
        GameDataManager.Instance.SaveData();
        HexGrid.loadDeckDirectly = true;
        UIManager.goStartPage = false;
        SceneManager.LoadScene(0);
    }
    public void StartInGameLevelUI()
    {
        startScreen.SetActive(false);
        inGameScreen.SetActive(true);
        starParent.SetActive(true);
        UIManager.Instance.levelText.text = "LEVEL " + GameDataManager.Instance.levelToLoad.ToString();
        UIManager.Instance.themeText.text = GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].themeName;
        hintAmount.text = GameDataManager.Instance.hintAmount.ToString();
        if (GameDataManager.Instance.hintAmount == 0)
        {
            hintButton.interactable = false;
        }
        for (int i = 0; i < GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].starSpotIndexes.Count; i++)
        {
            Instantiate(UIManager.Instance.levelHeaderStar, UIManager.Instance.starParent.transform);
        }
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
        GameDataManager.Instance.hintAmount--;
        hintAmount.text = GameDataManager.Instance.hintAmount.ToString();
        HexGrid.Instance.PaintHintHexa();
        GameDataManager.Instance.SaveData();
        if (GameDataManager.Instance.hintAmount == 0)
        {
            hintButton.interactable = false;
        }

    }
    //DEFAULT UI FUNCTIONS
    public void UpdateSound()
    {
        isSoundOn = GameDataManager.Instance.playSound;
        if (isSoundOn == 0)
        {
            soundOff.gameObject.SetActive(true);
            SoundsOff();
        }

        if (isSoundOn == 1)
        {
            soundOn.gameObject.SetActive(true);
            SoundsOn();
        }
    }
    public void UpdateMusic()
    {
        isMusicOn = GameDataManager.Instance.playMusic;
        if (isMusicOn == 0)
        {
            musicOff.gameObject.SetActive(true);
            MusicOff();
        }

        if (isMusicOn == 1)
        {
            musicOn.gameObject.SetActive(true);
            MusicOn();
        }
    }

    public void UpdateVibrate()
    {
        isVibrateOn = GameDataManager.Instance.playVibrate;
        if (isVibrateOn == 0)
        {
            vibrationOff.gameObject.SetActive(true);
            VibrationOff();
        }

        if (isVibrateOn == 1)
        {
            vibrationOn.gameObject.SetActive(true);
            VibrationOn();
        }
    }

    public void MusicOff()
    {
        GameDataManager.Instance.playMusic = 0;
        musicOn.gameObject.SetActive(false);
        musicOff.gameObject.SetActive(true);
        GameMusic.SetActive(false);
        PlayerPrefs.SetInt("PlayMusicKey", GameDataManager.Instance.playMusic);

        //UpdateMusic();

    }

    public void MusicOn()
    {
        GameDataManager.Instance.playMusic = 1;
        musicOff.gameObject.SetActive(false);
        musicOn.gameObject.SetActive(true);
        GameMusic.SetActive(true);
        PlayerPrefs.SetInt("PlayMusicKey", GameDataManager.Instance.playMusic);

        //UpdateMusic();
    }

    public void SoundsOff()
    {
        GameDataManager.Instance.playSound = 0;
        soundOn.gameObject.SetActive(false);
        soundOff.gameObject.SetActive(true);
        PlayerPrefs.SetInt("PlaySoundKey", GameDataManager.Instance.playSound);

        //UpdateSound();
    }

    public void SoundsOn()
    {
        GameDataManager.Instance.playSound = 1;
        soundOff.gameObject.SetActive(false);
        soundOn.gameObject.SetActive(true);
        PlayerPrefs.SetInt("PlaySoundKey", GameDataManager.Instance.playSound);

        //UpdateSound();
    }

    public void VibrationOff()
    {
        GameDataManager.Instance.playVibrate = 0;
        vibrationOn.gameObject.SetActive(false);
        vibrationOff.gameObject.SetActive(true);
        Handheld.Vibrate();
        PlayerPrefs.SetInt("PlayVibrateKey", GameDataManager.Instance.playVibrate);

        //UpdateVibrate();
    }

    public void VibrationOn()
    {
        GameDataManager.Instance.playVibrate = 1;
        vibrationOff.gameObject.SetActive(false);
        vibrationOn.gameObject.SetActive(true);
        Handheld.Vibrate();
        PlayerPrefs.SetInt("PlayVibrateKey", GameDataManager.Instance.playVibrate);
        //UpdateVibrate();
    }

    public void VibratePhone()
    {
        Handheld.Vibrate();
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
