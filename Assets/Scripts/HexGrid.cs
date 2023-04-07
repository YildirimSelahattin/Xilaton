using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;

    private float hexWidth;
    private float hexHeight;

    public Color touchColor = Color.red;
    public List<GameObject> touchedHexes = new List<GameObject>();

    public GameObject prevTouchedHex;
    public string touchedLetters;

    private HashSet<string> englishWords = new HashSet<string>();
    public List<GameObject> gridList;

    public bool isGettingTouch = false;
    public float spacing = 0.1f;

    public static HexGrid Instance;
    Camera cam;
    public static bool loadDeckDirectly;

    private int comboCounter = 1;

    public GameObject clueFindStartObject;
    public string clueString;
    public List<GameObject> cluePointList;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        cam = Camera.main;
        if (loadDeckDirectly)
        {
            CreateLevelByIndex(GameDataManager.Instance.levelToLoadWhenNextPressed);
            isGettingTouch = true;
            loadDeckDirectly = false;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && isGettingTouch)
        {
            Touch touch = Input.GetTouch(0);

            //if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                worldTouchPosition.z = 0;

                GameObject touchedHexa = GetHexAtPosition(worldTouchPosition);

                if (touchedHexa != null)
                {
                    HexCell hexCell = touchedHexa.GetComponent<HexCell>();
                    int cellIndex = hexCell.GetIndex();
                    char letter = hexCell.GetComponentInChildren<TextMeshPro>().text[0];
                    if (touch.phase == TouchPhase.Began)//if it is first touch
                    {
                        if (cellIndex == 2) // if it is a start grid
                        {
                            touchedHexes.Add(touchedHexa);
                            touchedLetters = hexCell.GetComponentInChildren<TextMeshPro>().text;
                            Debug.Log("CCCC");
                            prevTouchedHex = touchedHexa;
                            Vector3 originPos = hexCell.originPos;
                            Vector3 originChildPos = hexCell.originChildPos;
                            hexCell.transform.DOMove(new Vector3(originPos.x, originPos.y - 0.03f, originPos.z), 0.1f);
                            hexCell.transform.GetChild(1).transform.DOMove(new Vector3(originChildPos.x, originChildPos.y + 0.03f, originChildPos.z), 0.1f);
                            if (GameDataManager.Instance.playSound == 1)
                            {
                                GameObject sound = new GameObject("sound");
                                sound.AddComponent<AudioSource>().PlayOneShot(GameDataManager.Instance.forwardMoveSound);
                                Destroy(sound, GameDataManager.Instance.forwardMoveSound.length); // Creates new object, add to it audio source, play sound, destroy this object after playing is done
                            }
                        }
                    }
                    else if (touchedHexa != prevTouchedHex && prevTouchedHex != null && IsANeighbour(prevTouchedHex,touchedHexa)) // if it is different than the previous hex
                    {
                        prevTouchedHex = touchedHexa;
                        Debug.Log("DDDD");
                        Debug.Log(touchedLetters);
                        if (cellIndex == 0)
                        {
                            touchedLetters += letter;
                            hexCell.SetIndex(4);
                            touchedHexes.Add(touchedHexa);
                            touchedHexa.GetComponent<SpriteRenderer>().color = touchColor;

                            Vector3 originPos = hexCell.originPos;
                            Vector3 originChildPos = hexCell.originChildPos;
                            hexCell.transform.DOMove(new Vector3(originPos.x, originPos.y - 0.03f, originPos.z), 0.1f);
                            hexCell.transform.GetChild(1).transform.DOMove(new Vector3(originChildPos.x, originChildPos.y + 0.03f, originChildPos.z), 0.1f);
                            if (GameDataManager.Instance.playSound == 1)
                            {
                                GameObject sound = new GameObject("sound");
                                sound.AddComponent<AudioSource>().PlayOneShot(GameDataManager.Instance.forwardMoveSound);
                                Destroy(sound, GameDataManager.Instance.forwardMoveSound.length); // Creates new object, add to it audio source, play sound, destroy this object after playing is done
                            }
                        }
                        if (cellIndex == 3)
                        {
                            touchedLetters += letter;
                            hexCell.SetIndex(8);
                            cellIndex = 8;
                            touchedHexes.Add(touchedHexa);
                            Vector3 originPos = hexCell.originPos;
                            Vector3 originChildPos = hexCell.originChildPos;
                            hexCell.transform.DOMove(new Vector3(originPos.x, originPos.y - 0.03f, originPos.z), 0.1f);
                            hexCell.transform.GetChild(1).transform.DOMove(new Vector3(originChildPos.x, originChildPos.y + 0.03f, originChildPos.z), 0.1f);

                        }
                        if (cellIndex == 4)
                        {
                            /*List<char> touchedletterList = touchedLetters.ToList();
                            touchedletterList.Remove(hexCell.GetComponentInChildren<TextMeshPro>().text[0]);
                            touchedLetters = touchedletterList.ToString();*/
                            char[] touchedletterArray = touchedLetters.ToCharArray();
                            List<char> touchedletterList = touchedletterArray.ToList();
                            int index = Array.IndexOf(touchedHexes.ToArray(), touchedHexa);
                            Debug.Log("index" + index);
                            for (int i = touchedHexes.Count - 1; i > index; i--)
                            {
                                if (touchedHexes[i].GetComponent<HexCell>().index == 8)
                                {
                                    continue;
                                }

                                Vector3 originPos = touchedHexes[i].GetComponent<HexCell>().originPos;
                                Vector3 originChildPos = touchedHexes[i].GetComponent<HexCell>().originChildPos;
                                touchedHexes[i].transform.DOMove(new Vector3(originPos.x, originPos.y, originPos.z), 0.1f);
                                touchedHexes[i].transform.GetChild(1).transform.DOMove(new Vector3(originChildPos.x, originChildPos.y, originPos.z), 0.1f);
                                SpriteRenderer touchedSpriteRenderer = touchedHexes[i].GetComponent<SpriteRenderer>();
                                touchedSpriteRenderer.color = new Color(240 / 255f, 240 / 255f, 240 / 255f, 1);
                                touchedHexes[i].GetComponent<SpriteRenderer>().color = new Color(240 / 255f, 240 / 255f, 240 / 255f, 1);
                                touchedletterList.RemoveAt(i);
                                touchedHexes[i].GetComponent<HexCell>().SetIndex(0);
                                touchedHexes.RemoveAt(i);

                            }
                            touchedLetters = string.Join("", touchedletterList.ToArray()); ;
                            Debug.Log(touchedLetters);

                            if (GameDataManager.Instance.playSound == 1)
                            {
                                GameObject sound = new GameObject("sound");
                                sound.AddComponent<AudioSource>().PlayOneShot(GameDataManager.Instance.backwardMoveSound);
                                Destroy(sound, GameDataManager.Instance.backwardMoveSound.length); // Creates new object, add to it audio source, play sound, destroy this object after playing is done
                            }
                        }
                        if (cellIndex == 2)
                        {
                            for (int i = touchedHexes.Count - 1; i > 0; i--)
                            {
                                Vector3 originPos = touchedHexes[i].GetComponent<HexCell>().originPos;
                                Vector3 originChildPos = touchedHexes[i].GetComponent<HexCell>().originChildPos;
                                touchedHexes[i].transform.DOMove(new Vector3(originPos.x, originPos.y, originPos.z), 0.1f);
                                touchedHexes[i].transform.GetChild(1).transform.DOMove(new Vector3(originChildPos.x, originChildPos.y, originPos.z), 0.1f);
                                if (touchedHexes[i].GetComponent<HexCell>().GetIndex() == 8)
                                {
                                    touchedHexes[i].GetComponent<HexCell>().SetIndex(3);
                                    touchedHexes.RemoveAt(i);
                                    continue;
                                }
                                touchedHexes[i].GetComponent<SpriteRenderer>().color = new Color(240 / 255f, 240 / 255f, 240 / 255f, 1);
                                touchedHexes[i].GetComponent<HexCell>().SetIndex(0);
                                touchedHexes.RemoveAt(i);
                            }
                            touchedLetters = hexCell.GetComponentInChildren<TextMeshPro>().text;
                        }

                        //CONTROLL 
                        if (englishWords.Contains(touchedLetters))
                        {
                            comboCounter++;
                            UIManager.Instance.comboText.gameObject.SetActive(true);
                            UIManager.Instance.comboText.text = comboCounter + " x!";
                            StartCoroutine(CorrectFeel());

                            if (cellIndex == 8)//if it is last index 
                            {
                                UIManager.Instance.starParent.GetComponent<Animator>().enabled = true;
                                UIManager.Instance.winPanel.SetActive(true);
                                UIManager.Instance.inGameScreen.SetActive(false);
                            }
                            else
                            {
                                hexCell.SetIndex(2);
                                hexCell.Colorize(2);
                                touchedLetters = hexCell.GetComponentInChildren<TextMeshPro>().text;
                                Debug.Log("WWWW");
                                clueString = "";
                                clueString += hexCell.GetComponentInChildren<TextMeshPro>().text;
                                cluePointList.Clear();
                                clueFindStartObject = touchedHexa;
                            }

                            for (int i = 0; i < touchedHexes.Count; i++)
                            {
                                if (touchedHexes[i] != prevTouchedHex)
                                {
                                    if (touchedHexes[i].transform.tag == "Star")
                                    {
                                        touchedHexes[i].transform.GetChild(0).gameObject.SetActive(false);
                                        Vector3 pos = Camera.main.WorldToScreenPoint(touchedHexes[i].transform.GetChild(0).gameObject.transform.position);
                                        GameObject tempStar = Instantiate(UIManager.Instance.levelHeaderStar, pos, Quaternion.identity, UIManager.Instance.canvas.transform);
                                        tempStar.GetComponent<Image>().sprite = UIManager.Instance.filledStar;
                                        tempStar.transform.DOMove(UIManager.Instance.starParent.transform.GetChild(GameManager.Instance.currentStarAmount).gameObject.transform.position, 1f).OnComplete(() =>
                                        {
                                            UIManager.Instance.starParent.transform.GetChild(GameManager.Instance.currentStarAmount).gameObject.GetComponent<Image>().sprite = UIManager.Instance.filledStar;
                                            GameManager.Instance.currentStarAmount++;
                                            Destroy(tempStar.gameObject);
                                        });

                                    }

                                    touchedHexes[i].GetComponent<HexCell>().SetIndex(1);
                                    touchedHexes[i].GetComponent<HexCell>().Colorize(1);
                                }
                            }
                            if (GameDataManager.Instance.playSound == 1)
                            {
                                GameObject sound = new GameObject("sound");
                                sound.AddComponent<AudioSource>().PlayOneShot(GameDataManager.Instance.successSound);
                                Destroy(sound, GameDataManager.Instance.successSound.length); // Creates new object, add to it audio source, play sound, destroy this object after playing is done
                            }
                            touchedHexes.Clear();
                            touchedHexes.Add(touchedHexa);
                        }
                    }
                }
                
            }

            if (touch.phase == TouchPhase.Ended)
            {
                prevTouchedHex = null;
                comboCounter = 0;
                touchedLetters = "";
                // Set the index of all other touched hexagons to 1

                for (int i = 0; i < touchedHexes.Count; i++)
                {
                    if (touchedHexes[i].GetComponent<HexCell>().index == 2)
                    {
                        Vector3 originEndPos = touchedHexes[i].GetComponent<HexCell>().originPos;
                        Vector3 originEndChildPos = touchedHexes[i].GetComponent<HexCell>().originChildPos;
                        touchedHexes[i].transform.DOMove(new Vector3(originEndPos.x, originEndPos.y, originEndPos.z), 0.1f);
                        touchedHexes[i].transform.GetChild(1).transform.DOMove(new Vector3(originEndChildPos.x, originEndChildPos.y, originEndChildPos.z), 0.1f);
                        continue;
                    }

                    if (touchedHexes[i].GetComponent<HexCell>().index == 8)
                    {
                        Vector3 originEndPos = touchedHexes[i].GetComponent<HexCell>().originPos;
                        Vector3 originEndChildPos = touchedHexes[i].GetComponent<HexCell>().originChildPos;
                        touchedHexes[i].transform.DOMove(new Vector3(originEndPos.x, originEndPos.y, originEndPos.z), 0.1f);
                        touchedHexes[i].transform.GetChild(1).transform.DOMove(new Vector3(originEndChildPos.x, originEndChildPos.y, originEndChildPos.z), 0.1f);
                        touchedHexes[i].GetComponent<HexCell>().SetIndex(3);
                        continue;
                    }
                    Vector3 originPos = touchedHexes[i].GetComponent<HexCell>().originPos;
                    Vector3 originChildPos = touchedHexes[i].GetComponent<HexCell>().originChildPos;
                    touchedHexes[i].transform.DOMove(new Vector3(originPos.x, originPos.y, originPos.z), 0.1f);
                    touchedHexes[i].transform.GetChild(1).transform.DOMove(new Vector3(originChildPos.x, originChildPos.y, originPos.z), 0.1f);
                    SpriteRenderer touchedSpriteRenderer = touchedHexes[i].GetComponent<SpriteRenderer>();
                    touchedSpriteRenderer.color = new Color(240 / 255f, 240 / 255f, 240 / 255f, 1);
                    touchedHexes[i].GetComponent<HexCell>().SetIndex(0);
                }


                // Clear the lists for the next touch event
                touchedHexes.Clear();
            }
        }
    }

    public IEnumerator CorrectFeel()
    {
        cam.backgroundColor = new Color(153, 255, 110);
        yield return new WaitForSeconds(0.2f);
        cam.backgroundColor = new Color(233, 233, 233);
    }

    public void CreateLevelByIndex(int levelNumber)
    {
        levelNumber--;
        LoadEnglishWords(levelNumber);
        hexWidth = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        hexHeight = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        gridWidth = GameDataManager.Instance.data.deckArray[levelNumber].gridWidth;
        gridHeight = GameDataManager.Instance.data.deckArray[levelNumber].gridHeight;
        isGettingTouch = true;
        CreateGrid(levelNumber);
    }

    GameObject GetHexAtPosition(Vector3 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject touchedObject = hit.collider.gameObject;

            if (touchedObject.transform.parent == this.transform)
            {
                return touchedObject;
            }
        }

        return null;
    }

    void CreateGrid(int levelNumber)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                int index = (y * gridWidth) + x;
                float xPos = x * (hexWidth * 0.75f + spacing);
                float yPos = -y * (hexHeight + spacing);

                if (x % 2 == 1)
                {
                    yPos += (hexHeight * 0.5f + spacing / 2);
                }

                GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, yPos - 0.5f, -y), Quaternion.identity);
                hex.transform.parent = this.transform;
                hex.name = "Hex_" + x + "_" + y;
                TextMeshPro textMeshPro = hex.GetComponentInChildren<TextMeshPro>();
                textMeshPro.text = GameDataManager.Instance.data.deckArray[levelNumber].gridValueIndexes[index];

                hex.GetComponent<HexCell>().SetIndex(0);
                hex.GetComponent<HexCell>().columnIndex = x;
                hex.GetComponent<HexCell>().rowIndex = y;
                gridList.Add(hex);

                if (GameDataManager.Instance.data.deckArray[levelNumber].gridValueIndexes[index].Equals(""))
                {
                    Destroy(hex);
                }

                if (GameDataManager.Instance.data.deckArray[levelNumber].starSpotIndexes.Contains(index))
                {
                    hex.transform.GetChild(0).gameObject.SetActive(true);
                    hex.transform.tag = "Star";
                }

                if (x == gridWidth / 2 && y == gridHeight / 2)
                {
                    Vector3 posHex = hex.transform.position;
                    cam.transform.position = new Vector3(posHex.x, posHex.y, cam.transform.position.z);

                    if (gridWidth >= gridHeight)
                    {
                        cam.orthographicSize = gridWidth + 1;
                    }
                    else
                    {
                        cam.orthographicSize = gridHeight + 1;
                    }
                }
            }

        }

        gridList[GameDataManager.Instance.data.deckArray[levelNumber].startPoint].GetComponent<HexCell>()
            .SetIndex(2);
        gridList[GameDataManager.Instance.data.deckArray[levelNumber].startPoint].GetComponent<HexCell>()
           .Colorize(2);
        //value of start point is 2 
        gridList[GameDataManager.Instance.data.deckArray[levelNumber].stopPoint].GetComponent<HexCell>()
            .SetIndex(3); //value of end point is 3;
        gridList[GameDataManager.Instance.data.deckArray[levelNumber].stopPoint].GetComponent<HexCell>()
            .Colorize(3); //value of end point is 3;
        clueString = gridList[GameDataManager.Instance.data.deckArray[levelNumber].startPoint].GetComponentInChildren<TextMeshPro>().text;
        clueFindStartObject = gridList[GameDataManager.Instance.data.deckArray[levelNumber].startPoint];
    }

    private void LoadEnglishWords(int levelNumber)
    {
        foreach (string word in GameDataManager.Instance.data.deckArray[levelNumber].wordsCanBeFoundArray)
        {
            englishWords.Add(word); // Harfler büyük olduğu için kelimeleri büyük harfe çeviriyoruz
        }
    }
    public void PaintHintHexa()
    {
        int clueHexIndex = GetNextClueIndex();
        Debug.Log(clueHexIndex);
        if (clueHexIndex != -1)
        {
            gridList[clueHexIndex].GetComponent<HexCell>().hintObject.SetActive(true);
        }
    }

    private int GetNextClueIndex()
    {
        int rowIndex = clueFindStartObject.GetComponent<HexCell>().rowIndex;
        int columnIndex = clueFindStartObject.GetComponent<HexCell>().columnIndex;
        Debug.Log(rowIndex + "qwe" + columnIndex);

        if (rowIndex - 1 >= 0)//Has a upper row
        {
            for (int i = -1; i < 2; i++)
            {
                if (columnIndex + i < 0)
                {
                    continue;
                }
                int index = (gridWidth * (rowIndex - 1)) + columnIndex + i;
                Debug.Log(index + "index");
                if ((gridList[index].IsDestroyed()) || index < 0 || columnIndex + i < 0 || columnIndex + i >= gridWidth || gridList[index].GetComponent<HexCell>().index == 1 || cluePointList.Contains(gridList[index]))
                {
                    continue;
                }

                foreach (string word in englishWords)
                {

                    if (word.StartsWith(clueString + gridList[index].GetComponentInChildren<TextMeshPro>().text))
                    {
                        clueString += gridList[index].GetComponentInChildren<TextMeshPro>().text;
                        cluePointList.Add(gridList[index]);
                        clueFindStartObject = gridList[index];
                        return index;
                    }
                }
            }
        }

        for (int i = -1; i < 2; i = i + 2)
        {

            int index = (gridWidth * (rowIndex)) + columnIndex + i;
            if ((gridList[index].IsDestroyed()) || index < 0 || columnIndex + i < 0 || columnIndex + i >= gridWidth || gridList[index].GetComponent<HexCell>().index == 1 || cluePointList.Contains(gridList[index]))
            {
                continue;
            }
            foreach (string word in englishWords)
            {
                Debug.Log(word);
                Debug.Log("qwe1" + clueString + gridList[index].GetComponentInChildren<TextMeshPro>().text);
                if (word.StartsWith(clueString + gridList[index].GetComponentInChildren<TextMeshPro>().text))
                {
                    clueString += gridList[index].GetComponentInChildren<TextMeshPro>().text;
                    cluePointList.Add(gridList[index]);
                    clueFindStartObject = gridList[index];
                    return index;
                }
            }
        }

        if (rowIndex + 1 < gridWidth )//Has a lower row
        {
            Debug.Log("gridim");
            for (int i = -1; i < 2; i++)
            {

                int index = (gridWidth * (rowIndex + 1)) + columnIndex + i;

                if ((gridList[index].IsDestroyed()) || index < 0 || columnIndex + i < 0 || columnIndex + i >= gridWidth || gridList[index].GetComponent<HexCell>().index == 1 || cluePointList.Contains(gridList[index]))
                {
                    Debug.Log("geçtim");
                    continue;
                }
                foreach (string word in englishWords)
                {
                    if (word.StartsWith(clueString + gridList[index].GetComponentInChildren<TextMeshPro>().text))
                    {
                        clueString += gridList[index].GetComponentInChildren<TextMeshPro>().text;
                        Debug.Log("clueString"+clueString);

                        cluePointList.Add(gridList[index]);
                        clueFindStartObject = gridList[index];
                        return index;
                    }
                }
            }
        }

        return -1;

    }

    public bool IsANeighbour(GameObject prevTouchedHex,GameObject touchedHexa)
    {
        int rowIndexPrev = prevTouchedHex.GetComponent<HexCell>().rowIndex;
        int columnIndexPrev = prevTouchedHex.GetComponent<HexCell>().columnIndex; 
        int rowIndexCur = touchedHexa.GetComponent<HexCell>().rowIndex;
        int columnIndexCur = touchedHexa.GetComponent<HexCell>().columnIndex;
        if (rowIndexCur - 1 == rowIndexPrev)//Has a upper row
        {
            if (columnIndexCur==columnIndexPrev + 1|| columnIndexCur == columnIndexPrev|| columnIndexCur == columnIndexPrev  -1)
            {
                return true;
            }
        }

        else if (rowIndexCur== rowIndexPrev)
        {
            if (columnIndexCur == columnIndexPrev + 1 ||  columnIndexCur == columnIndexPrev  -1)
            {
                return true;
            }
        }

        else if (rowIndexCur + 1 == rowIndexPrev)//Has a lower row
        {

            if (columnIndexCur == columnIndexPrev + 1 || columnIndexCur == columnIndexPrev || columnIndexCur == columnIndexPrev  -1)
            {
                return true;
            }
        }
        
        return false;
        
    }
}

