using UnityEngine;
 
public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexPressed;
    public Texture2D cursorTexReleased;
    public static CursorManager Instance;

    void Awake() { 
        
        if(Instance == null){
            Instance = this;
        }
        Cursor.SetCursor(cursorTexReleased, Vector2.zero, CursorMode.ForceSoftware);  }

    public void Pressed()
    {
        Cursor.SetCursor(cursorTexPressed, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void Release()
    {
        Cursor.SetCursor(cursorTexReleased, Vector2.zero, CursorMode.ForceSoftware);
    }
}