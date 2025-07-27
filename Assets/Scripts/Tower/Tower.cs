
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Enemy currentEnemy;
    [SerializeField] protected float attackCooldown= 1f; // Time in seconds between attacks
     protected float lastTimeAttacked;


    [Header("Tower Settings")]
    [SerializeField] protected EnemyType enemyPriorityType= EnemyType.None;
    [SerializeField] protected Transform towerHead;
    [SerializeField] protected   float rotationSpeed=10f;
    private bool canRotate ;
    [SerializeField] protected float attackRange = 2.5f;
    [SerializeField] protected LayerMask whatIsEnemy;


    [Space]
    [Tooltip("Enabling this allows tower to change target between attacks\n Kích hoạt tính năng này cho phép tháp thay đổi mục tiêu giữa các cuộc tấn công")]
    [SerializeField] private bool dynamicTargetChange;
    private float targetCheckInterval = 0.1f; // Time in seconds to check for enemies
    private float lastTimeCheckedTarget = 0f;

    protected virtual void Awake()
    {
        EnableRotion(true);
    }
    protected virtual void Update()
    {

        UpdateTargetIfNeeded();
        if (currentEnemy == null)
        {
            currentEnemy = FindEnemyWithinRange();
            return;
        }
        if (CanAttack())
        {

            Attack();
        }

        LooseTargetIfNeeded();

        rotateTowardsEnemy();
    }

    private void LooseTargetIfNeeded()
    {
        if (Vector3.Distance(currentEnemy.CenterPoint(), transform.position) > attackRange)
            currentEnemy = null;
    }

    private void UpdateTargetIfNeeded()
    {
        if (dynamicTargetChange == false)
        {
            return; // Dynamic target change is disabled
        }
        if(Time.time > lastTimeCheckedTarget + targetCheckInterval)
        {
            lastTimeCheckedTarget = Time.time;
            currentEnemy = FindEnemyWithinRange();
        }
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

    protected Enemy FindEnemyWithinRange()
    {
        List<Enemy> priorityTargets = new List<Enemy>();
        List<Enemy> possibleTargets = new List<Enemy>();
        Collider[] enemiesAround = Physics.OverlapSphere(transform.position, attackRange, whatIsEnemy);

        foreach (Collider enemy in enemiesAround)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();

            EnemyType newEnemyType = newEnemy.GetEnemyType();
            if (newEnemyType == enemyPriorityType)
            {
                priorityTargets.Add(newEnemy);

            }
            else
            {
                possibleTargets.Add(newEnemy); 
            }
            possibleTargets.Add(newEnemy);
        }
    
       
        if (priorityTargets.Count > 0)
        {
            return getMostAdvancedEnemy(priorityTargets);
        }
        if(possibleTargets.Count > 0)
        {
            return getMostAdvancedEnemy(possibleTargets);
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
        Vector3 directionToEnemy = DirectionToEnemyFrom(towerHead);

        Quaternion lookRotation = Quaternion.LookRotation(directionToEnemy);


        Vector3 rotation = Quaternion.Lerp(towerHead.rotation, lookRotation, rotationSpeed * Time.deltaTime).eulerAngles;
        towerHead.rotation = Quaternion.Euler(rotation);
    }
    protected Vector3 DirectionToEnemyFrom(Transform startPoint)
    {
        return (currentEnemy.CenterPoint() - startPoint.position).normalized;
    }
   

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
