using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;
    public int currentStarAmount = 0;
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
}
