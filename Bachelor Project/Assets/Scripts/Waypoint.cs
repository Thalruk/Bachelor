using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Road> roads;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Car>(out var car))
        {
            car.HitWaypoint(City.Instance.splineContainer.Splines[Random.Range(roads[0].GetIndex(), roads[roads.Count - 1].GetIndex() + 1)]);
        }
    }
}
