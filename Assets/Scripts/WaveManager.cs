using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDetails
{

    public GridBuilder nextGrid;
    public EnemyPortal[] newPortal;
    public int basicEnenmy;
    public int fastEnemy;
}
public class WaveManager : MonoBehaviour
{
    private UI_InGame inGameUI;

    [SerializeField] private GridBuilder currentGrid;
    public bool waveCompleted;
    public float timeBetweenWaves = 10f;
    public float waveTimer;
    [SerializeField] private WaveDetails[] levelWaves;
    private int waveIndex;
    private float checkInterval = .5f;
    private float nextCheckTime;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject basicEnemy;
    [SerializeField] private GameObject fastEnemy;

    private List<EnemyPortal> enemyPortals;



    [System.Obsolete]
    private void Awake()
    {
        enemyPortals = new List<EnemyPortal>(FindObjectsOfType<EnemyPortal>());
        inGameUI = FindFirstObjectByType<UI_InGame>(FindObjectsInactive.Include);

    }
    void Start()
    {
        SetUpNextWave();
    }
    private void Update()
    {
        HandleWaveCompletion();
        HandleWaveTiming();

    }

    private void HandleWaveCompletion()
    {
        if (ReadyToCheck() == false)
        {
            return; // Skip if not ready to check
        }
        if (waveCompleted == false && AllEnemiesDefeated())
        {
            CheckForNewLevelLayout();

            waveCompleted = true;
            waveTimer = timeBetweenWaves;
            inGameUI.EnableWaveTimer(true);
        }
    }

    private void HandleWaveTiming()
    {
        if (waveCompleted)
        {
            waveTimer -= Time.deltaTime;
            inGameUI.UpdateWaveTimerUI(waveTimer);
            if (waveTimer <= 0f)
            {
                inGameUI.EnableWaveTimer(false);

                SetUpNextWave();
            }
        }

    }

    public void ForceNextWave()
    {
        if (AllEnemiesDefeated()==false)//nếu tất cả kẻ thù đã bị đánh bại          
        {
            Debug.LogWarning("Cannot force next wave while enemies are still active.");
            return;
        }

        inGameUI.EnableWaveTimer(false); 
        SetUpNextWave();
    }

    [ContextMenu("Set Up Next Wave")]
    private void SetUpNextWave()
    {

        List<GameObject> newEnemies = newEnemyWave();
        int portalIndex = 0;
        if (newEnemies == null)
        {
            Debug.LogWarning("No wave to setup.");
            return;
        }
        for (int i = 0; i < newEnemies.Count; i++)
        {
            GameObject enemyToAdd = newEnemies[i];
            EnemyPortal portalToReciveEnemy = enemyPortals[portalIndex];

            portalToReciveEnemy.AddEnemy(enemyToAdd);
            portalIndex++;
            if (portalIndex >= enemyPortals.Count)
            {
                portalIndex = 0; // Reset to the first portal if we run out of portals
            }
        }

        waveCompleted = false;
    }

    private List<GameObject> newEnemyWave()
    {
        if (waveIndex >= levelWaves.Length)
        {
            Debug.LogWarning("No more waves available.");
            return null;
        }
        List<GameObject> newEnemyList = new List<GameObject>();

        for (int i = 0; i < levelWaves[waveIndex].basicEnenmy; i++)
        {
            newEnemyList.Add(basicEnemy);
        }
        for (int i = 0; i < levelWaves[waveIndex].fastEnemy; i++)
        {
            newEnemyList.Add(fastEnemy);
        }
        waveIndex++;
        return newEnemyList;
    }
    private void CheckForNewLevelLayout()
    {
        if (waveIndex >= levelWaves.Length)
        {
            return;
        }
        WaveDetails nextWave = levelWaves[waveIndex];

        if (nextWave.nextGrid != null)
        {
            UpdateLevelTiles(nextWave.nextGrid);
            EnableNewPortal(nextWave.newPortal);

        }

        currentGrid.UpdateNavMesh();
    }



    private void UpdateLevelTiles(GridBuilder nextGrid)
    {
        List<GameObject> grid = currentGrid.GetTilesSetup();
        List<GameObject> newGrid = nextGrid.GetTilesSetup();

        for (int i = 0; i < grid.Count; i++)
        {
            TileSlot currentTile = grid[i].GetComponent<TileSlot>();
            TileSlot newTile = newGrid[i].GetComponent<TileSlot>();

            bool shouldBeUpdated = currentTile.GetMesh() != newTile.GetMesh() ||
                                   currentTile.GetMaterial() != newTile.GetMaterial() ||
                                   currentTile.GetAllChildren().Count != newTile.GetAllChildren().Count ||
                                   currentTile.transform.rotation != newTile.transform.rotation;
            if (shouldBeUpdated)
            {
                currentTile.gameObject.SetActive(false);
                newTile.gameObject.SetActive(true);
                newTile.transform.parent = currentGrid.transform.parent;

                grid[i] = newTile.gameObject; // Update the reference in the grid list
                Destroy(currentTile.gameObject); // Optionally destroy the old tile
            }

        }
    }
    private void EnableNewPortal(EnemyPortal[] newPortals)
    {
        foreach (EnemyPortal portal in newPortals)
        {
            portal.gameObject.SetActive(true);
            enemyPortals.Add(portal);
        }
    }
    private bool AllEnemiesDefeated()
    {
        foreach (EnemyPortal portal in enemyPortals)
        {
            if (portal.GetAcTiveEnemies().Count > 0)
            {
                return false; // If any portal has active enemies, return false
            }
        }
        return true; // All portals have no active enemies
    }
    private bool ReadyToCheck()
    {
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            return true;
        }
        return false;
    }
}
