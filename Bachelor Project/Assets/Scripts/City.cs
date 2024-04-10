using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(SplineContainer))]
[RequireComponent(typeof(SplineRoad))]
public class City : MonoBehaviour
{
    public static City Instance;
    public SplineContainer splineContainer;
    public SplineRoad splineRoad;
    public CityData cityData;

    public List<Waypoint> waypoints;
    [SerializeField] private List<Waypoint> startPoints;

    public GameObject carPrefab;
    [SerializeField] private GameObject carHolder;


    private void Awake()
    {
        splineContainer = GetComponent<SplineContainer>();

        startPoints = waypoints.FindAll(w => w.startPoint == true);

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        UpdateRoadData();
        StartCoroutine(SpawnCars());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Car car = Instantiate(carPrefab, waypoints[0].transform.position, Quaternion.identity).GetComponent<Car>();

            car.currentSpline = splineContainer.Splines[0];

        }
    }

    private IEnumerator SpawnCars()
    {
        for (int i = 0; i < cityData.maxCarAmount; i++)
        {
            RespawnCar();
            yield return new WaitUntil(() =>
            {
                return startPoints.FindAll(w => w.GetCarAmount() == 0).Count() > 0;
            });
        }
    }

    public void RespawnCar()
    {

        List<Waypoint> possibleSpawnPositions = startPoints.FindAll(w => w.GetCarAmount() == 0);

        if (possibleSpawnPositions.Count > 0)
        {
            Waypoint spawnPosition = possibleSpawnPositions[Random.Range(0, startPoints.Count - 1)];
            GameObject car = Instantiate(carPrefab, spawnPosition.transform.position, Quaternion.identity, carHolder.transform);
            car.GetComponent<Car>().currentSpline = splineContainer.Splines[spawnPosition.GetRoads()[0].GetIndex()];
        }
    }

    public void LoadWaypoints()
    {
        waypoints = GetComponentsInChildren<Waypoint>().ToList<Waypoint>();
    }

    public void GenerateRoads()
    {
        foreach (Waypoint waypoint in waypoints)
        {
            foreach (Road road in waypoint.GetRoads())
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
            foreach (Road road in waypoint.GetRoads())
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
        splineRoad.DeleteMesh();
    }
}
