using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVetor;
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        float newRotationSpeed = rotationSpeed * 100;
        transform.Rotate(rotationVetor * newRotationSpeed * Time.deltaTime);
    }
}
