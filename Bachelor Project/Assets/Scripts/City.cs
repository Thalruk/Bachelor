using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
public class City : MonoBehaviour
{
    public static City Instance;
    public SplineContainer splineContainer;
    public List<Waypoint> waypoints;

    public GameObject carPrefab;


    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        UpdateRoadData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Car car = Instantiate(carPrefab, waypoints[0].transform.position, Quaternion.identity).GetComponent<Car>();

            car.currentSpline = splineContainer.Splines[0];

        }
    }

    public void GenerateRoads()
    {
        foreach (Waypoint waypoint in waypoints)
        {
            foreach (Road road in waypoint.roads)
            {
                Spline spline = splineContainer.AddSpline();

                BezierKnot[] knots = new BezierKnot[2];
                knots[0] = new BezierKnot(waypoint.transform.position - transform.position);
                knots[1] = new BezierKnot(road.to.transform.position - transform.position);
                spline.Knots = knots;
                spline.SetTangentMode(TangentMode.AutoSmooth);

                road.SetLength(spline.GetLength());
                road.SetIndex(splineContainer.Splines.Count - 1);
            }
        }
    }

    public void UpdateRoadData()
    {
        int splineIndex = 0;

        foreach (Waypoint waypoint in waypoints)
        {
            foreach (Road road in waypoint.roads)
            {

                road.SetLength(splineContainer.Splines[splineIndex].GetLength());
                road.SetIndex(splineIndex);
                splineIndex++;
            }
        }
    }

    public void DeleteRoads()
    {
        foreach (Spline road in splineContainer.Splines)
        {
            splineContainer.RemoveSpline(road);
        }
    }
}
