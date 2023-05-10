using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathRequest> _pathRequestsQueue = new Queue<PathRequest>();
    private PathRequest _currentPathRequest;

    private static PathRequestManager _instance;
    private PathfindingEngine _engine;
    private bool _isProcessingPath;

    private void Awake()
    {
        _instance = this;
        _engine = GetComponent<PathfindingEngine>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        _instance._pathRequestsQueue.Enqueue(newRequest);
        _instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if (!_isProcessingPath && _pathRequestsQueue.Count > 0)
        {
            _currentPathRequest = _pathRequestsQueue.Dequeue();
            _isProcessingPath = true;
            _engine.StartFindPath(_currentPathRequest.PathStart, _currentPathRequest.PathEnd);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool isSuccess)
    {
        _currentPathRequest.Callback(path, isSuccess);
        _isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 PathStart;
        public Vector3 PathEnd;
        public Action<Vector3[], bool> Callback;

        public PathRequest(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
        {
            PathStart = pathStart;
            PathEnd = pathEnd;
            Callback = callback;
        }
    }
    
}
