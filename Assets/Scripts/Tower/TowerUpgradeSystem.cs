using System.Collections.Generic;
using System.Linq;
using TMPro;
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
            Button btn = b.transform.Find($"UpgradeButton{i}").GetComponent<Button>();
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
        for (int i = 0; i < upgradeButtons.Count; i++)
            FillUpgradeText(i);
    }

    private void FillUpgradeText(int buttonCount)
    {
        GameObject selectedButton = upgradeButtons[buttonCount];

        Button upgradeButton = selectedButton.transform.Find("UpgradeButton" + buttonCount).GetComponent<Button>();
        TMP_Text upgradeButtonText = upgradeButton.transform.Find("UpgradeButtonText" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextName = selectedButton.transform.Find("UpgradeTextName" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextAmount = selectedButton.transform.Find("UpgradeTextAmount" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextMax = selectedButton.transform.Find("UpgradeTextMax" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text towerTextName = upgradePanel.transform.Find("TowerName").GetComponent<TMP_Text>();

        selectedTowerUpgrades.RecalculatePrice(buttonCount);
        upgradeButtonText.text = "Upgrade:\n$" + selectedTowerUpgrades.upgradeCost[buttonCount];
        upgradeTextName.text = selectedTowerUpgrades.upgrade[buttonCount].ToReadableString();
        selectedTowerUpgrades.GetStat(buttonCount, out float stat);
        upgradeTextAmount.text = stat.ToString("0.##");
        upgradeTextMax.text = (selectedTowerUpgrades.upgradeCount[buttonCount] + "/" + selectedTowerUpgrades.upgradeMax[buttonCount]).ToString();
        towerTextName.text = selectedTower.type.ToReadableString();

        if (selectedTowerUpgrades.upgradeCount[buttonCount] >= selectedTowerUpgrades.upgradeMax[buttonCount])
            DisableUpgradeButton(upgradeButton);
        else
            EnableUpgradeButton(upgradeButton);
    }

    public void TowerDeselected()
    {
        upgradePanel.SetActive(false);
        selectedTower = null;
    }

    public void DisableUpgradeButton(Button button)
    {
        button.interactable = false;
    }

    private void EnableUpgradeButton(Button button)
    {
        button.interactable = true;
    }
}
