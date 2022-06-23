using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityRandomGenerate : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildings = new();
    [SerializeField] private Vector2 xBounds;
    [SerializeField] private Vector2 yBounds;

    private List<GameObject> instantiatedBuildings = new();
    
    public void Generate()
    {
        while (instantiatedBuildings.Count < 5)
        {
            var x = Random.Range(xBounds.x, xBounds.y);
            var y = Random.Range(yBounds.x, yBounds.y);

            var pos = new Vector3(x, y, 0);
            var c = Instantiate(buildings[0], pos, Quaternion.identity);
            instantiatedBuildings.Add(c);
        }
    }
}
