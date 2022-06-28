using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameConsole : MonoBehaviour
{
    [SerializeField] private Transform logsParent;
    [SerializeField] private GameObject logPrefab;

    public void PrintLog(string log)
    {
        var c = Instantiate(logPrefab, logsParent);
        c.GetComponentInChildren<TMP_Text>().text = log;
    }

    public void Start()
    {
        PrintLog("hello world");
        PrintLog("hello");
        PrintLog(" world");
        PrintLog(" ");
        PrintLog("helooo");
    }
}
