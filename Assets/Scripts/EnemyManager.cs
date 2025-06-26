using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class WaveDetails
{
    public int basicEnenmy;
    public int fastEnemy;
}
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private WaveDetails currentWave;
    [Space]
    [SerializeField] private float spawnCooldown;

    [SerializeField] private Transform respawn;
    private float spawnTimer;

    private List<GameObject> enemiesToCreate;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    void Start()
    {
        enemiesToCreate = newEnemyWave();
    }
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && enemiesToCreate.Count > 0)
        {

            CreateEnemy();
            spawnTimer = spawnCooldown;
        }
    }

    private void CreateEnemy()
    {
        GameObject radomEnemy = getRandomEnemy();
        GameObject newEnemy = Instantiate(radomEnemy, respawn.position, Quaternion.identity);

        // newEnemy.GetComponent<NavMeshAgent>
    }
    private GameObject getRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject chooseEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.Remove(chooseEnemy); // Remove the enemy from the list to avoid duplicates in the same wave
        return chooseEnemy;
    } 
    private List<GameObject> newEnemyWave()
    {
        List<GameObject> newEnemyList = new List<GameObject>();

        for (int i = 0; i < currentWave.basicEnenmy; i++)
        {
            newEnemyList.Add(basicEnemy);
        }
        for (int i = 0; i < currentWave.fastEnemy; i++)
        {  
            newEnemyList.Add(fastEnemy);
        }
        return newEnemyList;
    }
}
