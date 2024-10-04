using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioManagerNoMixers))]
public class SFXSOLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AudioManagerNoMixers loader = (AudioManagerNoMixers)target;
        if (GUILayout.Button("Load ScriptableObjects"))
        {
            loader.LoadScriptableObjects();
        }
    }
}
