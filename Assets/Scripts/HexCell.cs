using System;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public int index = 0; // Hex cell's index value
    public  SpriteRenderer gridCellSpriteRenderer;
    
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

    public void Colorize(int index)
    {
        if (index == 1)
        {
            gridCellSpriteRenderer.color = Color.green;
        }

        if (index == 2)
        {
            gridCellSpriteRenderer.color = Color.yellow;
        }

        if (index == 3)
        {
            gridCellSpriteRenderer.color = Color.red;
        }
    }

    
    private void Update()
    {
        
    }
}