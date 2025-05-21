using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeSystem : MonoBehaviour
{
    private GameObject upgradePanel;
    private Tower selectedTower;
    private TowerUpgrades selectedTowerUpgrades;
    private Button upgradeButton1;
    private Button upgradeButton2;
    private Button upgradeButton3;
    private List<Button> upgradeButtons;

    private void Start()
    {

    }

    public void FindUpgradePanel(GameObject panel)
    {
        upgradePanel = panel;
    }

    public void NewTowerSelected(Tower newTower)
    {
        upgradePanel.SetActive(true);
        selectedTower = newTower;
        selectedTowerUpgrades = selectedTower.GetComponent<TowerUpgrades>();

        

        for (int i = 0; i < 3; i++)
        {
            FillUpgrades(i);
        }
    }

    private void FillUpgrades(int buttonCount)
    {
        buttonCount++;
        Button selectedButton = upgradeButtons[buttonCount - 1];

        TMP_Text UpgradeButtonText = selectedButton.transform.Find("UpgradeButtonText" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text UpgradeTextName = selectedButton.transform.Find("UpgradeTextName" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text UpgradeTextAmount = selectedButton.transform.Find("UpgradeTextAmount" + buttonCount).GetComponent<TMP_Text>();
        int UpgradeCount = selectedTowerUpgrades.upgrade1Count;
        int UpgradeMax = selectedTowerUpgrades.upgrade1Max;
    }

    public void TowerDeselected()
    {
        upgradePanel.SetActive(false);
        selectedTower = null;
    }
}
