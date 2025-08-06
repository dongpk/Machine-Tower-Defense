 using System;
using System.Collections.Generic;
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
    private GameManager gameManager;
    private EnemyPortal myPortal;
    private NavMeshAgent agent;

    [SerializeField] private Transform centerPoint;
    [SerializeField] private EnemyType enemyType;
    public int healthPoints = 4;

    [Header("Movement Settings")]
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private List<Transform> myWaypoints;

    [Space]
    private float totalDistance;


    private int nextWaypointIndex;
    private int currentWaypointIndex;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Disable automatic position updates
        agent.avoidancePriority = Mathf.RoundToInt(agent.speed * 10);
        gameManager = FindFirstObjectByType<GameManager>();
    }


    public void SetupEnemy(List<Waypoint> newWaypoints, EnemyPortal myEnemyPortal)
    {
        myWaypoints = new List<Transform>();


        if (newWaypoints == null || newWaypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints provided for enemy setup.");
            return;
        }
        foreach (var point in newWaypoints)
        {
            myWaypoints.Add(point.transform);
        }
        CollectTotalDistance();
        myPortal = myEnemyPortal;
        
    }

    public float DistanceToFinishLine() => totalDistance + agent.remainingDistance;
    private void CollectTotalDistance()
    {
        for (int i = 0; i < myWaypoints.Count - 1; i++)
        {
            float Distance = Vector3.Distance(myWaypoints[i].position, myWaypoints[i + 1].position);

            totalDistance += Distance;
        }
    }



    void Update()
    {

        FaceTarget(agent.steeringTarget);
        if (ShouldChangeWaypoint())
        {
            agent.SetDestination(GetNextWaypoint());    
        }
    }

    private bool ShouldChangeWaypoint()
    {
        if (nextWaypointIndex >= myWaypoints.Count)
        {
            return false;
        }
        if(agent.remainingDistance < 0.5f)
        {
            return true; // No need to change waypoint if we are close enough
        }
        Vector3 currentWaypoint = myWaypoints[currentWaypointIndex].position;
        Vector3 nextWaypoint = myWaypoints[nextWaypointIndex].position;
        float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
        float distanceBetweenPoints = Vector3.Distance(currentWaypoint, nextWaypoint);

        return distanceBetweenPoints > distanceToNextWaypoint;
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
        if (nextWaypointIndex >= myWaypoints.Count)
        {
            return transform.position; // Return to the start position if all waypoints are visited
        }
        Vector3 targetPoint = myWaypoints[nextWaypointIndex].position;

        if (nextWaypointIndex > 0)
        {
            float Distance = Vector3.Distance(myWaypoints[nextWaypointIndex].position, myWaypoints[nextWaypointIndex - 1].position);
            totalDistance -= Distance;
        }


        nextWaypointIndex = nextWaypointIndex + 1;
        currentWaypointIndex = nextWaypointIndex - 1;
        return targetPoint;
    }

    public void TakeDamage(int damage)
    {
        healthPoints = healthPoints - damage;
        if (healthPoints <= 0)
        {
            Die();
        }
    }
    private void Die()
    {        
        myPortal.RemoveActiveEnemy(gameObject);      
        gameManager.UpdateCurrency(1);
        Destroy(gameObject);
    }
    public void DestroyEnemy()
    {
        myPortal.RemoveActiveEnemy(gameObject);
        Destroy(gameObject);
    }
    public Vector3 CenterPoint() => centerPoint.position;
    public EnemyType GetEnemyType() => enemyType;

}
