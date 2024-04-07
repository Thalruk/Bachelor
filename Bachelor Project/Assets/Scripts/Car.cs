using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [SerializeField] float power;


    private Rigidbody rb;
    public Spline currentSpline;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var spline = new NativeSpline(currentSpline);
        _ = SplineUtility.GetNearestPoint(spline, transform.position, out float3 nearest, out float t);

        transform.position = nearest;

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);

        var remappedForward = new Vector3(0, 0, 1);
        var remappedUp = new Vector3(0, 1, 0);
        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));

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
    }
}
