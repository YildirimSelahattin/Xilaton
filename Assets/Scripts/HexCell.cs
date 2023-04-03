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
            gridCellSpriteRenderer.color = new Color(211/255f,255/255f,141/255f,1);
        }

        if (index == 2)
        {
            gridCellSpriteRenderer.color = new Color(81/255f,226/255f,255/255f,1);
        }

        if (index == 3)
        {
            gridCellSpriteRenderer.color = new Color(255/255f,104/255f,95/255f,1);
        }
    }

    
    private void Update()
    {
        
    }
}