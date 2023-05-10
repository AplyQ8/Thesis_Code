using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Состояние покоя
public class GA_Idling : GA_BaseState
{
    [SerializeField] private GroupManagingScript groupManaging;
    public override void EnterState(GroupAlgorithm_StateMachine stateMachine)
    {
        StateName = "Idling";
        groupManaging = GetComponent<GroupManagingScript>();
        //Перевод всех П.О. в состояние покоя
        foreach (var item in groupManaging.controlledObjects)
        {
            item.GetComponent<FieldOfViewScript>().foundingPath = false;
            item.GetComponent<FieldOfViewScript>().canSeeTraces = false;
            item.GetComponent<FieldOfViewScript>().canSeePlayer = false;
        }
    }

    public override void UpdateState(GroupAlgorithm_StateMachine stateMachine)
    {
        //если один из П.О. обнаружил цель, то переход в состояние преследования
        if(groupManaging.enemySpotted)
            stateMachine.SwitchState(stateMachine.chasing);
    }
}
