using System.Collections;
using UnityEngine;

public class CrossBow_Visual : MonoBehaviour
{
    private Tower_CrossBow tower;
    [SerializeField] private LineRenderer attackLineVisuals;
    [SerializeField] private float attackVisualDuration = 0.1f;

    void Awake()
    {
        tower = GetComponent<Tower_CrossBow>();
    }
    public void PlayAttackVFX(Vector3 startPoint, Vector3 endPoint)
    {
        StartCoroutine(FXCoroutine(startPoint, endPoint));
    }

    private IEnumerator FXCoroutine(Vector3 startPoint, Vector3 endPoint)
    {
        tower.EnableRotion(false); // Disable rotation during attack
        attackLineVisuals.enabled = true;
        attackLineVisuals.SetPosition(0, startPoint);
        attackLineVisuals.SetPosition(1, endPoint);
        yield return new WaitForSeconds(attackVisualDuration);
        attackLineVisuals.enabled = false;

        tower.EnableRotion(true); // Re-enable rotation after attack
    }
}
