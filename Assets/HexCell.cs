using UnityEngine;

public class HexCell : MonoBehaviour
{
    public int index = 0; // Hex cell's index value

    public void SetIndex(int newIndex)
    {
        index = newIndex;
    }

    public int GetIndex()
    {
        return index;
    }
}