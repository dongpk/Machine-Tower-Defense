using UnityEngine;

public class Castle : MonoBehaviour
{
    private GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().DestroyEnemy();
            if (gameManager == null)
            {
                gameManager = FindFirstObjectByType<GameManager>();
            }
            if (gameManager != null)
            {
                gameManager.UpdateHP(-1);
            }
        }
    }
}
