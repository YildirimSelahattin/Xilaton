using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;

    private float hexWidth;
    private float hexHeight;

    public Color touchColor = Color.red;
    private List<GameObject> touchedHexes = new List<GameObject>();

    private GameObject prevTouchedHex;
    private string touchedLetters;

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
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (cellIndex == 2)
                        {
                            touchedLetters = hexCell.GetComponentInChildren<TextMeshPro>().text;
                        }
                    }
                    else if (touchedHexa != prevTouchedHex)
                    {
                        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
                        {
                            touchedLetters += hexCell.GetComponentInChildren<TextMeshPro>().text;
                        }

                        touchedHexes.Add(touchedHexa);
                        touchedHexa.GetComponent<SpriteRenderer>().color = touchColor;

                        prevTouchedHex = touchedHexa;

                        if (englishWords.Contains(touchedLetters))
                        {
                            comboCounter++;
                            UIManager.Instance.comboText.gameObject.SetActive(true);
                            UIManager.Instance.comboText.text = comboCounter + " x!";

                            StartCoroutine(CorrectFeel());
                            int lastIndex = hexCell.GetIndex();

                            if (lastIndex == 3)
                            {
                                UIManager.Instance.winPanel.SetActive(true);
                                UIManager.Instance.inGameScreen.SetActive(false);
                            }
                            else
                            {
                                hexCell.SetIndex(2);
                                touchedLetters = hexCell.GetComponentInChildren<TextMeshPro>().text;
                            }

                            foreach (GameObject touchedHex in touchedHexes)
                            {
                                if (touchedHex != prevTouchedHex)
                                {
                                    touchedHex.GetComponent<HexCell>().SetIndex(1);
                                }
                            }
                            touchedHexes.Clear();
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                comboCounter = 0;

                if (englishWords.Contains(touchedLetters))
                {
                    StartCoroutine(CorrectFeel());
                    HexCell lastHexCell = prevTouchedHex.GetComponent<HexCell>();
                                        int lastIndex = lastHexCell.GetIndex();

                    if (lastIndex == 3)
                    {
                        UIManager.Instance.winPanel.SetActive(true);
                    }
                    else
                    {
                        lastHexCell.SetIndex(2);
                    }

                    // Set the index of all other touched hexagons to 1
                    foreach (GameObject touchedHexa in touchedHexes)
                    {
                        if (touchedHexa != prevTouchedHex)
                        {
                            HexCell hexCell = touchedHexa.GetComponent<HexCell>();
                            hexCell.SetIndex(1);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < touchedHexes.Count; i++)
                    {
                        SpriteRenderer touchedSpriteRenderer = touchedHexes[i].GetComponent<SpriteRenderer>();
                        touchedSpriteRenderer.color = Color.white;
                    }
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
                    textMeshPro.color = Color.blue;
                }

                if (x == gridWidth / 2 && y == gridHeight / 2)
                {
                    Vector3 posHex = hex.transform.position;
                    cam.transform.position = new Vector3(posHex.x, posHex.y, cam.transform.position.z);
                    cam.orthographicSize = gridWidth + 1;
                }
            }
        }

        gridList[GameDataManager.Instance.data.deckArray[levelNumber].startPoint].GetComponent<HexCell>()
            .SetIndex(2); //value of start point is 2 
        gridList[GameDataManager.Instance.data.deckArray[levelNumber].stopPoint].GetComponent<HexCell>()
            .SetIndex(3); //value of end point is 3;
    }

    private void LoadEnglishWords(int levelNumber)
    {
        foreach (string word in GameDataManager.Instance.data.deckArray[levelNumber].wordsCanBeFoundArray)
        {
            englishWords.Add(word); // Harfler büyük olduğu için kelimeleri büyük harfe çeviriyoruz
        }
    }
}

