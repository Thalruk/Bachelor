using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] CarData carData;
    public Waypoint nextWaypoint;

    private Rigidbody rb;
    public Spline currentSpline;

    [Range(50, 140)]
    [SerializeField] int maxSpeed;
    private Ray frontRaycast;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxSpeed = UnityEngine.Random.Range(50, 100);
        Debug.Log(maxSpeed);
        frontRaycast = new Ray(transform.position, transform.forward);

        rb.maxLinearVelocity = maxSpeed;

    }

    private void Update()
    {
        if ((transform.position - nextWaypoint.transform.position).magnitude <= 0.3f)
        {

            if (nextWaypoint.GetRoads().Count != 0)
            {
                Road nextRoad = nextWaypoint.GetRandomRoad();
                HitWaypoint(City.Instance.splineContainer.Splines[nextRoad.GetIndex()]);
                nextWaypoint = nextRoad.GetWaypoint();
            }
            else
            {
                Destroy(gameObject);
            }

        }

        if (Physics.Raycast(frontRaycast, out RaycastHit hit, 10))
        {
            if (hit.collider.TryGetComponent(out Car car))
            {

            }
        }
    }

    private void FixedUpdate()
    {
        var spline = new NativeSpline(currentSpline);
        _ = SplineUtility.GetNearestPoint(spline, transform.position, out float3 nearest, out float t, SplineUtility.PickResolutionMax, 5);
        transform.position = nearest;

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);

        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward, Vector3.up));

        transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

        Vector3 engineForward = transform.forward;

        if (Vector3.Dot(rb.velocity, transform.forward) < 0)
        {
            engineForward *= -1;
        }

        rb.velocity = rb.velocity.magnitude * engineForward;
        Throttle(power);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

    }

    private void Throttle(float power)
    {
        Vector3 dir = power * transform.forward;
        rb.AddForce(dir, ForceMode.VelocityChange);
    }

    public void HitWaypoint(Spline road)
    {
        currentSpline = road;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(nextWaypoint.transform.position, 0.1f);
        Gizmos.DrawRay(frontRaycast);
    }
}
