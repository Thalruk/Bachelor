using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineSampler : MonoBehaviour
{
    [SerializeField]
    private SplineContainer _splineContainer;

    [SerializeField]
    [Range(0f, 1f)] private float _time;
    [SerializeField] private float _width;

    public int NumSplines;

    float3 position;
    float3 forward;
    float3 upVector3;

    private void Update()
    {
        NumSplines = _splineContainer.Splines.Count;
    }

    internal void SampleSplineWidth(int j, float t, float width, out Vector3 p1, out Vector3 p2)
    {
        _splineContainer.Evaluate(j, t, out position, out forward, out upVector3);

        float3 right = Vector3.Cross(forward, upVector3).normalized;

        p1 = position + (right * width);
        p2 = position + (-right * width);
    }
}
