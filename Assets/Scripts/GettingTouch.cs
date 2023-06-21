using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GettingTouch : MonoBehaviour
{
    public RectTransform m_parent;

    public Camera m_uiCamera;

    public RectTransform m_image;

    public Canvas m_canvas;

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
                var pos = Camera.main.ScreenToWorldPoint(touch.position);
                pos.x += 0.25f;
                pos.y -= 0.5f;
                pos.z = m_image.transform.position.z;
                m_image.transform.position = pos;
            }
        }
    }
}
