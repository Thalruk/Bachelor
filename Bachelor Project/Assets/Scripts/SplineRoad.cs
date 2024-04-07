using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
[RequireComponent(typeof(SplineSampler))]
public class SplineRoad : MonoBehaviour
{
    private List<Vector3> _vertsP1 = new();
    private List<Vector3> _vertsP2 = new();

    [SerializeField] private int resolution;
    [SerializeField] private float _width;

    [SerializeField] private SplineSampler _splineSampler;
    [SerializeField] private MeshFilter _meshFilter;

    private void OnEnable()
    {
        Spline.Changed += OnSplineChanged;
        Rebuild();
    }

    private void OnDisable()
    {
        Spline.Changed -= OnSplineChanged;
    }

    private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3)
    {
        Rebuild();
    }

    private void Update()
    {
        Rebuild();
    }

    private void Rebuild()
    {
        GetVerts();
        BuildMesh();
    }

    private void GetVerts()
    {
        _vertsP1 = new List<Vector3>();
        _vertsP2 = new List<Vector3>();

        float step = 1f / (float)resolution;
        Vector3 p1;
        Vector3 p2;

        for (int j = 0; j < _splineSampler.NumSplines; j++)
        {
            for (int i = 0; i < resolution; i++)
            {
                float t = step * i;
                _splineSampler.SampleSplineWidth(j, t, _width, out p1, out p2);
                _vertsP1.Add(p1);
                _vertsP2.Add(p2);
            }
            _splineSampler.SampleSplineWidth(j, 1f, _width, out p1, out p2);
            _vertsP1.Add(p1);
            _vertsP2.Add(p2);
        }
    }

    private void BuildMesh()
    {

        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        int offset = 0;

        int length = _vertsP2.Count;

        for (int currentSplineIndex = 0; currentSplineIndex < _splineSampler.NumSplines; currentSplineIndex++)
        {
            int splineOffset = resolution * currentSplineIndex;
            splineOffset += currentSplineIndex;

            for (int currentSplinePoint = 1; currentSplinePoint < resolution + 1; currentSplinePoint++)
            {
                int vertoffset = splineOffset + currentSplinePoint;
                Vector3 p1 = _vertsP1[vertoffset - 1];
                Vector3 p2 = _vertsP2[vertoffset - 1];
                Vector3 p3 = _vertsP1[vertoffset];
                Vector3 p4 = _vertsP2[vertoffset];

                offset = 4 * resolution * currentSplineIndex;
                offset += 4 * (currentSplinePoint - 1);

                int t1 = offset + 0;
                int t2 = offset + 2;
                int t3 = offset + 3;

                int t4 = offset + 3;
                int t5 = offset + 1;
                int t6 = offset + 0;

                verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
                tris.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });
            }
        }
        _meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        Handles.matrix = transform.localToWorldMatrix;
        foreach (var p1 in _vertsP1)
        {
            Handles.SphereHandleCap(0, p1, Quaternion.identity, 0.5f, EventType.Repaint);
        }
        foreach (var p2 in _vertsP2)
        {
            Handles.SphereHandleCap(0, p2, Quaternion.identity, 0.5f, EventType.Repaint);
        }
    }
}
