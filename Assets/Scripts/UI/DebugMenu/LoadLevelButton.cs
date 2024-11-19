using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sceneNameText;
    [SerializeField] private Button button;

    public void Initialize(string sceneName)
    {
        sceneNameText.text = sceneName;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
    }
}
