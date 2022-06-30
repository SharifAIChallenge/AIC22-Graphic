using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlaybackManager playbackManager;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private LogHandler logHandler;

    private void Awake()
    {
        mapManager.Setup();
        logHandler.Setup();
    }

    private void Start()
    {
        if (Config.Cached)
        {
            playbackManager.CreateCache();
        }
    }
}
