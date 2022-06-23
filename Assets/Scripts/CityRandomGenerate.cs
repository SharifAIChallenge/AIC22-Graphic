using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityRandomGenerate : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings = new();
    [SerializeField] private Vector2 xBounds;
    [SerializeField] private Vector2 zBounds;

    private List<GameObject> instantiatedBuildings = new();
    
    public void Generate()
    {
        while (instantiatedBuildings.Count < 5)
        {
            var x = Random.Range(xBounds.x, xBounds.y);
            var z = Random.Range(zBounds.x, zBounds.y);

            var pos = new Vector3(x, 0, z);
            var c = Instantiate(buildings[0], pos, Quaternion.identity);
            instantiatedBuildings.Add(c);
        }
    }
}
