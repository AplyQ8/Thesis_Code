using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlledObjectPrefab : MonoBehaviour
{
    [SerializeField] private BotStateMachine controlledObject;
    [SerializeField] private GameObject color;
    [SerializeField] private TMP_Text state;

    public void SetInfo(GameObject _controlledObject, Color _color)
    {
        controlledObject = _controlledObject.GetComponent<BotStateMachine>();
        color.GetComponent<Image>().color = _color;
    }

    private void Update()
    {
        state.text = controlledObject.currentState.StateName;
    }
}
