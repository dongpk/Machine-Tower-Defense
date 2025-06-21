using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private Transform[] Waypoint;

    private int waypointIndex;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        // agent.SetDestination(Waypoint.position);
    }
    void Update()
    {
        FaceTarget(agent.steeringTarget);
        if (agent.remainingDistance < 0.5f)
        {
            agent.SetDestination(GetNextWaypoint());

        } 
    }
    private void FaceTarget(Vector3 newTarget)
    {
        Vector3 directionToTarget = newTarget - transform.position;
        directionToTarget.y = 0;

        Quaternion newRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, turnSpeed * Time.deltaTime);
    }


    private Vector3 GetNextWaypoint()
    {
        if (waypointIndex >= Waypoint.Length)
        {
            return transform.position; // Return to the start position if all waypoints are visited
        }
        Vector3 targetPoint = Waypoint[waypointIndex].position;
        waypointIndex = waypointIndex + 1;
        return targetPoint;
    }
}
