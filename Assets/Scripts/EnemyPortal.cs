using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    private float spawnTimer;

    [Space]
    [SerializeField] private List<Waypoint> waypointList;

    private List<GameObject> enemiesToCreate = new List<GameObject>();
    private List<GameObject>activeEnemies = new List<GameObject>();

    private void Awake()
    {
        CollectWaypoints();
    }
    void Update()
    {
        if (CanMakeNewEnemy())
        {
            CreateEnemy();
        }
    }

    private bool CanMakeNewEnemy()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f && enemiesToCreate.Count > 0)
        {
            spawnTimer = spawnCooldown;
            return true;
        }
        return false;
    }

    private void CreateEnemy()
    {
        GameObject radomEnemy = getRandomEnemy();
        GameObject newEnemy = Instantiate(radomEnemy, transform.position, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetupEnemy(waypointList,this);

        activeEnemies.Add(newEnemy);
    }
    private GameObject getRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject chooseEnemy = enemiesToCreate[randomIndex];
        enemiesToCreate.Remove(chooseEnemy); // Remove the enemy from the list to avoid duplicates in the same wave
        return chooseEnemy;
    }
    public void AddEnemy(GameObject enemyToAdd) => enemiesToCreate.Add(enemyToAdd);
    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if (activeEnemies.Contains(enemyToRemove))
        {
            activeEnemies.Remove(enemyToRemove);
        }
    }
    public List<GameObject>GetAcTiveEnemies() => activeEnemies;

    [ContextMenu("Collect Waypoints")]
    private void CollectWaypoints()
    {
        waypointList = new List<Waypoint>();
        foreach (Transform child in transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            if (waypoint != null)
            {
                waypointList.Add(waypoint);
            }
        }
    }
}
