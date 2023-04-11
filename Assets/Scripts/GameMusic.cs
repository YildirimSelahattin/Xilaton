using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMusic : MonoBehaviour
{
    public static GameMusic Instance;
    public int isSoundOn;
    public int isMusicOn;
    public int isVibrateOn;


    public Button musicOn;
    public Button musicOff;
    public Button soundOn;
    public Button soundOff;
    public Button vibrationOn;
    public Button vibrationOff;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            UpdateSound();
            UpdateMusic();
            UpdateVibrate();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Update()
    {

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
        GameDataManager.Instance.gameMusic.SetActive(false);
        PlayerPrefs.SetInt("PlayMusicKey", GameDataManager.Instance.playMusic);

        //UpdateMusic();

    }

    public void MusicOn()
    {
        GameDataManager.Instance.playMusic = 1;
        musicOff.gameObject.SetActive(false);
        musicOn.gameObject.SetActive(true);
        GameDataManager.Instance.gameMusic.SetActive(true);
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
    // Update is called once per frame
   
}
