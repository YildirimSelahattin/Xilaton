using System;
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

    private void Update()
    {
        if (index == 1)
        {
            Destroy(gameObject);
        }

        if (index == 2)
        {
            SpriteRenderer gridCellSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            gridCellSpriteRenderer.color = Color.yellow;
        }
    }
}