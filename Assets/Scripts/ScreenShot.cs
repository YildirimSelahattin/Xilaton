using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ScreenCapture.CaptureScreenshot("SomeLevel.png");
        }
    }
}
