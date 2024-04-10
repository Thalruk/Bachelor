using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool startPoint;
    public bool endPoint;

    [SerializeField] private int carAmount;
    [SerializeField] private List<Road> roads;

    private void Awake()
    {
        carAmount = 0;
    }

    public int GetCarAmount()
    {
        return carAmount;
    }
    public List<Road> GetRoads()
    {
        return roads;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Car car))
        {
            carAmount++;
            if (roads.Count != 0)
            {
                car.HitWaypoint(City.Instance.splineContainer.Splines[Random.Range(roads[0].GetIndex(), roads[roads.Count - 1].GetIndex() + 1)]);
            }
            if (endPoint)
            {
                Destroy(other.gameObject);
                City.Instance.RespawnCar();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Car car))
        {
            carAmount--;
        }
    }
}
