using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour
{
    [SerializeField] float power;
    [SerializeField] CarData carData;
    public Waypoint nextWaypoint;

    [SerializeField] int maxSpeed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] private GameObject raySpawner;
    private bool didRayHit = false;

    private Rigidbody rb;
    public Spline currentSpline;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxSpeed = UnityEngine.Random.Range(30, 100);
    }

    private void Update()
    {
        didRayHit = false;
        if ((transform.position - nextWaypoint.transform.position).magnitude <= 0.5f)
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

        Ray ray = new Ray(raySpawner.transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
        {
            didRayHit = true;

            print("HIT");

            if (hit.rigidbody.velocity.magnitude < rb.velocity.magnitude)
            {
                maxSpeed = hit.collider.GetComponent<Car>().maxSpeed;
            }
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            maxSpeed = 10;
        }

    }

    private void FixedUpdate()
    {

        var spline = new NativeSpline(currentSpline);
        SplineUtility.GetNearestPoint(spline, transform.position, out float3 nearest, out float t, SplineUtility.PickResolutionMax, 5);
        transform.position = nearest;

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);

        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward, Vector3.up));
        transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

        rb.velocity = rb.velocity.magnitude * transform.forward;

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * power, ForceMode.VelocityChange);
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.AddForce(-transform.forward * power * 3, ForceMode.VelocityChange);
        }
    }

    public void HitWaypoint(Spline road)
    {
        currentSpline = road;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(nextWaypoint.transform.position, 0.1f);

        if (didRayHit)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.white;
        }

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);

    }
}
