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

    public Road GetRandomRoad()
    {
        System.Random random = new System.Random();
        return roads[random.Next(roads.Count)];
    }
}
