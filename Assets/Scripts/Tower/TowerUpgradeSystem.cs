using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgradeButtons;

    private GameObject upgradePanel;
    private Tower selectedTower;
    private TowerUpgrades selectedTowerUpgrades;

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
        GameObject selectedButton = upgradeButtons[buttonCount - 1];

        TMP_Text upgradeButtonText = selectedButton.transform.Find("UpgradeButton" + buttonCount).transform.Find("UpgradeButtonText" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextName = selectedButton.transform.Find("UpgradeTextName" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextAmount = selectedButton.transform.Find("UpgradeTextAmount" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextMax = selectedButton.transform.Find("UpgradeTextMax" + buttonCount).GetComponent<TMP_Text>();

        selectedTowerUpgrades.RecalculatePrice(buttonCount - 1);
        upgradeButtonText.text = "Upgrade:\n$" + selectedTowerUpgrades.upgradeCost[buttonCount - 1];
        upgradeTextName.text = selectedTowerUpgrades.upgrade[buttonCount - 1].ToReadableString() + ":";
        selectedTowerUpgrades.GetStat(buttonCount, out float stat);
        upgradeTextAmount.text = stat.ToString();
        upgradeTextMax.text = (selectedTowerUpgrades.upgradeCount[buttonCount - 1] + "/" + selectedTowerUpgrades.upgradeMax[buttonCount - 1]).ToString();
    }

    public void TowerDeselected()
    {
        upgradePanel.SetActive(false);
        selectedTower = null;
    }
}
