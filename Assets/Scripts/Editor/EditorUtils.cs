using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

public static class EditorUtils
{
    //Display Dialog Box
    public static void DisplayDialogBox(string aMessage)
    {
        EditorUtility.DisplayDialog("Warning", aMessage, "OK");
    }


    //Get the selected Game object
    public static GameObject GetSelectedObject(string aWarningMessage)
    {
        GameObject selectedGO = Selection.activeGameObject;
        if (!selectedGO)
        {
            EditorUtility.DisplayDialog("Warning", aWarningMessage, "OK");
            return null;
        }
        else
        {
            return selectedGO;
        }
    }

    public static GameObject GetSelectedObject()
    {
        GameObject selectedGO = Selection.activeGameObject;
        if (!selectedGO)
        {
            return null;
        }
        else
        {
            return selectedGO;
        }
    }
}