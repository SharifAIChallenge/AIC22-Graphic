using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityRandomGenerate : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings = new();
    [SerializeField] private Vector2 xBounds;
    [SerializeField] private Vector2 zBounds;

    private List<GameObject> instantiatedBuildings = new();

    public LayerMask layerMask;
    public int count;
    [SerializeField] private Transform buildingParent;
    
    public IEnumerator Generate()
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

            var pos = new Vector3(x, 0, z);
            if(Physics.OverlapBox(pos, Vector3.one * 3, Quaternion.identity, layerMask).Length > 0)
            {
                yield return null;
                continue;
            }
            
            var c = Instantiate(randomBuilding, pos, Quaternion.identity, buildingParent);
            instantiatedBuildings.Add(c);
        }
    }
}
