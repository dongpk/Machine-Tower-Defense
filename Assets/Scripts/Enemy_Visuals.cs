using UnityEngine;

public class Enemy_Visuals : MonoBehaviour
{
    [SerializeField] private Transform visuals;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float verticalRotationSpeed;

    void Update()
    {
        AlignWithSlope();
    }
    private void AlignWithSlope()
    {
        if (visuals == null)
        {
            return;
        }
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, whatIsGround))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            visuals.rotation = Quaternion.Slerp(visuals.rotation, targetRotation, verticalRotationSpeed * Time.deltaTime);
        }
    }
}
