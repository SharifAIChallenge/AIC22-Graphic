using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> trees;
    [SerializeField] private float minRadius = 100;
    [SerializeField] private float maxRadius = 200;
    [SerializeField] private int count = 1000;
    
    [SerializeField] private float minScale = 3;
    [SerializeField] private float maxScale = 10;

    private void Start()
    {
        Build();
        
    }

    void Build()
    {
        for (int i = 0; i < count; i++)
        {
            var randomDirection = (Random.insideUnitCircle).normalized;
            var randomDistance = Random.Range(minRadius, maxRadius);
            var point = randomDirection * randomDistance;
            var position = new Vector3(point.x, 0, point.y);
            
            var rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            var o = Instantiate(trees[Random.Range(0, trees.Count)], position, rotation, transform);

            var x = Mathf.InverseLerp(minRadius, maxRadius, randomDistance);
            
            o.transform.localScale = Vector3.one * (Mathf.Lerp(minScale, maxScale, x) + Random.Range(-1, 1));
        }
    }
}
