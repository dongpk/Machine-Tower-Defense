using UnityEngine;

public class Tower_CrossBow : Tower
{
    private CrossBow_Visual visual;

    [Header("Crossbow Details")]
    [SerializeField] private int damage;
    [SerializeField] protected Transform gunPoint;

    protected override void Awake()
    {
        base.Awake();

        visual = GetComponent<CrossBow_Visual>();
    }

    protected override void Attack()
    {
        if (currentEnemy == null || gunPoint == null)
        {
            return; // Không có kẻ thù hoặc gunPoint chưa được gán
        }

        Vector3 directionToEnemy = DirectionToEnemyFrom(gunPoint);

        if (Physics.Raycast(gunPoint.position, directionToEnemy, out RaycastHit hitInfo, Mathf.Infinity))
        {
            towerHead.forward = directionToEnemy; // Quay đầu tháp về phía kẻ thù
                                                  // Debug.DrawLine(gunPoint.position, hitInfo.point);


            Enemy enemyTarget = null;
            IDamagable damagable = hitInfo.transform.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
                enemyTarget = currentEnemy;
            }
            
            visual.PlayAttackVFX(gunPoint.position, hitInfo.point,enemyTarget);
            visual.PlayReloadVFX(attackCooldown);
        }
    }
}
