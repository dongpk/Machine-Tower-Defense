using Unity.VisualScripting;
using UnityEngine;

public class TowerPreview : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private TowerAttackRadiusDisplay attackRadiusDisplay;
    private Tower myTower;

    private float attackRange ;

    private void Awake()
    {
        attackRadiusDisplay = transform.AddComponent<TowerAttackRadiusDisplay>();
        myTower = GetComponent<Tower>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
        attackRange = myTower.GetAttackRange();

        MakeAllTransperent();
        DestroyExtraComponent();
    }
    public void ShowPreview(bool showPreview, Vector3 previewPosition)
    {
        transform.position = previewPosition;
        attackRadiusDisplay.CreateCirle(showPreview, attackRange);

    }

    private void DestroyExtraComponent()
    {
        if (myTower != null)
        {
            CrossBow_Visual crossBow_Visual = GetComponent<CrossBow_Visual>();

            Destroy(crossBow_Visual);
            Destroy(myTower);
        }
    }

    private void MakeAllTransperent()
    {
        Material previewMat = FindFirstObjectByType<BuildManager>().GetBuildPreviewMaterial();
        foreach (var mesh in meshRenderers)
        {
            mesh.material = previewMat;
        }
    }
}
