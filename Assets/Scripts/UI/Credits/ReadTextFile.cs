using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ReadTextFile : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMeshProUGUI;

    void Start()
    {
        string filePath = Path.Combine(Application.dataPath, "credits.txt");
        if (File.Exists(filePath))
        {
            string text = File.ReadAllText(filePath);
            m_TextMeshProUGUI.text = text;
            Debug.Log(text);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}
