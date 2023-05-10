using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//using MathNet.Numerics;

public class PredictiveMovementMachine : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Movement _targetMovement;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private int radius;
    private Vector3 _nextPos;

    void Awake()
    {
        _targetMovement = target.GetComponent<Movement>();
    }
    
    public Vector3 GetPrediction(GameObject target, List<Vector3> dataSet,float[] timeArray, int time)
    {
        float[] xValues = new float[dataSet.Count];
        float[] yValues = new float[dataSet.Count];
        for (int i = 0; i < dataSet.Count; i++)
        {
            xValues[i] = dataSet[i].x;
            yValues[i] = dataSet[i].y;
        }
        float rSquaredX, interceptX, slopeX;
        float rSquared, intercept, slope;
        LinearRegression(xValues, timeArray, out rSquaredX, out interceptX, out slopeX);
        var predictedValueX = (slopeX * time) + interceptX;
        LinearRegression(xValues, yValues, out rSquared, out intercept, out slope);
        var predictValueY = (slope * predictedValueX) + intercept;

        var position = new Vector3(predictedValueX, predictValueY, target.transform.position.z);
        
        return position;
    }

    private void LinearRegression(
        float[] xValues,
        float[] yValues,
        out float rSquared,
        out float intercept,
        out float slope)
    {
        if (xValues.Length != yValues.Length)
        {
            throw new Exception("Input values should be with the same length.");
        }

        double sumOfX = 0;
        double sumOfY = 0;
        double sumOfXSq = 0;
        double sumOfYSq = 0;
        double sumCodeviates = 0;

        for (var i = 0; i < xValues.Length; i++)
        {
            var x = xValues[i];
            var y = yValues[i];
            sumCodeviates += x * y;
            sumOfX += x;
            sumOfY += y;
            sumOfXSq += x * x;
            sumOfYSq += y * y;
        }

        var count = xValues.Length;
        var ssX = sumOfXSq - ((sumOfX * sumOfX) / count);
        var ssY = sumOfYSq - ((sumOfY * sumOfY) / count);

        var rNumerator = (count * sumCodeviates) - (sumOfX * sumOfY);
        var rDenom = (count * sumOfXSq - (sumOfX * sumOfX)) * (count * sumOfYSq - (sumOfY * sumOfY));
        var sCo = sumCodeviates - ((sumOfX * sumOfY) / count);

        var meanX = sumOfX / count;
        var meanY = sumOfY / count;
        var dblR = rNumerator / Math.Sqrt(rDenom);

        rSquared = (float)(dblR * dblR);
        intercept = (float)(meanY - ((sCo / ssX) * meanX));
        slope = (float)(sCo / ssX);
    }

    public Vector2 GetSimplePrediction()
    {
        Vector3 targetPos = target.transform.position;
        Vector3 nextPosition = targetPos;
        for (int i = 10; i > 0; i--)
        {
            nextPosition = (Vector2)targetPos + _targetMovement.moveDir*i;
            _nextPos = nextPosition;
            bool isCollides = Physics2D.Linecast(targetPos, nextPosition, obstacleMask);
            if (!isCollides)
                return nextPosition;
        }
        return nextPosition;
    }

}
