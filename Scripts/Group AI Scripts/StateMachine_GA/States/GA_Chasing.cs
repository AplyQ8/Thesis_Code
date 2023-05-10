using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Состояние преследования
public class GA_Chasing : GA_BaseState
{
    [SerializeField] private GroupManagingScript groupManaging;
    
    //Фугкция вхождения в состояние
    public override void EnterState(GroupAlgorithm_StateMachine stateMachine)
    {
        StateName = "Chasing";
        groupManaging = GetComponent<GroupManagingScript>();
        //Оповещение всех П.О-ов о нахождении цели
        foreach (var item in groupManaging.controlledObjects)
        {
            FieldOfViewScript script = item.GetComponent<FieldOfViewScript>();
            if(!script.canSeePlayer || !script.canSeeTraces)
                script.foundingPath = true;
        }
    }
    
    public override void UpdateState(GroupAlgorithm_StateMachine stateMachine)
    {
        //Если ни один из П.О. не видит цель - переход в состояние покоя
        if(!groupManaging.enemySpotted)
            stateMachine.SwitchState(stateMachine.idling);
    }
}
