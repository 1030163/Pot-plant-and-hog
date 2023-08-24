using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class ScreenshotCapture : MonoBehaviour
{
    public string folderName = "ppahScreenshots";

    public void CaptureScreenshotOnDesktop()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

        // Combine the desktop path with the folder name
        string folderPath = Path.Combine(desktopPath, folderName);

        // Create the folder if it doesn't exist
        Directory.CreateDirectory(folderPath);

        // Generate a unique file name with a timestamp
        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string screenshotFileName = "screenshot" + timestamp + ".png";

        // Specify the file path for the screenshot
        string screenshotPath = Path.Combine(folderPath, screenshotFileName);

        // Capture the screenshot
        ScreenCapture.CaptureScreenshot(screenshotPath);
        Debug.Log(screenshotFileName + "captured at: " + folderPath);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            CaptureScreenshotOnDesktop();
        }
    }
}