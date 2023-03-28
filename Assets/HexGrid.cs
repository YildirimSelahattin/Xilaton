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
    
    public string[] letters = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"}; // Harfleri tanımlayın.
    
    public Color touchColor = Color.red; // Dokunulan altıgenin rengini belirleyin.
    private List<GameObject> touchedHexes = new List<GameObject>(); // Touched hexagons list
    private List<Color> originalColors = new List<Color>(); // Original colors list
    
    private GameObject prevTouchedHex; // Önceki dokunulan altıgeni saklamak için.
    private string touchedLetters = ""; // Touched letters string
    
    private HashSet<string> englishWords = new HashSet<string>(); // İngilizce kelimeleri saklamak için

    void Start()
    {
        LoadEnglishWords();
        hexWidth = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        hexHeight = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        gridWidth = GameDataManager.Instance.data.deckArray[0].gridWidth;
        gridHeight = GameDataManager.Instance.data.deckArray[0].gridHeight;
        CreateGrid();
        transform.position = new Vector3(-1.9f, -3, 0);
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
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
                        originalColors.Add(touchedSpriteRenderer.color);
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
                }
                else
                {
                    Debug.Log("Not a valid English word: " + touchedLetters);

                    for (int i = 0; i < touchedHexes.Count; i++)
                    {
                        SpriteRenderer touchedSpriteRenderer = touchedHexes[i].GetComponent<SpriteRenderer>();
                        touchedSpriteRenderer.color = originalColors[i];
                    }
                }

                // Clear the lists for the next touch event
                touchedHexes.Clear();
                originalColors.Clear();
            }

        }
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

    void CreateGrid()
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
                textMeshPro.text = GameDataManager.Instance.data.deckArray[0].gridValueIndexes[index];
                if (GameDataManager.Instance.data.deckArray[0].starSpotIndexes.Contains(index))
                {
                    textMeshPro.color = Color.blue;
                }
            }
        }
    }
    
    private void LoadEnglishWords()
    {
        foreach (string word in GameDataManager.Instance.data.deckArray[0].wordsCanBeFoundArray)
        {
            englishWords.Add(word); // Harfler büyük olduğu için kelimeleri büyük harfe çeviriyoruz
        }
    }
    
}