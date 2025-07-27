using System;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Basic,
    Fast,
    None
}
public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;
    private NavMeshAgent agent;
    public int healthPoints = 4;

    [Header("Movement Settings")]
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private Transform[] Waypoint;

    [Space]
    private float totalDistance;


    private int waypointIndex;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Disable automatic position updates
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
    }

    void Start()
    {

        // agent.SetDestination(Waypoint.position);
        Waypoint = FindFirstObjectByType<WayPontManager>().GetWaypoints();
        CollectTotalDistance();

    }
    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;
    private void CollectTotalDistance()
    {
        for (int i = 0; i < Waypoint.Length - 1; i++)
        {
            float Distance = Vector3.Distance(Waypoint[i].position, Waypoint[i + 1].position);

            totalDistance += Distance;
        }
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

        if (waypointIndex > 0)
        {
            float Distance = Vector3.Distance(Waypoint[waypointIndex].position, Waypoint[waypointIndex - 1].position);
            totalDistance -= Distance;
        }


        waypointIndex = waypointIndex + 1;
        return targetPoint;
    }

    public void TakeDamage(int damage)
    {
        healthPoints = healthPoints - damage;
        if (healthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public Vector3 CenterPoint() => centerPoint.position;
    public EnemyType GetEnemyType() => enemyType;

}
