using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class Edge : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public BezierLineRenderer bezierLineRenderer;
    public BezierSpline spline;

    public Material roadMat, busMat, trainMat;
    
    public void Setup(Vector3 from, Vector3 to)
    {
        spline.Initialize( 2 );
        spline[0].position = from;
        spline[1].position = to;
        spline.ConstructLinearPath();
    }
}

public enum EdgeType
{
    ROAD,
    BUS,
    TRAIN
}
