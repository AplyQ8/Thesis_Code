using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Класс-абстракция состояний группового алгоритма
public abstract class GA_BaseState : MonoBehaviour
{
    public string StateName { get; protected set; }
    public abstract void EnterState(GroupAlgorithm_StateMachine stateMachine);
    public abstract void UpdateState(GroupAlgorithm_StateMachine stateMachine);
}
