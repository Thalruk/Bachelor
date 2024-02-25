using UnityEngine;
using UnityEngine.AI;

public class Car : MonoBehaviour
{
    NavMeshAgent agent;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    public void MoveTo(Vector3 position)
    {
        Debug.Log("MOVING");
        agent.SetDestination(position);
    }
}
