using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.IO;

public class HexGrid : MonoBehaviour
{
    public GameObject hexPrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    private float hexWidth;
    private float hexHeight;
    public string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" }; // Harfleri tanımlayın.
    public Color touchColor = Color.blue; // Dokunulan altıgenin rengini belirleyin.
    public Color lastLetterColor = Color.yellow;
    private List<GameObject> touchedHexes = new List<GameObject>(); // Touched hexagons list
    private List<Color> originalColors = new List<Color>(); // Original colors list
    private GameObject prevTouchedHex; // Önceki dokunulan altıgeni saklamak için.
    private string touchedLetters = ""; // Touched letters string
    private HashSet<string> englishWords = new HashSet<string>();
    private GameObject lastTouchedHex;
    private bool isValidWord = false;
    private bool validWordFound = false;
    private bool canStartNewTouch = true;
    private bool canStartNewWord = true;
    private bool isTouchActive = true;
    private GameObject lastValidHex;
    bool firstInvalidWord = false;

    void Start()
    {
        hexWidth = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        hexHeight = hexPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        CreateGrid();
        transform.position = new Vector3(-1.9f, -3, 0);
        LoadEnglishWords();
    }

    void Update()
    {
        HandleTouchInput();
    }

    private void LoadEnglishWords()
    {
        string path = Path.Combine(Application.dataPath, "words.txt");
        string[] words = File.ReadAllLines(path);

        foreach (string word in words)
        {
            englishWords.Add(word.ToUpper()); // Harfler büyük olduğu için kelimeleri büyük harfe çeviriyoruz
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
                float xPos = x * hexWidth * 0.75f;
                float yPos = y * hexHeight;

                if (x % 2 == 1)
                {
                    yPos += hexHeight * 0.5f;
                }

                GameObject hex = Instantiate(hexPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                hex.transform.parent = this.transform;
                hex.name = "Hex_" + x + "_" + y;
                // CreateGrid() fonksiyonundaki altıgen yaratma döngüsü içinde şunları ekleyin:
                int letterIndex = Random.Range(0, letters.Length);
                TextMeshPro textMeshPro = hex.GetComponentInChildren<TextMeshPro>();
                textMeshPro.text = letters[letterIndex];
            }
        }
    }

    void HandleTouchInput()
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
                        if (lastValidHex == null || touchedHex == lastValidHex || !validWordFound)
                        {
                            touchedLetters = ""; // Clear the string when the touch begins
                            isTouchActive = true;
                        }
                        else
                        {
                            isTouchActive = false;
                        }
                    }

                    if (isTouchActive)
                    {
                        HandleTouchMoveAndBegin(touch, touchedHex);
                        HandleTouchEnd(touch);
                    }
                }
            }
        }
    }

    void HandleTouchMoveAndBegin(Touch touch, GameObject touchedHex)
    {
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

    void HandleTouchEnd(Touch touch)
    {
        if (touch.phase == TouchPhase.Ended)
        {
            if (englishWords.Contains(touchedLetters))
            {
                Debug.Log("It's a valid English word: " + touchedLetters);

                // Change the color of the last letter's hexagon to yellow
                if (touchedHexes.Count > 0)
                {
                    SpriteRenderer lastHexSpriteRenderer = touchedHexes[touchedHexes.Count - 1]
                        .GetComponent<SpriteRenderer>();
                    lastHexSpriteRenderer.color = lastLetterColor;

                    // Update last valid hex
                    lastValidHex = touchedHexes[touchedHexes.Count - 1];
                }

                validWordFound = true;
                isTouchActive = false;
            }
            else
            {
                Debug.Log("Not a valid English word: " + touchedLetters);

                for (int i = 0; i < touchedHexes.Count; i++)
                {
                    SpriteRenderer touchedSpriteRenderer =
                        touchedHexes[i].GetComponent<SpriteRenderer>();
                    touchedSpriteRenderer.color = originalColors[i];
                }

                validWordFound = false;
                isTouchActive = false;
            }

            // Clear the lists for the next touch event
            touchedHexes.Clear();
            originalColors.Clear();
        }
    }
}
