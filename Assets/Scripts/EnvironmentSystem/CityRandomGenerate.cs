using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CityRandomGenerate : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings = new();
    [SerializeField] private GameObject groundChunk;
    [SerializeField] private Vector2 xBounds;
    [SerializeField] private Vector2 zBounds;
    [SerializeField] private float yOffset;
    [SerializeField] private float gridSize = 4;
    [SerializeField] private float overlapSize = 3;

    [SerializeField] private Vector2 mapXBounds;
    [SerializeField] private Vector2 mapZBounds;
    
    private List<GameObject> instantiatedBuildings = new();

    public LayerMask layerMask;
    public LayerMask islandAreaMask;
    
    public int count;
    [SerializeField] private Transform buildingParent;

    private void Start()
    {
        //Generate();
        //StartCoroutine(Generate());
    }

    /*public IEnumerator Generate()
    {
        var startTime = Time.time;
        
        while (instantiatedBuildings.Count < count)
        {
            if(startTime + 1 < Time.time)
            {
                break;
            }

            var randomBuilding = buildings[Random.Range(0, buildings.Count)];

            var x = Random.Range(xBounds.x, xBounds.y);
            var z = Random.Range(zBounds.x, zBounds.y);

            var pos = new Vector3(x, yOffset, z);
            if(Physics.OverlapBox(pos, Vector3.one * 5, Quaternion.identity, layerMask).Length > 0)
            {
                yield return null;
                continue;
            }
            
            var c = Instantiate(randomBuilding, pos, randomBuilding.transform.rotation, buildingParent);
            c.transform.localScale /= 500;
            instantiatedBuildings.Add(c);
        }
    }*/

    public void Generate()
    {
        var blockSize = 7;
        
        var xCount = (int) (xBounds.y - xBounds.x) / gridSize;
        var zCount = (int) (zBounds.y - zBounds.x) / gridSize;

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < zCount; j++)
            {
                var x = xBounds.x + i * gridSize;
                var z = zBounds.x + j * gridSize;
                var pos = new Vector3(x, yOffset, z);
                
                if (Physics.OverlapBox(pos, Vector3.one * (overlapSize * 2), Quaternion.identity, islandAreaMask).Length == 0)
                {
                    continue;
                }
                
                Instantiate(groundChunk, pos + Vector3.up * 0.3f, Quaternion.identity, buildingParent);

                if(x < mapXBounds.x || x > mapXBounds.y || z < mapZBounds.x || z > mapZBounds.y)
                {
                    if(i % blockSize == 0 || j % blockSize == 0)
                    {
                        continue;
                    }
                }
                
                if (Physics.OverlapBox(pos, Vector3.one * overlapSize, Quaternion.identity, layerMask).Length > 0)
                {
                    continue;
                }
                
                var randomBuilding = buildings[Random.Range(0, buildings.Count)];
                var c = Instantiate(randomBuilding, pos, randomBuilding.transform.rotation, buildingParent);
                c.transform.localScale /= 500;
                
                var randomRotation = Random.Range(0, 4);
                c.transform.Rotate(0, randomRotation * 90, 0, Space.World);
                
                instantiatedBuildings.Add(c);
            }
        }
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3((mapXBounds.y + mapXBounds.x)/2, 0, (mapZBounds.y + mapZBounds.x)/2) , new Vector3(mapXBounds.y - mapXBounds.x, 1, mapZBounds.y - mapZBounds.x));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3((xBounds.y + xBounds.x)/2, 0, (zBounds.y + zBounds.x)/2), new Vector3(xBounds.y - xBounds.x, 1, zBounds.y - zBounds.x));
    }
    #endif
}
