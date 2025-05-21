using UnityEngine;

public class TowerUpgradeSystem : MonoBehaviour
{
    private GameObject upgradePanel;
    private Tower selectedTower;

    public void FindUpgradePanel(GameObject panel)
    {
        upgradePanel = panel;
    }

    public void NewTowerSelected(Tower newTower)
    {
        upgradePanel.SetActive(true);
        selectedTower = newTower;
    }

    public void TowerDeselected()
    {
        upgradePanel.SetActive(false);
        selectedTower = null;
    }
}
