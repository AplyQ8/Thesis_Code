using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInVisionState : BotBaseState
{
    private FieldOfViewScript _viewScript;
    private float speed;
    public override void EnterState(BotStateMachine botStateMachine)
    {
        StateName = "Target In Vision";
        _viewScript = GetComponent<FieldOfViewScript>();
        speed = GetComponent<SpeedScript>().speed;
    }

    public override void UpdateAction()
    {
        //Движение в направление к цели
        StopCoroutine(nameof(Moving));
        StartCoroutine(nameof(Moving));
        
    }

    public override void UpdateState(BotStateMachine botStateMachine)
    {
        if (_viewScript.canSeeTraces)
        {
            StopCoroutine(nameof(Moving));
            botStateMachine.SwitchState(botStateMachine.tracesInVision);
        }
        if (_viewScript.foundingPath)
        {
            StopCoroutine(nameof(Moving));
            botStateMachine.SwitchState(botStateMachine.pathfinding);
        }
    }

    IEnumerator Moving()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        while (true)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _viewScript.target.transform.position,
                speed * Time.deltaTime);
            yield return delay;
        }
    }
}
