using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgradeButtons;

    private Main main;
    private GameObject upgradePanel;
    private Button[] buttons;
    private Tower selectedTower;
    private TowerUpgrades selectedTowerUpgrades;

    private void Start()
    {
        main = FindObjectOfType<Main>();

        buttons = upgradeButtons.Select((b, i) =>
        {
            Button btn = b.transform.Find($"UpgradeButton{i + 1}").GetComponent<Button>();
            btn.onClick.AddListener(() => BuyUpgrade(i));
            return btn;
        }).ToArray();
    }

    private void BuyUpgrade(int upgradeIndex)
    {
        int upgradeCost = selectedTowerUpgrades.Upgrade(upgradeIndex, main.coinsAmount);
        if (upgradeCost > 0)
        {
            FillUpgradeText(upgradeIndex);
            main.coinsAmount -= upgradeCost;
            main.ip.RedrawCoinText(main.coinsAmount);
            selectedTowerUpgrades.RecalculatePrice(upgradeIndex);
        }
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
            FillUpgradeText(i);
    }

    private void FillUpgradeText(int buttonCount)
    {
        buttonCount++;
        GameObject selectedButton = upgradeButtons[buttonCount - 1];

        TMP_Text upgradeButtonText = selectedButton.transform.Find("UpgradeButton" + buttonCount).transform.Find("UpgradeButtonText" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextName = selectedButton.transform.Find("UpgradeTextName" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextAmount = selectedButton.transform.Find("UpgradeTextAmount" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextMax = selectedButton.transform.Find("UpgradeTextMax" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text towerTextName = upgradePanel.transform.Find("TowerName").GetComponent<TMP_Text>();

        selectedTowerUpgrades.RecalculatePrice(buttonCount - 1);
        upgradeButtonText.text = "Upgrade:\n$" + selectedTowerUpgrades.upgradeCost[buttonCount - 1];
        upgradeTextName.text = selectedTowerUpgrades.upgrade[buttonCount - 1].ToReadableString();
        selectedTowerUpgrades.GetStat(buttonCount, out float stat);
        upgradeTextAmount.text = stat.ToString("0.##");
        upgradeTextMax.text = (selectedTowerUpgrades.upgradeCount[buttonCount - 1] + "/" + selectedTowerUpgrades.upgradeMax[buttonCount - 1]).ToString();
        towerTextName.text = selectedTower.type.ToReadableString();
    }

    public void TowerDeselected()
    {
        upgradePanel.SetActive(false);
        selectedTower = null;
    }
}
