using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    int index = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            ScreenCapture.CaptureScreenshot("C:/Users/aaron/Documents/Unity Projects/Phone Games/RocketJump/Screenshots/ScreenShot_" + index + ".png");
            index++;
        }
    }
}
