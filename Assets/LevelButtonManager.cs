using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelButtonManager : MonoBehaviour
{
    public TextMeshProUGUI levelNumberText;
    public int levelIndex;

    public void OnButtonClicked()
    {
        HexGrid.Instance.CreateLevelByIndex(levelIndex);
        UIManager.Instance.levelSelectionPanel.SetActive(false);
        UIManager.Instance.inGameScreen.SetActive(true);
        UIManager.Instance.levelText.text = "LEVEL " + levelNumberText.text;
        UIManager.Instance.themeText.text = GameDataManager.Instance.data.deckArray[GameDataManager.Instance.levelToLoad - 1].themeName;
    }
}
