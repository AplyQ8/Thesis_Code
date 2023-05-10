using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceScript : MonoBehaviour
{
    public float lifeTime;
    private void Awake()
    {
        StartCoroutine(LifeTimer());
    }
    IEnumerator LifeTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(lifeTime);
            Destroy(gameObject);
        }
    }
}
