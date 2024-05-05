using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool startPoint;
    public float spawnCooldown = 999.99f;
    public bool canSpawn = true;
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

    public IEnumerator Spawn()
    {
        canSpawn = false;
        yield return new WaitForSecondsRealtime(spawnCooldown);
        canSpawn = true;
    }
}
