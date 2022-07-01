using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlaybackManager playbackManager;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private LogHandler logHandler;

    [SerializeField] private GameObject loadingPanel;
    
    private void Awake()
    {
        loadingPanel.SetActive(true);
        mapManager.Setup();
        logHandler.Setup();
    }

    private IEnumerator Start()
    {
        if (Config.Cached)
        {
            playbackManager.CreateCache();
        }

        yield return new WaitForSeconds(3);
        loadingPanel.SetActive(false);
    }
}
