using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotStateMachine: MonoBehaviour
{
    public BotBaseState currentState;
    
    //Список возможных состояний
    [Header("Available states")]
    public IdlingState idling;
    public PathfindingState pathfinding;
    public TargetInVisionState targetInVision;
    public TracesInVisionState tracesInVision;

    void Awake()
    {
        idling = gameObject.AddComponent<IdlingState>();
        pathfinding = gameObject.AddComponent<PathfindingState>();
        targetInVision = gameObject.AddComponent<TargetInVisionState>();
        tracesInVision = gameObject.AddComponent<TracesInVisionState>();
    }
    void Start()
    {
        currentState = idling;
        currentState.EnterState(this);
        StartCoroutine(UpdateStateWithDelay());
        StartCoroutine(UpdateActionWithDelay());
    }
    
    //Проверка на переход в состояние
    IEnumerator UpdateStateWithDelay()
    {
        WaitForSeconds delay = new WaitForSeconds(.2f);
        while (true)
        {
            currentState.UpdateState(this);
            yield return delay;
        }
    }
    //Выполнение действия состояния
    IEnumerator UpdateActionWithDelay()
    {
        //WaitForEndOfFrame delay = new WaitForEndOfFrame();
        WaitForSeconds delay = new WaitForSeconds(0.01f);

        while (true)
        {
            currentState.UpdateAction();
            yield return delay;
        }
    }
    //Переключатель состояния
    public void SwitchState(BotBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    
}
