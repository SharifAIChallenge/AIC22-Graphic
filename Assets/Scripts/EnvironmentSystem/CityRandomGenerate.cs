using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CityRandomGenerate : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings = new();
    [SerializeField] private Vector2 xBounds;
    [SerializeField] private Vector2 zBounds;
    [SerializeField] private float yOffset;
    [SerializeField] private float gridSize = 4;
    [SerializeField] private float overlapSize = 3;

    private List<GameObject> instantiatedBuildings = new();

    public LayerMask layerMask;
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
        var xCount = (int) (xBounds.y - xBounds.x) / gridSize;
        var zCount = (int) (zBounds.y - zBounds.x) / gridSize;

        for (int i = 0; i < xCount; i++)
        {
            for (int j = 0; j < zCount; j++)
            {
                var x = xBounds.x + i * gridSize;
                var z = zBounds.x + j * gridSize;
                var pos = new Vector3(x, yOffset, z);
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
}
