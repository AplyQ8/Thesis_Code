using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool IsWalkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;
    public Node Parent;
    private int _heapIndex;
    
    public int GCost;
    public int HCost;

    public Node(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        IsWalkable = isWalkable;
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
    }

    public int FCost => GCost + HCost;

    public int HeapIndex
    {
        get => _heapIndex;
        set => _heapIndex = value;
    }
    public int CompareTo(Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = HCost.CompareTo(nodeToCompare.HCost);
        }

        return -compare;
    }
}
