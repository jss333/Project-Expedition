using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

public static class EditorUtils
{
    public static void DisplayDialogBox(string aMessage)
    {
        EditorUtility.DisplayDialog("Warning", aMessage, "OK");
    }

    public static GameObject GetSelectedObject(string aWarningMessage)
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (!selectedObject)
        {
            EditorUtility.DisplayDialog("Warning", aWarningMessage, "OK");
            return null;
        }
        else
        {
            return selectedObject;
        }
    }

    public static GameObject GetSelectedObject()
    {
        GameObject selectedObject = Selection.activeGameObject;
        if (!selectedObject)
        {
            return null;
        }
        else
        {
            return selectedObject;
        }
    }
}