using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> waypoints;
    [SerializeField] List<GameObject> roads;

    [SerializeField] GameObject road;

    private void Awake()
    {
        waypoints = new List<Waypoint>();
        roads = new List<GameObject>();
    }

    private void OnValidate()
    {
        Debug.Log("something changed");
        foreach (GameObject road in roads)
        {
            Spline spline = road.GetComponent<Spline>();
            spline.SetKnot(0, new BezierKnot(gameObject.transform.position));
            spline.SetKnot(1, new BezierKnot(waypoints[roads.IndexOf(road)].gameObject.transform.position));
        }
    }

}



