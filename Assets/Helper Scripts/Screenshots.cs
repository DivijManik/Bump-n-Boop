using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Screenshots : MonoBehaviour
{
    private string filePath = null;
    private Texture2D m_Texture;

    // Use this for initialization
    private void Start()
    {
        m_Texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        filePath = "/Users/monkhub/Downloads";
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeScreenshot();
        }
    }

    public void TakeScreenshot()
    {
        StartCoroutine("CaptureCoroutine");
    }

    private IEnumerator CaptureCoroutine()
    {
        yield return new WaitForEndOfFrame();
        m_Texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, true);
        m_Texture.Apply();
        byte[] bytes = m_Texture.EncodeToPNG();
        string name_file = filePath + "/ss_" + System.DateTime.Now.ToString("yyyyMMddHHmmssffff");
        File.WriteAllBytes(name_file + ".png", bytes);
    }
}