using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GA_Console : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject groupAlg;
    [SerializeField] private GroupAlgorithm_StateMachine stateMachine;

    void Start()
    {
        stateMachine = groupAlg.GetComponent<GroupAlgorithm_StateMachine>();
    }
    
    // Update is called once per frame
    void Update()
    { 
        text.text = stateMachine.currentState.StateName;
    }
}
