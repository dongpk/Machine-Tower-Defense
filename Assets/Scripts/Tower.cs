
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Transform currentEnemy;
    [SerializeField] protected float attackCooldown= 1f; // Time in seconds between attacks
     protected float lastTimeAttacked;  


    [Header("Tower Settings")] 
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected   float rotationSpeed=10f;
    private bool canRotate ;
    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;

    protected virtual void Awake()
    {
        
    }
    protected virtual void Update()
    {
        if (currentEnemy == null)
        {
            currentEnemy = FindRandomEnemyWithinRange();
            return;
        }
        if (CanAttack())
        {

            Attack();
        }


        if (Vector3.Distance(currentEnemy.position, transform.position) > attackRange)
            currentEnemy = null;


        rotateTowardsEnemy();
    }
    protected virtual void Attack()
    {
        Debug.Log("attack performed at " + Time.time);
        // lastTimeAttacked = Time.time;
    }
    protected bool CanAttack()
    {
        if(currentEnemy == null)
        {
            return false; // No enemy to attack
        }
        if (Time.time > lastTimeAttacked + attackCooldown)
        {
            lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }

    protected Transform FindRandomEnemyWithinRange()
    {
        List<Enemy> possibleTargets = new List<Enemy>();
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
         
            possibleTargets.Add(newEnemy);
        }
    
        Enemy newTarget = getMostAdvancedEnemy(possibleTargets);
        if (newTarget != null)
        {
            return newTarget.transform; // Return the most advanced enemy's transform
        }
        return null; // No valid target found 
        
    }
    private Enemy getMostAdvancedEnemy(List<Enemy> targets)
    {
        Enemy mostAdvancedEnemy = null;
        float minRemainingDistance = float.MaxValue;
        foreach (Enemy enemy in targets)
        {
            float remainingDistance = enemy.DistanceToFinishLine();
            if(remainingDistance < minRemainingDistance)
            {
                minRemainingDistance = remainingDistance;
                mostAdvancedEnemy = enemy;
            }
        }

        return mostAdvancedEnemy;      
    }
    public void EnableRotion(bool enable)
    {
        canRotate = enable;
    }
   
    protected virtual void rotateTowardsEnemy()
    {
        if(canRotate == false)
        {
            return; // Rotation is disabled
        }
        if (currentEnemy == null)
        {
            return; // No enemy to rotate towards
        }
        Vector3 directionToEnemy = currentEnemy.position - towerHead.position;

        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);


        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;
        towerHead.rotation = Quaternion.Euler(rotation);
    }
    protected Vector3 DirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.position - startPoint.position).normalized;
    }
   

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
