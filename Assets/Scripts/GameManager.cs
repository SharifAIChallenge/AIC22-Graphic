using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlaybackManager playbackManager;
    [SerializeField] private MapManager mapManager;
    [SerializeField] private LogHandler logHandler;

    [SerializeField] private GameObject loadingPanel;
    
    private void Awake()
    {
        loadingPanel.SetActive(true);
        
        var reader = new StreamReader(Config.LogFilePath);
        reader.ReadLine();
        mapManager.Setup(reader.ReadLine());
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

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
