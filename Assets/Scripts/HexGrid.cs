using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using System.Reflection;
using DG.Tweening;

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
    public bool isTrueBegan = false;
    public float spacing = 0.1f;

    public static HexGrid Instance;
    Camera cam;
    public static bool loadDeckDirectly;

    private int comboCounter = 1;

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
            CreateLevelByIndex(GameDataManager.Instance.levelToLoad);
            isGettingTouch = true;
            loadDeckDirectly = false;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && isGettingTouch)
        {
            Touch touch = Input.GetTouch(0);

            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
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
                            touchedLetters = hexCell.GetComponentInChildren<TextMeshPro>().text;
                            prevTouchedHex = touchedHexa;
                        }
                    }
                    else if (touchedHexa != prevTouchedHex && prevTouchedHex != null) // if it is different than the previous hex
                    {
                        prevTouchedHex = touchedHexa;
                        Debug.Log(touchedLetters);
                        if (cellIndex == 0)
                        {
                            touchedLetters += letter;
                            hexCell.SetIndex(4);
                            touchedHexes.Add(touchedHexa);
                            touchedHexa.GetComponent<SpriteRenderer>().color = touchColor;
                            Vector3 originPos = hexCell.transform.position;
                            hexCell.transform.DOMove(new Vector3(originPos.x, originPos.y-0.03f, originPos.z), 0.1f);
                            hexCell.transform.GetChild(1).transform.DOMove(new Vector3(originPos.x, originPos.y-0.06f, originPos.z), 0.1f);
                        }
                        if (cellIndex == 3)
                        {
                            touchedLetters += letter;
                            touchedHexes.Add(touchedHexa);
                        }
                        if (cellIndex == 4 )
                        {
                            /*List<char> touchedletterList = touchedLetters.ToList();
                            touchedletterList.Remove(hexCell.GetComponentInChildren<TextMeshPro>().text[0]);
                            touchedLetters = touchedletterList.ToString();*/
                            char[] touchedletterArray = touchedLetters.ToCharArray();
                            List<char> touchedletterList = touchedletterArray.ToList();
                            int index = Array.IndexOf(touchedHexes.ToArray(), touchedHexa);
                            Debug.Log("index" + index);
                            for (int i = touchedHexes.Count-1; i >index; i--)
                            {
                                if (touchedHexes[i].GetComponent<HexCell>().index == 3)
                                {
                                    continue;
                                }
                                touchedHexes[i].GetComponent<SpriteRenderer>().color = Color.white;
                                touchedletterList.RemoveAt(i+1);
                                touchedHexes[i].GetComponent<HexCell>().SetIndex(0);
                                touchedHexes.RemoveAt(i);
                            }
                            touchedLetters = string.Join("", touchedletterList.ToArray()); ;
                            Debug.Log(touchedLetters);

                        }
                        if(cellIndex == 2)
                        {
                            for(int i = touchedHexes.Count - 1; i >= 0; i--)
                            {
                                touchedHexes[i].GetComponent<SpriteRenderer>().color = Color.white;
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

                            if (cellIndex == 3)//if it is last index 
                            {
                                UIManager.Instance.winPanel.SetActive(true);
                                UIManager.Instance.inGameScreen.SetActive(false);
                            }
                            else
                            {
                                hexCell.SetIndex(2);
                                hexCell.Colorize(2);
                                touchedLetters = hexCell.GetComponentInChildren<TextMeshPro>().text;
                            }

                            for (int i = 0; i < touchedHexes.Count; i++)
                            {
                                if (touchedHexes[i] != prevTouchedHex)
                                {
                                    if (touchedHexes[i].transform.tag == "Star")
                                    {
                                        touchedHexes[i].transform.GetChild(0).gameObject.SetActive(false);
                                    }
                                    
                                    touchedHexes[i].GetComponent<HexCell>().SetIndex(1);
                                    touchedHexes[i].GetComponent<HexCell>().Colorize(1);
                                }
                            }
                            touchedHexes.Clear();
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
                    if (touchedHexes[i].GetComponent<HexCell>().index == 3)
                    {
                        continue;
                    }
                    SpriteRenderer touchedSpriteRenderer = touchedHexes[i].GetComponent<SpriteRenderer>();
                    touchedSpriteRenderer.color = Color.white;
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
    }

    private void LoadEnglishWords(int levelNumber)
    {
        foreach (string word in GameDataManager.Instance.data.deckArray[levelNumber].wordsCanBeFoundArray)
        {
            englishWords.Add(word); // Harfler büyük olduğu için kelimeleri büyük harfe çeviriyoruz
        }
    }
}

