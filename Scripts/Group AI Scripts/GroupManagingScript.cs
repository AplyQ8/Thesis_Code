using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupManagingScript : MonoBehaviour
{
    [SerializeField] private PredictiveMovementMachine _machine;
    [SerializeField] private GameObject target;
    public List<GameObject> controlledObjects;
    
    //Характеристики следа
    [Header("Trace Info")]
    [SerializeField] private GameObject tracePrefab;
    [SerializeField] private float traceLifeTime;
    
    //характеристики поля зрения подконтрольных объектов
    [Header("Enemy Vision Info")]
    [SerializeField] private float viewRadius;
    [SerializeField] [Range(0, 360)] private float viewAngle;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask traceMask;
    
    //Характеристики отладочной консоли (опционально)
    [Header("Console Info")]
    [SerializeField] private bool _consoleActive;
    [SerializeField] private GameObject _console;
    [SerializeField] private GameObject _consoleList;
    [SerializeField] private GameObject _consoleControlledObjectPrefab;
    

    public bool enemySpotted;
    //Список подконтрольных объектов, указывающий на то, видят они цель или нет
    [SerializeField] private Dictionary<GameObject, bool> _coSpotted = new Dictionary<GameObject, bool>();
    
    
    
    [SerializeField] private List<Vector3> targetsPositions = new List<Vector3>();
    [SerializeField] private int time = 0;
    [SerializeField] private List<float> _timeArray = new List<float>();

    private void Awake()
    {
        StartCoroutine(nameof(Timer));
        _machine = GetComponent<PredictiveMovementMachine>();
        foreach (var controlledObject in controlledObjects)
        {
            _coSpotted.Add(controlledObject, false);
        }
        if(!_consoleActive)
            _console.SetActive(false);
        if (target.tag.Equals("Untagged"))
            throw new Exception("Target should be with a tag");
        try
        {
            tracePrefab.GetComponent<TraceScript>().lifeTime = traceLifeTime;
            target.AddComponent<TraceMaker>().tracePrefab = tracePrefab;
            target.GetComponent<TraceMaker>().traceKeeper = new GameObject("Traces");
            foreach (var controlledObject in controlledObjects)
            {
                //Установка нужных характеристик подконтрольным объектам
                controlledObject.AddComponent<Rigidbody2D>().gravityScale = 0;
                controlledObject.AddComponent<SpeedScript>().speed = target.GetComponent<Movement>().speed * 0.75f;
                controlledObject.AddComponent<CircleCollider2D>();
                controlledObject.AddComponent<FieldOfViewScript>().SetInfo(viewRadius, viewAngle, targetMask, obstacleMask,traceMask, target);
                controlledObject.AddComponent<BotStateMachine>();
                controlledObject.GetComponent<FieldOfViewScript>().EnemyDetection += Detection;
                controlledObject.GetComponent<FieldOfViewScript>().LostFromVision += LostFromVision;
                controlledObject.GetComponent<FieldOfViewScript>().AskForPathFinding += AnswerForPathfinding;
                controlledObject.GetComponent<FieldOfViewScript>().AskForCoordinates += AnswerForPositionToMove;
                controlledObject.GetComponent<FieldOfViewScript>().RecordTrace += RecordTrace;
                
                if (_consoleActive)
                {
                    GameObject prefab = Instantiate(_consoleControlledObjectPrefab, _consoleList.transform, true);
                    prefab.GetComponent<ControlledObjectPrefab>().SetInfo(controlledObject, controlledObject.GetComponent<SpriteRenderer>().color);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Something went wrong: {ex}");
        }
    }
    
    //Если П.О. обнаружил цель -> значение в списке меняется
    private void Detection(GameObject coWhoSpotted)
    {
        _coSpotted[coWhoSpotted] = true;
    }
    
    //Если П.О. потерял цель -> значение в списке меняется
    private void LostFromVision(GameObject coLost)
    {
        _coSpotted[coLost] = false;
    }
    
    //Функция, отвечающая, есть ли цель в поле зрения какого либо из подконтрольных объектов
    private void AnswerForPathfinding(GameObject controlledObject)
    {
        FieldOfViewScript view = controlledObject.GetComponent<FieldOfViewScript>();
        if (view.canSeePlayer || view.canSeeTraces)
            return;
        
        view.foundingPath = enemySpotted;
        _coSpotted[controlledObject] = enemySpotted;
    }

    private void AnswerForPositionToMove(GameObject controlledObject)
    {
        // if (targetsPositions.Count != 10)
        // {
        //     controlledObject.GetComponent<FieldOfViewScript>().positionToMoveWhilePathfinding =
        //         target.transform.position;
        //     return;
        // }
        // controlledObject.GetComponent<FieldOfViewScript>().positionToMoveWhilePathfinding =
        //     _machine.GetPrediction(target, targetsPositions,_timeArray.ToArray(), time+1);
        controlledObject.GetComponent<FieldOfViewScript>().positionToMoveWhilePathfinding = _machine.GetSimplePrediction();
    }

    private void RecordTrace(Vector3 position)
    {
        if(targetsPositions.Count == _timeArray.Count)
            targetsPositions.RemoveAt(0);
        targetsPositions.Add(position);
    }
    
    //Пока цель в поле зрения хотя бы одного П.О., то цель в поле зрения Г.А.
    void Update()
    {
        foreach (var item in _coSpotted)
        {
            if (item.Value)
            {
                enemySpotted = true;
                return;
            }
            enemySpotted = false;
        }
    }

    private IEnumerator Timer()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        while (true)
        {
            time++;
            _timeArray.Add((float)time);
            if(_timeArray.Count >10)
                _timeArray.RemoveAt(0);
            yield return delay;
        }
    }
}
