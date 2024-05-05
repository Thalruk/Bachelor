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
    [SerializeField] int currentMaxSpeed;
    [SerializeField] LayerMask layerMask;
    [SerializeField] private GameObject raySpawner;

    private Rigidbody rb;
    public Spline currentSpline;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxSpeed = UnityEngine.Random.Range(30, 100);
        currentMaxSpeed = maxSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, nextWaypoint.transform.position) <= 0.5f)
        {
            if (nextWaypoint.GetRoads().Count != 0)
            {
                Road nextRoad = nextWaypoint.GetRandomRoad();
                // if (nextRoad.to.GetRoads().Any(Road => Road.to == nextWaypoint))
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

            if (hit.collider.TryGetComponent(out Car otherCar))
            {
                if (otherCar.rb.velocity.magnitude < rb.velocity.magnitude)
                {
                    currentMaxSpeed = hit.collider.GetComponent<Car>().maxSpeed;
                }
            }
            if (hit.collider.TryGetComponent(out JunctionLights junctionLights))
            {

                if (junctionLights.boxCollider.enabled && Vector3.Distance(transform.position, junctionLights.transform.position) < 0.5f)
                {
                    currentMaxSpeed = 0;
                }
            }
        }
        else
        {
            currentMaxSpeed = maxSpeed;
        }
    }

    void SpeedUp()
    {
        rb.AddForce(transform.forward * power, ForceMode.VelocityChange);
    }
    void SlownDown()
    {
        rb.AddForce(-transform.forward * power, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        var spline = new NativeSpline(currentSpline);
        SplineUtility.GetNearestPoint(spline, transform.position, out float3 nearest, out float t, SplineUtility.PickResolutionMax, 5);
        transform.position = new Vector3(nearest.x, nearest.y, nearest.z);

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);

        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward, Vector3.up));
        transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

        rb.velocity = rb.velocity.magnitude * transform.forward;

        if (rb.velocity.magnitude < currentMaxSpeed - 5 && currentMaxSpeed != 0)
        {
            this.Invoke(nameof(SpeedUp), carData.reactionOffset);
        }

        if (rb.velocity.magnitude > currentMaxSpeed + 5 && currentMaxSpeed != 0)
        {
            this.Invoke(nameof(SlownDown), carData.reactionOffset);
        }
    }

    public void HitWaypoint(Spline road)
    {
        currentSpline = road;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(nextWaypoint.transform.position, 0.1f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);
    }
}
