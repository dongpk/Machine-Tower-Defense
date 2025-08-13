using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<TowerUnlocData> towerUnlocks;


    private void Start()
    {
        UnlockAvailableTowers();
    }
    private void UnlockAvailableTowers()
    {
        UI ui = FindFirstObjectByType<UI>();

        foreach(var unlockData in towerUnlocks)
        {
            foreach(var buildButton in ui.buildButtonsUI.GetBuildButtons())
            {
                buildButton.UnlockTowerIfNeeded(unlockData.towerName, unlockData.isUnlocked);
            }
        }

        ui.buildButtonsUI.UpdateUnlockButton(); 
    }

    [ContextMenu("Initialize Tower Data")]
    private void InitislizeTowerData()
    {
        towerUnlocks.Clear();
        towerUnlocks.Add(new TowerUnlocData("Crossbow", false));
        towerUnlocks.Add(new TowerUnlocData("Cannon", false));
        towerUnlocks.Add(new TowerUnlocData("Gatling Gun", false));
        towerUnlocks.Add(new TowerUnlocData("Spider Nest", false));
        towerUnlocks.Add(new TowerUnlocData("Anti-air Harpoon", false));
        towerUnlocks.Add(new TowerUnlocData("Fan", false));
        towerUnlocks.Add(new TowerUnlocData("Hammer", false));
    }

}

[System.Serializable]
public class TowerUnlocData
{
    public string towerName;
    public bool isUnlocked;

    public TowerUnlocData(string newTowerName, bool newIsUnlocked)
    {
        towerName = newTowerName;
        isUnlocked = newIsUnlocked;
    }
}