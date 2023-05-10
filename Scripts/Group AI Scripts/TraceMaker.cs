using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceMaker : MonoBehaviour
{
    public GameObject tracePrefab;
    public GameObject traceKeeper;
    
    public void Start()
    {
        StartCoroutine(TraceLeaving());
    }

    IEnumerator TraceLeaving()
    {
        while (true)
        {
            var trace = Instantiate(tracePrefab, transform.position, Quaternion.identity);
            trace.transform.SetParent(traceKeeper.transform);
            yield return new WaitForSeconds(.1f);
        }
    }
}
