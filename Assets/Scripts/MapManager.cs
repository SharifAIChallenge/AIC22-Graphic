using System.Collections;
using System.Collections.Generic;
using GraphCreator;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform mapParent;
    
    [SerializeField] private CityRandomGenerate cityGenerator;

    [SerializeField] private string mapName;
    [SerializeField] private string relativePath;

    private Graph mapPrefab;

    public Graph Map;
    
    //[SerializeField] private AssetReferenceGameObject mapPrefabReference;

    private void Start()
    {
        /*var locations = Addressables.LoadResourceLocationsAsync(address);
        yield return locations;
        if (locations.Status != AsyncOperationStatus.Succeeded) {
            yield break;
        }*/

        /*var op = Addressables.LoadAssetAsync<GameObject>(mapName);
        yield return op;

        if (op.Status == AsyncOperationStatus.Succeeded)
        {
            mapPrefab = op.Result.GetComponent<Graph>();
            Map = Instantiate(mapPrefab, mapParent);
            StartCoroutine(cityGenerator.Generate());
        }*/
        
        Setup();
    }

    private async void Setup()
    {
        /*var locations = Addressables.LoadResourceLocationsAsync(address);
        yield return locations;
        if (locations.Status != AsyncOperationStatus.Succeeded) {
            yield break;
        }*/
        
        //var path = Application.dataPath + relativePath;
        var path = Config.GamePath + relativePath;

        var operation = Addressables.LoadContentCatalogAsync(path);
        var locator = await operation.Task;
        
        Addressables.ClearResourceLocators();
        Addressables.AddResourceLocator(locator);
        
        var op = Addressables.LoadAssetAsync<GameObject>(mapName);
        var prefab = await op.Task;
        
        mapPrefab = prefab.GetComponent<Graph>();
        Map = Instantiate(mapPrefab, mapParent);
        StartCoroutine(cityGenerator.Generate());
    }
}
