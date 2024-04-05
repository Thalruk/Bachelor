using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[CustomEditor(typeof(City))]
public class CityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        City city = (City)target;
        SplineContainer splineContainer = target.GetOrAddComponent<SplineContainer>();

        if (GUILayout.Button("Create Roads"))
        {
            foreach (Waypoint waypoint in city.waypoints)
            {
                foreach (Waypoint to in waypoint.waypointList)
                {
                    Spline spline = splineContainer.AddSpline();

                    BezierKnot[] knots = new BezierKnot[2];
                    knots[0] = new BezierKnot(waypoint.transform.position - city.transform.position);
                    knots[1] = new BezierKnot(to.transform.position - city.transform.position);
                    spline.Knots = knots;
                    spline.SetTangentMode(TangentMode.AutoSmooth);
                }
            }
        }

        if (GUILayout.Button("Delete Roads"))
        {
            foreach (Spline road in splineContainer.Splines)
            {
                splineContainer.RemoveSpline(road);
            }
        }
    }
}
