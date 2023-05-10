using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracesInVisionState : BotBaseState
{
    private FieldOfViewScript _viewScript;
    private float speed;
    public override void EnterState(BotStateMachine botStateMachine)
    {
        StateName = "Traces In Vision";
        _viewScript = GetComponent<FieldOfViewScript>();
        speed = GetComponent<SpeedScript>().speed;
    }

    public override void UpdateAction()
    {
        StopCoroutine(nameof(Moving));
        StartCoroutine(nameof(Moving));
        
    }

    IEnumerator Moving()
    {
        WaitForEndOfFrame delay = new WaitForEndOfFrame();
        while (true)
        {
            try
            {
                //Движение к текущему следу
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    _viewScript.trace.transform.position,
                    speed * Time.deltaTime);
            }
            catch (Exception)
            {
                // ignored
            }
            yield return delay;
        }
    }

    public override void UpdateState(BotStateMachine botStateMachine)
    {
        if (_viewScript.canSeePlayer)
        {
            StopCoroutine(nameof(Moving));
            botStateMachine.SwitchState(botStateMachine.targetInVision);
            
        }
        if (_viewScript.foundingPath)
        {
            StopCoroutine(nameof(Moving));
            botStateMachine.SwitchState(botStateMachine.pathfinding);
        }
        if (!_viewScript.canSeeTraces && !_viewScript.foundingPath)
        {
            StopCoroutine(nameof(Moving));
            _viewScript.AskForPathfinding();
        }
        if (!_viewScript.foundingPath && !_viewScript.canSeePlayer && !_viewScript.canSeeTraces)
        {
            StopCoroutine(nameof(Moving));
            botStateMachine.SwitchState(botStateMachine.idling);
        }
        
    }
    
}
