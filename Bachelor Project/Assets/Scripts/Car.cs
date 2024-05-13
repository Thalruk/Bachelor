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

    public bool isInTraffic = false;
    public float trafficTime = 0.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxSpeed = UnityEngine.Random.Range(30, 70);
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
            if (hit.collider.TryGetComponent(out Car otherCar) && Vector3.Distance(transform.position, otherCar.transform.position) < 4)
            {
                if (otherCar.currentMaxSpeed < currentMaxSpeed)
                {
                    currentMaxSpeed = otherCar.currentMaxSpeed;
                }
            }
            if (hit.collider.TryGetComponent(out JunctionLights junctionLights) && Vector3.Distance(transform.position, junctionLights.transform.position) < 3)
            {

                if (junctionLights.boxCollider.isTrigger == false)
                {

                    currentMaxSpeed = 0;
                }
                else
                {
                    currentMaxSpeed = maxSpeed;
                }
            }
        }
        else
        {
            currentMaxSpeed = maxSpeed;
        }

        if (currentMaxSpeed == 0)
        {
            isInTraffic = true;
        }
        else
        {
            isInTraffic = false;
        }

        if (isInTraffic)
        {
            trafficTime += Time.deltaTime;
        }
    }

    void SpeedUp()
    {
        rb.AddForce(transform.forward * power, ForceMode.VelocityChange);
    }
    private void FixedUpdate()
    {
        var spline = new NativeSpline(currentSpline);
        SplineUtility.GetNearestPoint(spline, transform.position, out float3 nearest, out float t, SplineUtility.PickResolutionMax, SplineUtility.PickResolutionMax);
        transform.position = new Vector3(nearest.x, nearest.y, nearest.z);

        Vector3 forward = Vector3.Normalize(spline.EvaluateTangent(t));
        Vector3 up = spline.EvaluateUpVector(t);

        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(Vector3.forward, Vector3.up));
        transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

        rb.velocity = rb.velocity.magnitude * transform.forward;



        if (rb.velocity.magnitude < currentMaxSpeed)
        {
            this.Invoke(nameof(SpeedUp), carData.reactionOffset);
        }

        if (rb.velocity.magnitude > currentMaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * currentMaxSpeed;

        }
    }

    public void HitWaypoint(Spline road)
    {
        currentSpline = road;
        trafficTime = 0;
    }

    public float GetActualTrafficTime()
    {
        return trafficTime;
    }

    internal void ResetActualTrafficTime()
    {
        trafficTime = 0;
        isInTraffic = false;
    }
}
