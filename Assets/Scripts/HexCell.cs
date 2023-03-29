using System;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public int index = 0; // Hex cell's index value
    private SpriteRenderer gridCellSpriteRenderer;
    
    private void Start()
    {
        gridCellSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

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
        /*
        if (index == 1)
        {
            Destroy(gameObject);
        }
        */
        if (index == 1)
        {
            gridCellSpriteRenderer.color = Color.cyan;
        }

        if (index == 2)
        {
            gridCellSpriteRenderer.color = Color.yellow;
        }
        
        if (index == 3)
        {
            gridCellSpriteRenderer.color = Color.green;
        }
    }
}