using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool startPoint;

    [SerializeField] private List<Road> roads;

    public List<Road> GetRoads()
    {
        return roads;
    }
}
