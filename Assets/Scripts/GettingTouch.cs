using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingTouch : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tutorialHand;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 temp = touch.position /300;
                temp.y -= 4.5f;
                temp.x -= 2f;
                temp.z = 0.9f;
                tutorialHand.transform.localPosition = temp;
            }
        }
    }
}
