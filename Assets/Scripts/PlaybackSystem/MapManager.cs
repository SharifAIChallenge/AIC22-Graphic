using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GraphCreator;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform mapParent;
    
    [SerializeField] private CityRandomGenerate cityGenerator;

    [SerializeField] private string mapName;
    [SerializeField] private string relativePath;

    [SerializeField] private Graph mapPrefab;

    public Graph Map;

    [SerializeField] private bool load;

    public void Setup()
    {
        LoadMapFromFile();
        StartCoroutine(CreateCity());
    }

    private IEnumerator CreateCity()
    {
        yield return new WaitForEndOfFrame();
        cityGenerator.Generate();        
    }

    private void LoadMapFromFile()
    {
        string path = Config.GamePath + relativePath;
        string json = File.ReadAllText(path);
        var parsed = JsonUtility.FromJson<GraphJsonData>(json);

        Map = Instantiate(mapPrefab, mapParent);
        
        foreach (var node in parsed.nodes)
        {
            Map.AddNode(node.id, node.position);
        }

        foreach (var edge in parsed.edges)
        {
            Map.AddEdge(edge.node1Id, edge.node2Id);
        }
    }
    /*private void Start()
    {
        /*var locations = Addressables.LoadResourceLocationsAsync(address);
        yield return locations;
        if (locations.Status != AsyncOperationStatus.Succeeded) {
            yield break;
        }#1#

        /*var op = Addressables.LoadAssetAsync<GameObject>(mapName);
        yield return op;

        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            mapPrefab = op.Result.GetComponent<Graph>();
            Map = Instantiate(mapPrefab, mapParent);
            StartCoroutine(cityGenerator.Generate());
        }#1#
        
        Setup();
    }

    private async void Setup()
    {
        /*var locations = Addressables.LoadResourceLocationsAsync(address);
        yield return locations;
        if (locations.Status != AsyncOperationStatus.Succeeded) {
            yield break;
        }#1#

        if (load)
        {

            //var path = Application.dataPath + relativePath;
            var path = Config.GamePath + relativePath;

            var operation = Addressables.LoadContentCatalogAsync(path);
            var locator = await operation.Task;

            Addressables.ClearResourceLocators();
            Addressables.AddResourceLocator(locator);

            var op = Addressables.LoadAssetAsync<GameObject>(mapName);
            var prefab = await op.Task;

            mapPrefab = prefab.GetComponent<Graph>();
        }

        Map = Instantiate(mapPrefab, mapParent);
        StartCoroutine(cityGenerator.Generate());
    }*/
}

[Serializable]
public class GraphJsonData{
    public List<NodeJsonData> nodes = new();
    public List<EdgeJsonData> edges = new();
}
    
[Serializable]
public class NodeJsonData{
    public int id;
    public Vector3 position;
}
[Serializable]
public class EdgeJsonData{
    public int node1Id;
    public int node2Id;
}
