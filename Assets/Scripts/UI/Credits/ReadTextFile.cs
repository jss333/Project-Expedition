using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReadTextFile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI creditsTextComponent;

    void Start()
    {
        //string creditsFilePath = Path.Combine(Application.dataPath, "credits.txt");
        //if (File.Exists(creditsFilePath))
        //{
        //    string creditsText = File.ReadAllText(creditsFilePath);
        //    creditsTextComponent.text = creditsText;
        //}
        //else
        //{
        //    Debug.LogError("Credits file not found: " + creditsFilePath);
        //}
    }
}
