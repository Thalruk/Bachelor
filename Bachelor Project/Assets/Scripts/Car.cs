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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if ((transform.position - nextWaypoint.transform.position).magnitude < 0.3f)
        {
            Debug.Log((transform.position - nextWaypoint.transform.position).magnitude);
            Debug.Log(nextWaypoint.name);

            if (nextWaypoint.GetRoads().Count != 0)
            {
                HitWaypoint(City.Instance.splineContainer.Splines[nextWaypoint.GetRoads()[0].GetIndex()]);
                nextWaypoint = nextWaypoint.GetRoads()[0].GetWaypoint();
            }
            else
            {
                Destroy(gameObject);
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

        if (Input.GetKey(KeyCode.W))
        {
            Throttle(power);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Throttle(-power);
        }
    }

    private void Throttle(float power)
    {
        Vector3 dir = power * transform.forward;
        rb.AddForce(dir);
    }

    public void HitWaypoint(Spline road)
    {
        currentSpline = road;
        Debug.Log("HITWAYPOINT " + name);
    }
}
