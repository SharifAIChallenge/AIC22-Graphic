using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using GraphCreator;
using UnityEngine;
using UnityEngine.Animations;

public class Edge : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public BezierSpline spline;

    public Material roadMat, busMat, trainMat;

    //public bool hasRoad = false, hasBus = false, hasTrain = false;

    public EdgeType edgeType;
    
    public void SetTransportationMethod(EdgeType type)
    {
        edgeType = type;
        switch (type)
        {
            case EdgeType.ROAD:
                meshRenderer.material = roadMat;
                meshRenderer.transform.localPosition = Vector3.up * 0.02f;
                break;
            case EdgeType.BUS:
                meshRenderer.material = busMat;
                meshRenderer.transform.localPosition = Vector3.up * 0.01f;
                break;
            case EdgeType.TRAIN:
                meshRenderer.material = trainMat;
                meshRenderer.transform.localPosition = Vector3.zero;
                break;
        }
        
        /*switch (type)
        {
            case EdgeType.ROAD:
                hasRoad = true;
                break;
            case EdgeType.BUS:
                hasBus = true;
                break;
            case EdgeType.TRAIN:
                hasTrain = true;
                break;
        }*/
    }

    public void Setup(Transform from, Transform to, EdgeType type)
    {
        SetTransportationMethod(type);
        spline.Initialize(2);
        spline[0].position = from.position;
        spline[1].position = to.position;

        var c = spline[0].gameObject.AddComponent<PositionConstraint>();
        var s = new ConstraintSource {sourceTransform = from, weight = 1};
        c.AddSource(s);
        c.constraintActive = true;

        c = spline[1].gameObject.AddComponent<PositionConstraint>();
        s = new ConstraintSource {sourceTransform = to, weight = 1};
        c.AddSource(s);
        c.constraintActive = true;

        spline.autoCalculateNormals = true;
        //spline.AutoCalculateNormals(0f,10, false);
        spline.autoConstructMode = SplineAutoConstructMode.Linear;
        spline.ConstructLinearPath();
    }

    public Tuple<string, int> GetPathsInformation(int currentId, MyTuple edgesKey,
        float roadPrice, float busPrice, float trainPrice)
    {
        string res = "";
        int i = 0;
        if (edgeType == EdgeType.ROAD)
        {
            res += "    - id: " + currentId + "\n" +
                   "      firstNodeId: " + edgesKey.Item1 + "\n" +
                   "      secondNodeId: " + edgesKey.Item2 + "\n" +
                   "      price: " + roadPrice + "\n";
            i++;
        }else if (edgeType == EdgeType.BUS)
        {
            int index = currentId + i;
            res += "    - id: " + index + "\n" +
                   "      firstNodeId: " + edgesKey.Item1 + "\n" +
                   "      secondNodeId: " + edgesKey.Item2 + "\n" +
                   "      price: " + busPrice + "\n";
            i++;
        }else if (edgeType == EdgeType.TRAIN)
        {
            int index = currentId + i;
            res += "    - id: " + index + "\n" +
                   "      firstNodeId: " + edgesKey.Item1 + "\n" +
                   "      secondNodeId: " + edgesKey.Item2 + "\n" +
                   "      price: " + trainPrice + "\n";
            i++;
        }

        return Tuple.Create(res, i + currentId);
    }
}

public enum EdgeType
{
    ROAD,
    BUS,
    TRAIN
}