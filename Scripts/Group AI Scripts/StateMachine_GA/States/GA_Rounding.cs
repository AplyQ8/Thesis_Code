using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GA_Rounding : GA_BaseState
{
    [SerializeField] private GroupManagingScript groupManaging;
    public override void EnterState(GroupAlgorithm_StateMachine stateMachine)
    {
        StateName = "Rounding";
        groupManaging = GetComponent<GroupManagingScript>();
    }

    public override void UpdateState(GroupAlgorithm_StateMachine stateMachine)
    {
        
    }
}
