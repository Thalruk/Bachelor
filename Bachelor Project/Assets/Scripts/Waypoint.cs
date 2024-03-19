using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public List<Waypoint> to;
    public Vector3 lineOffset = new Vector3(0, 5, 0);

    private void Awake()
    {

    }

    private void OnDrawGizmos()
    {
        foreach (Waypoint waypoint in to)
        {
            if (waypoint.to.Contains(this))
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.magenta;
            }
            Gizmos.DrawLine(transform.position + lineOffset, waypoint.transform.position + lineOffset);
        }
    }
}



