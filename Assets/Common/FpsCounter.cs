using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCounter : MonoBehaviour
{
    string label = "";
    float count;
    private GUIStyle guiStyle = new GUIStyle();

    IEnumerator Start()
    {
        Application.targetFrameRate = 30;

        GUI.depth = 50;
        while (true)
        {
            if (Time.timeScale == 1)
            {
                yield return new WaitForSeconds(0.1f);
                count = (1 / Time.deltaTime);
                label = "FPS :" + (Mathf.Round(count));
            }
            else
            {
                label = "Pause";
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnGUI()
    {
        guiStyle.fontSize = 50;
        GUI.Label(new Rect(30, 500, 400, 200), label, guiStyle);
    }
}
