using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Состояние покоя
public class IdlingState : BotBaseState
{
    private FieldOfViewScript _viewScript;
    public override void EnterState(BotStateMachine botStateMachine)
    {
        StateName = "Idling";
        _viewScript = GetComponent<FieldOfViewScript>();
    }

    public override void UpdateAction()
    {
        //ничего не происходит
    }

    public override void UpdateState(BotStateMachine botStateMachine)
    {
        if(_viewScript.canSeePlayer)
            botStateMachine.SwitchState(botStateMachine.targetInVision);
        if(_viewScript.canSeeTraces)
            botStateMachine.SwitchState(botStateMachine.tracesInVision);
        if(_viewScript.foundingPath)
            botStateMachine.SwitchState(botStateMachine.pathfinding);
        
    }
    
}
