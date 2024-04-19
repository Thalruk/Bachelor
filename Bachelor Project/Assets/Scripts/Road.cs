using System;
using UnityEngine;

[Serializable]
public class Road
{
    [SerializeField] public Waypoint to;
    [SerializeField] private float length;
    [SerializeField] private int index;

    public Waypoint GetWaypoint()
    {
        return to;
    }

    public void SetLength(float length)
    {
        this.length = length;
    }

    public float GetLength()
    {
        return length;
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }
    public int GetIndex()
    {
        return index;
    }


}
