using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupAlgorithm_StateMachine : MonoBehaviour
{
    public GA_BaseState currentState;

    [Header("Available States")]
    public GA_Idling idling;
    public GA_Chasing chasing;
    public GA_Rounding rounding;

    void Awake()
    {
        idling = gameObject.AddComponent<GA_Idling>();
        chasing = gameObject.AddComponent<GA_Chasing>();
        rounding = gameObject.AddComponent<GA_Rounding>();
    }

    void Start()
    {
        currentState = idling;
        currentState.EnterState(this);
        StartCoroutine(UpdateWithDelay());
    }
    IEnumerator UpdateWithDelay()
    {
        WaitForSeconds delay = new WaitForSeconds(.2f);
        while (true)
        {
            currentState.UpdateState(this);
            yield return delay;
        }
    }

    public void SwitchState(GA_BaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
