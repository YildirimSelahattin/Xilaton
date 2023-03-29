using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;

    private float hexWidth;
    private float hexHeight;

    public string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

    public Color touchColor = Color.red; // Dokunulan altıgenin rengi.
    private List<GameObject> touchedHexes = new List<GameObject>(); // Touched hexagons list

    private GameObject prevTouchedHex; // Onceki dokunulan altigeni saklamak icin
    private string touchedLetters = "asdasd"; // Touched letters string
    
    private HashSet<string> englishWords = new HashSet<string>(); // Kelimeleri saklamak için
    public List<GameObject> gridList;

    public bool isGettingTouch = false;
    public bool isTrueBegan = false;
    public float spacing = 0.1f;
    
    public static HexGrid Instance;
    Camera cam;
    public static bool loadDeckDirectly;

    private int comboCounter = 1;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        cam = Camera.main;
        if(loadDeckDirectly == true)
        {
            CreateLevelByIndex(GameDataManager.Instance.levelToLoad);
            isGettingTouch = true;
            loadDeckDirectly = false;
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && isGettingTouch != false)
        {
            Touch touch = Input.GetTouch(0);

            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Vector3 worldTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                worldTouchPosition.z = 0;

                GameObject touchedHexa = GetHexAtPosition(worldTouchPosition);
                
                if (touchedHexa != null )
                {
                    int cellIndex = touchedHexa.GetComponent<HexCell>().GetIndex();
                    if (touch.phase == TouchPhase.Began)
                    {
                        if (cellIndex == 2)
                        {
                            touchedLetters = ""; // Clear the string when the touch begins
                            touchedLetters += touchedHexa.GetComponentInChildren<TextMeshPro>().text;
                        }
                    }
                    else
                    {
                        if (touchedHexa != prevTouchedHex)
                        {
                            TextMeshPro touchedTextMeshPro = touchedHexa.GetComponentInChildren<TextMeshPro>();

                            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
                            {
                                touchedLetters += touchedTextMeshPro.text; // Add the letter to the string
                                Debug.Log("Current string: " + touchedLetters);
                            }

                            SpriteRenderer touchedSpriteRenderer = touchedHexa.GetComponent<SpriteRenderer>();
                            touchedHexes.Add(touchedHexa);
                            touchedSpriteRenderer.color = touchColor;

                            prevTouchedHex = touchedHexa;
                            
                            ///////////////////////////
             
                            if (englishWords.Contains(touchedLetters))
                            {
                                comboCounter++;
                                UIManager.Instance.comboText.gameObject.SetActive(true);
                                UIManager.Instance.comboText.text = comboCounter + " x!";

                                Debug.Log("It's a valid English word: " + touchedLetters);
                                StartCoroutine(CorrectFeel());
                                // Set the index of the last touched hexagon to 2
                                HexCell lastHexCell = prevTouchedHex.GetComponent<HexCell>();
                                int lastIndex = lastHexCell.GetIndex();

                                if (lastIndex == 3)
                                {
                                    UIManager.Instance.winPanel.SetActive(true);
                                    UIManager.Instance.inGameScreen.SetActive(false);
                                }
                                else
                                {
                                    lastHexCell.SetIndex(2);
                                    touchedLetters = lastHexCell.GetComponentInChildren<TextMeshPro>().text;
                                }
                                
                                foreach (GameObject touchedHex in touchedHexes)
                                {
                                    if (touchedHex != prevTouchedHex)
                                    {
                                        HexCell hexCell = touchedHex.GetComponent<HexCell>();
                                        hexCell.SetIndex(1);
                                    }
                                }
                                touchedHexes.Clear();
                            }
                        }
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                comboCounter = 0;

                if (englishWords.Contains(touchedLetters))
                {
                    Debug.Log("It's a valid English word: " + touchedLetters);
                    StartCoroutine(CorrectFeel());
                    // Set the index of the last touched hexagon to 2
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
                    Debug.Log("Not a valid English word: " + touchedLetters);

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
        cam.backgroundColor = Color.green;
        yield return new WaitForSeconds(0.3f);
        cam.backgroundColor = new Color(233, 233, 233);
    }

    public void CreateLevelByIndex(int levelNumber)
    {
        //jsonda leveller 0 dan baslıyor
        levelNumber--;
        LoadEnglishWords(levelNumber);
        hexWidth = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        hexHeight = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        gridWidth = GameDataManager.Instance.data.deckArray[levelNumber].gridWidth;
        gridHeight = GameDataManager.Instance.data.deckArray[levelNumber].gridHeight;
        isGettingTouch = true;//start getting player touch
        CreateGrid(levelNumber);
        //transform.position = new Vector3(-1.9f, -3, 0);
        
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

                GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, yPos - 0.5f, -y ), Quaternion.identity);
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
                if (x == gridWidth/2 && y == gridHeight/2)
                {
                    Vector3 posHex = hex.transform.position;
                    cam.transform.position = new Vector3(posHex.x, posHex.y, cam.transform.position.z);
                    cam.orthographicSize = gridWidth + 1;
                }
                
            }
        }

        gridList[GameDataManager.Instance.data.deckArray[levelNumber].startPoint].GetComponent<HexCell>().SetIndex(2);//value of start point is 2 
        gridList[GameDataManager.Instance.data.deckArray[levelNumber].stopPoint].GetComponent<HexCell>().SetIndex(3);//value of end point is 3;
    }

    private void LoadEnglishWords(int levelNumber)
    {
        foreach (string word in GameDataManager.Instance.data.deckArray[levelNumber].wordsCanBeFoundArray)
        {
            englishWords.Add(word); // Harfler büyük olduğu için kelimeleri büyük harfe çeviriyoruz
        }
    }
}