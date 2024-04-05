using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> waypointList;

    private void Awake()
    {
        waypointList = new List<Waypoint>();
    }
}
