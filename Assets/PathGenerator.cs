using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    string path;

    public void OpenFileExplorer()
    {
        path = EditorUtility.OpenFilePanel("Choose your file(.txt)", "", "txt");
        Debug.Log("log file path: " + path);
    }
}