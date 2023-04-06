using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI levelNumberText;
    public int levelIndex;
    public GameObject starParent;
    
    public void OnButtonClicked()
    {
        HexGrid.Instance.CreateLevelByIndex(levelIndex);
        UIManager.Instance.levelSelectionPanel.SetActive(false);
        UIManager.Instance.inGameScreen.SetActive(true);
        UIManager.Instance.starParent.SetActive(true);
        UIManager.Instance.levelText.text = "LEVEL " + GameDataManager.Instance.levelToLoad.ToString();
        UIManager.Instance.themeText.text = GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].themeName;
        UIManager.Instance.hintAmount.text = GameDataManager.Instance.hintAmount.ToString();
        if (GameDataManager.Instance.hintAmount == 0)
        {
            UIManager.Instance.hintButton.interactable = false;
        }
        for (int i = 0; i < GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].starSpotIndexes.Count; i++)
        {
            Instantiate(UIManager.Instance.levelHeaderStar, UIManager.Instance.starParent.transform);
        }
    }
}
