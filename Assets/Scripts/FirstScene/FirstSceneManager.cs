using System;
using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstSceneManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pathText;
    [SerializeField] private Button StartButton;

    private void Awake()
    {
        StartButton.interactable = false;
    }

    public void ChooseFolder()
    {
        /*var path = StandaloneFileBrowser.OpenFolderPanel("Choose a folder", "", false);
        
        if(path.Length == 0)
            return;
        
        if(string.IsNullOrEmpty(path[0]))
            return;

        print(path[0]);
        Config.GamePath = path[0];
        pathText.text = path[0];
        StartButton.interactable = true;*/
        
        var path = StandaloneFileBrowser.OpenFilePanel("Choose log file", "", "", false);
        
        if(path.Length == 0)
            return;
        
        if(string.IsNullOrEmpty(path[0]))
            return;

        print(path[0]);
        Config.LogFilePath = path[0];
        pathText.text = path[0];
        StartButton.interactable = true;
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}