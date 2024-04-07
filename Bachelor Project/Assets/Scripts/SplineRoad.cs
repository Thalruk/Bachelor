using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
public class SplineRoad : MonoBehaviour
{
    private List<Vector3> _vertsP1 = new();
    private List<Vector3> _vertsP2 = new();
    Mesh mesh;

    [SerializeField] private int resolution;
    [SerializeField] private float _width;

    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private SplineContainer _splineContainer;


    public int NumSplines;

    float3 position;
    float3 forward;
    float3 upVector3;

    private void OnEnable()
    {
        mesh = new Mesh();
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

    private void Rebuild()
    {
        NumSplines = _splineContainer.Splines.Count;
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

        for (int j = 0; j < NumSplines; j++)
        {
            for (int i = 0; i < resolution; i++)
            {
                float t = step * i;
                SampleSplineWidth(j, t, _width, out p1, out p2);
                _vertsP1.Add(p1);
                _vertsP2.Add(p2);
            }
            SampleSplineWidth(j, 1f, _width, out p1, out p2);
            _vertsP1.Add(p1);
            _vertsP2.Add(p2);
        }
    }

    internal void SampleSplineWidth(int j, float t, float width, out Vector3 p1, out Vector3 p2)
    {
        _splineContainer.Evaluate(j, t, out position, out forward, out upVector3);

        float3 right = Vector3.Cross(forward, upVector3).normalized;

        p1 = position + (right * width);
        p2 = position + (-right * width);
    }

    private void BuildMesh()
    {

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        int offset = 0;

        int length = _vertsP2.Count;

        for (int currentSplineIndex = 0; currentSplineIndex < NumSplines; currentSplineIndex++)
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
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        _meshFilter.mesh = mesh;
    }

    public void DeleteMesh()
    {
        mesh.Clear();
    }
}
