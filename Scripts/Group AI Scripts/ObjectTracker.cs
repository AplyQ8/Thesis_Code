using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTracker : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject targetIndicator;
    [SerializeField] private Vector3 v3AverageVelocity;
    [SerializeField] private Vector3 v3AverageAcceleration;

    [SerializeField] private Vector3 previousVelocity;
    [SerializeField] private Vector3 previousAcceleration;
    [SerializeField] private Vector3 previousPosition;

    private void LateUpdate()
    {
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        yield return new WaitForEndOfFrame();

        Vector3 v3Velocity = (target.transform.position - previousPosition) / Time.deltaTime;
        Vector3 v3Acceleration = v3Velocity - previousVelocity;

        v3AverageVelocity = v3Velocity;
        v3AverageAcceleration = v3Acceleration;

        GetProjectedPosition(10);

        previousPosition = target.transform.position;
        previousVelocity = v3Velocity;
        previousAcceleration = v3Acceleration;
    }

    private Vector3 GetProjectedPosition(float ftime)
    {
        Vector3 v3Ret = target.transform.position + (v3AverageVelocity * (Time.deltaTime * (ftime / Time.deltaTime))) +
                        (v3AverageAcceleration * (0.5f * Time.deltaTime * (float)Math.Pow(ftime / Time.deltaTime, 2)));
        targetIndicator.transform.position = v3Ret;
        return v3Ret;
    }
}
