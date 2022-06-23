using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;
using UnityEngine.Animations;

public class Edge : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public BezierLineRenderer bezierLineRenderer;
    public BezierSpline spline;

    public Material roadMat, busMat, trainMat;

    public bool hasRoad = false, hasBus = false, hasTrain = false;

    public void SetTransportationMethod(EdgeType type)
    {
        switch (type)
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
        }
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

        spline.autoConstructMode = SplineAutoConstructMode.Linear;
        spline.ConstructLinearPath();
    }
}

public enum EdgeType
{
    ROAD,
    BUS,
    TRAIN
}