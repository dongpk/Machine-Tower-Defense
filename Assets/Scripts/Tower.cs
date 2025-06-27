
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;

    [Header("Tower Settings")]
    [SerializeField] private Transform towerHead;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask whatIsEnemy;

    void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemyWithinRange();
            return;
        }


        if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange)
            currentEnemy = null;


        rotateTowardsEnemy();
    }

    private Transform FindRandomEnemyWithinRange()
    {
        List<Transform> possibleTargets = new List<Transform>();
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            possibleTargets.Add(enemy.transform);
        }

        int randomIndex = Random.Range(0, possibleTargets.Count);

        if (possibleTargets.Count <= 0)
            return null;

        return possibleTargets[randomIndex];
    }

    private void rotateTowardsEnemy()
    {
        if(currentEnemy == null)
        {
            return; // No enemy to rotate towards
        }
        Vector3 directionToEnemy = currentEnemy.position - towerHead.position;

        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);


        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;
        towerHead.rotation = Quaternion.Euler(rotation);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
