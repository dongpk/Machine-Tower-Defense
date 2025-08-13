using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private UI ui;
    public BuildSlot selectedBuildSlot;

    public WaveManager waveManager;
    public GridBuilder currentGrid;

    [Header("Build Materials")]
    [SerializeField] private Material attackRadiusMaterial;
    [SerializeField] private Material buildPreviewMaterial;

    private bool isMouseOvetUI;

    private void Awake()
    {
        ui = FindFirstObjectByType<UI>();

        MakeBuildSlotAvailableIfNeeded(waveManager, currentGrid);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuildAction();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) )
        {
            if(isMouseOvetUI)
                return;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                bool clickedNotOnBuildSlot = hit.collider.GetComponent<BuildSlot>() == null;

                if (clickedNotOnBuildSlot)
                {
                    CancelBuildAction();
                }
            }
        }
    }
    public void MouseOverUI(bool isOverUI) => isMouseOvetUI = isOverUI;
    public void MakeBuildSlotAvailableIfNeeded(WaveManager waveManager, GridBuilder currentGrid)
    {
        foreach (var wave in waveManager.GetLevelWave())
        {
            if (wave.nextGrid == null)
                continue;

            List<GameObject> grid = currentGrid.GetTilesSetup();
            List<GameObject> nextWaveGrid = wave.nextGrid.GetTilesSetup();

            for (int i = 0; i < grid.Count; i++)
            {
                TileSlot currentTile = grid[i].GetComponent<TileSlot>();
                TileSlot nextTile = nextWaveGrid[i].GetComponent<TileSlot>();

                bool tileNotTheSame = currentTile.GetMesh() != nextTile.GetMesh() ||
                                       currentTile.GetMaterial() != nextTile.GetMaterial() ||
                                       currentTile.GetAllChildren().Count != nextTile.GetAllChildren().Count;
                if (tileNotTheSame == false)
                    continue;

                BuildSlot buildSlot = grid[i].GetComponent<BuildSlot>();
                if (buildSlot != null)
                {
                    buildSlot.SetSlotAvailableTo(false);
                }


            }

        }
    }

    public void CancelBuildAction()
    {
        if (selectedBuildSlot == null)
            return;

        ui.buildButtonsUI.GetLastSelectedButton()?.SelectButton(false);
        selectedBuildSlot.UnselectTile();
        selectedBuildSlot = null;
        DisableBuildMenu();

    }

    public void SelectBuildSlot(BuildSlot newSlot)
    {
        if (selectedBuildSlot != null)
        {
            selectedBuildSlot.UnselectTile();
        }

        selectedBuildSlot = newSlot;
    }
    public void EnableBuildMenu()
    {
        if (selectedBuildSlot != null)
        {
            return;
        }
        ui.buildButtonsUI.ShowBuildButtons(true);
    }
    private void DisableBuildMenu()
    {
        ui.buildButtonsUI.ShowBuildButtons(false);
    }
    public BuildSlot GetSelectedSlot() => selectedBuildSlot;
    public Material GetAttackRadiusMaterial() => attackRadiusMaterial;
    public Material GetBuildPreviewMaterial() => buildPreviewMaterial;
}
