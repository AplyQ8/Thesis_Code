using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FieldOfViewScript : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float angle;
    
    private LayerMask _targetMask;
    private LayerMask _obstacleMask;
    private LayerMask _traceMask;

    public Vector3 positionToMoveWhilePathfinding;
    public GameObject target;
    public GameObject trace;
    
    public bool canSeePlayer;
    public bool canSeeTraces;
    public bool foundingPath;

    [SerializeField] private BotStateMachine botStateMachine;
    
    public event Action<GameObject> EnemyDetection = delegate {  };
    public event Action<GameObject> LostFromVision = delegate {  };
    public event Action<GameObject> AskForPathFinding = delegate {  };
    public event Action<GameObject> AskForCoordinates = delegate { };
    public event Action<Vector3> RecordTrace = delegate {  };

    private Collider2D[] _traceColliderArray;
    private Collider2D _traceCollider2D;
    [SerializeField] private List<Collider2D> tracesInVision;

    private void Awake()
    {
        tracesInVision = new List<Collider2D>();
        botStateMachine = GetComponent<BotStateMachine>();
    }
    public void SetInfo(float rad, float angleOfView, LayerMask targets, LayerMask obstacles,LayerMask traces, GameObject targetObject)
    {
        radius = rad;
        angle = angleOfView;
        
        _targetMask = targets;
        _obstacleMask = obstacles;
        _traceMask = traces;
        
        target = targetObject;
        StartCoroutine(FOVCheck_Target());
        StartCoroutine(FOVCheck_Traces());
    }
    private IEnumerator FOVCheck_Target()
    {
        //WaitForSeconds wait = new WaitForSeconds(0.2f);
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (true)
        {
            FOV_Target();
            yield return wait;
        }
    }

    private IEnumerator FOVCheck_Traces()
    {
        //WaitForSeconds wait = new WaitForSeconds(0.2f);
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (true)
        {
            //FOV_Traces();
            TracesInView();
            yield return wait;
        }
    }
    
    private void FOV_Target()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            _targetMask
            );
        if (rangeCheck.Length > 0)
        {
            Transform targetTransform = rangeCheck[0].transform;
            Vector2 directionToTarget = (targetTransform.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, targetTransform.position);
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleMask))
                {
                    canSeePlayer = true;
                    EnemyDetection.Invoke(gameObject);
                    canSeeTraces = false;
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
            
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void TracesInView()
    {
        tracesInVision.Clear();
        if (canSeePlayer) return;
        Collider2D[] arrayOfTraceColliders = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            _traceMask
        );
        foreach (var traceCollider in arrayOfTraceColliders)
        {
            Vector2 directionToTrace = (traceCollider.transform.position - transform.position).normalized;
            if (Vector2.Angle(transform.up, directionToTrace) < angle / 2)
            {
                float distanceToTrace = Vector2.Distance(transform.position, traceCollider.transform.position);
                if (!Physics2D.Raycast(transform.position, directionToTrace, distanceToTrace, _obstacleMask))
                {
                    tracesInVision.Add(traceCollider);
                    RecordTrace.Invoke(traceCollider.transform.position);
                }
            }
        }
        if (tracesInVision.Count == 0)
        {
            canSeeTraces = false;
            return;
        }
        canSeeTraces = true;
        trace = CalculateTheNearestTrace(tracesInVision, target.transform.position).gameObject;
        
    }

    public void AskForPathfinding()
    {
        AskForPathFinding.Invoke(gameObject);
    }

    public void AskForPositionToMove()
    {
        AskForCoordinates.Invoke(gameObject);
    }
    void Update()
    {
        if (canSeePlayer || canSeeTraces)
            foundingPath = false;
        // if (foundingPath)
        // {
        //     canSeePlayer = false;
        //     canSeeTraces = false;
        // }
        
        if(!canSeePlayer && !canSeeTraces)
            LostFromVision.Invoke(gameObject);
        
    }
    private void OnDrawGizmos()
    {
        
        UnityEditor.Handles.color = Color.white;
        //UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up,360 , radius);
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector3 angle1 = DirectionFromAngle(-transform.eulerAngles.z, -angle/2);
        Vector3 angle2 = DirectionFromAngle(-transform.eulerAngles.z, angle/2);

        if (angle < 360)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + angle1*radius);
            Gizmos.DrawLine(transform.position, transform.position + angle2*radius);
        }

        if (canSeePlayer)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }

        if (canSeeTraces)
        {
            Gizmos.color = Color.cyan;
            try
            {
                Gizmos.DrawLine(transform.position, trace.transform.position);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
    
    private Vector2 DirectionFromAngle(float eulerY, float angleInDeg)
    {
        angleInDeg += eulerY;
        return new Vector2(Mathf.Sin(angleInDeg*Mathf.Deg2Rad), MathF.Cos(angleInDeg*Mathf.Deg2Rad));
    }
    private Transform SortColliderArray(Collider2D[] array, Vector3 targetPos)
    {
        Transform objectToReturn = array[0].transform;
        float distance = float.MaxValue;
        foreach (var collider2D1 in array)
        {
            var localDist = Vector3.Distance(collider2D1.transform.position, targetPos);
            if (localDist < distance)
            {
                distance = localDist;
                objectToReturn = collider2D1.transform;
            }
        }
        return objectToReturn;

    }
    private Transform CalculateTheNearestTrace(List<Collider2D> collider2Ds, Vector3 targetPos)
    {
        Transform objectToReturn = collider2Ds[0].transform;
        float distance = float.MaxValue;
        foreach (var collider2D1 in collider2Ds)
        {
            var localDist = Vector3.Distance(collider2D1.transform.position, targetPos);
            if (localDist < distance)
            {
                distance = localDist;
                objectToReturn = collider2D1.transform;
            }
        }

        return objectToReturn;
    }
}
