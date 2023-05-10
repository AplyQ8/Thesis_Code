using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathfindingState : BotBaseState
{
    private FieldOfViewScript _viewScript;
    private PathfindingEngine _aStar;
    private Vector3[] _path;
    private int _targetIndex;
    private float speed;
    private bool _isActive;
    public override void EnterState(BotStateMachine botStateMachine)
    {
        _isActive = true;
        StateName = "Pathfinding";
        _viewScript = GetComponent<FieldOfViewScript>();
        _aStar = GameObject.Find("A*").GetComponent<PathfindingEngine>();
        speed = GetComponent<SpeedScript>().speed;
    }

    public override void UpdateAction()
    {
        _viewScript.AskForPositionToMove();
        //_aStar.FindPath(transform.position, _viewScript.positionToMoveWhilePathfinding);
        PathRequestManager.RequestPath(transform.position, _viewScript.positionToMoveWhilePathfinding, OnPathFound);

    }

    public override void UpdateState(BotStateMachine botStateMachine)
    {
        if (_viewScript.canSeePlayer)
        {
            _isActive = false;
            StopCoroutine(nameof(FollowPath));
            botStateMachine.SwitchState(botStateMachine.targetInVision);
        }

        // if (_viewScript.canSeeTraces)
        // {
        //     StopCoroutine(nameof(FollowPath));
        //     botStateMachine.SwitchState(botStateMachine.tracesInVision);
        // }

        if (!_viewScript.foundingPath)
        {
            _isActive = false;
            botStateMachine.SwitchState(botStateMachine.idling);
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;
            StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
        }
    }

    IEnumerator FollowPath()
    {
        try
        {
            Vector3 currentWaypoints = _path[0];
        }
        catch (Exception)
        {
            yield break;
        }
        Vector3 currentWaypoint = _path[0];
        
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                _targetIndex++;
                if (_targetIndex >= _path.Length)
                {
                    yield break;
                }

                currentWaypoint = _path[_targetIndex];
            }

            transform.position = Vector3.MoveTowards(
                transform.position, 
                currentWaypoint, 
                speed * Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (_path == null || !_isActive)
        {
            Gizmos.DrawCube(transform.position, Vector3.zero);
            return;
        }
        for (int i = _targetIndex; i < _path.Length; i++)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(_path[i], Vector3.one);
            if (i == _targetIndex)
            {
                Gizmos.DrawLine(transform.position, _path[i]);
            }
            else
            {
                Gizmos.DrawLine(_path[i-1], _path[i]);
            }
        }
    }
}
