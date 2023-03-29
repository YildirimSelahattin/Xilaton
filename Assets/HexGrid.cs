using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;
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
    private string touchedLetters = ""; // Touched letters string

    private HashSet<string> englishWords = new HashSet<string>(); // Kelimeleri saklamak için
    public List<GameObject> gridList;

    public bool isGettingTouch = false;
    public static HexGrid Instance;
    Camera cam;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        cam = Camera.main;
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

                GameObject touchedHex = GetHexAtPosition(worldTouchPosition);

                if (touchedHex != null)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        touchedLetters = ""; // Clear the string when the touch begins
                    }

                    if (touchedHex != prevTouchedHex)
                    {
                        TextMeshPro touchedTextMeshPro = touchedHex.GetComponentInChildren<TextMeshPro>();

                        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
                        {
                            touchedLetters += touchedTextMeshPro.text; // Add the letter to the string
                            Debug.Log("Current string: " + touchedLetters);
                        }

                        SpriteRenderer touchedSpriteRenderer = touchedHex.GetComponent<SpriteRenderer>();
                        touchedHexes.Add(touchedHex);
                        touchedSpriteRenderer.color = touchColor;

                        prevTouchedHex = touchedHex;
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (englishWords.Contains(touchedLetters))
                {
                    Debug.Log("It's a valid English word: " + touchedLetters);

                    // Set the index of the last touched hexagon to 2
                    HexCell lastHexCell = prevTouchedHex.GetComponent<HexCell>();
                    lastHexCell.SetIndex(2);

                    // Set the index of all other touched hexagons to 1
                    foreach (GameObject touchedHex in touchedHexes)
                    {
                        if (touchedHex != prevTouchedHex)
                        {
                            HexCell hexCell = touchedHex.GetComponent<HexCell>();
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

    public void CreateLevelByIndex(int levelNumber)
    {
        //jsonda leveller 0 dan baslıyor
        levelNumber--;
        LoadEnglishWords();
        hexWidth = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        hexHeight = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        gridWidth = GameDataManager.Instance.data.deckArray[levelNumber].gridWidth;
        gridHeight = GameDataManager.Instance.data.deckArray[levelNumber].gridHeight;
        isGettingTouch = true;//start getting player touch
        CreateGrid(levelNumber);
        transform.position = new Vector3(-1.9f, -3, 0);
        cam.transform.position = new Vector3(transform.position.x + 0.6f, transform.position.y, cam.transform.position.z);
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
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int index = (x * gridHeight) + y;
                float xPos = x * hexWidth * 0.75f;
                float yPos = y * hexHeight;

                if (x % 2 == 1)
                {
                    yPos += hexHeight * 0.5f;
                }

                GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                hex.transform.parent = this.transform;
                hex.name = "Hex_" + x + "_" + y;
                TextMeshPro textMeshPro = hex.GetComponentInChildren<TextMeshPro>();
                textMeshPro.text = GameDataManager.Instance.data.deckArray[levelNumber].gridValueIndexes[index];
                hex.GetComponent<HexCell>().SetIndex(0);
                gridList.Add(hex);
                if (GameDataManager.Instance.data.deckArray[levelNumber].starSpotIndexes.Contains(index))
                {
                    textMeshPro.color = Color.blue;
                }
            }
        }

        gridList[GameDataManager.Instance.data.deckArray[levelNumber].startPoint].GetComponent<HexCell>().SetIndex(2);//value of start point is 2 
        gridList[GameDataManager.Instance.data.deckArray[levelNumber].stopPoint].GetComponent<HexCell>().SetIndex(3);//value of end point is 3;
    }

    private void LoadEnglishWords()
    {
        foreach (string word in GameDataManager.Instance.data.deckArray[0].wordsCanBeFoundArray)
        {
            englishWords.Add(word); // Harfler büyük olduğu için kelimeleri büyük harfe çeviriyoruz
        }
    }
}