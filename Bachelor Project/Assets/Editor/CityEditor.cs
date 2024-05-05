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
        SplineContainer splineContainer = city.splineContainer;


        if (GUILayout.Button("Load Waypoints"))
        {
            city.LoadWaypoints();
        }
        if (GUILayout.Button("Generate Roads"))
        {
            city.GenerateRoads();
        }
        if (GUILayout.Button("Delete Roads"))
        {
            city.DeleteRoads();
        }
        if (GUILayout.Button("Regenerate Roads"))
        {
            city.DeleteRoads();
            city.GenerateRoads();
        }
    }
}
