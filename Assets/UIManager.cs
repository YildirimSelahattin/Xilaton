using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIManager Instance;
    public GameObject levelSelectionPanel;
    public GameObject startScreen;
    public GameObject winPanel;
    public int levelIndex = 1;
    public static bool goStartPage = true;
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
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void OnNextLevelButtonClicked()
    {
        GameDataManager.Instance.levelToLoad++;
        HexGrid.loadDeckDirectly = true;
        UIManager.goStartPage = false;
        SceneManager.LoadScene(0);
    }
}
