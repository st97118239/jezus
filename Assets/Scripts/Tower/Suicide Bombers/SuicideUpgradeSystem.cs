using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuicideUpgradeSystem : MonoBehaviour
{
    public Button destinationButton;

    [SerializeField] private List<GameObject> upgradeButtons;

    private Main main;
    private GameObject suicidePanel;
    private Button[] buttons;
    private BomberTower selectedBomber;

    private void Start()
    {
        main = FindObjectOfType<Main>();

        buttons = upgradeButtons.Select((b, i) =>
        {
            Button btn = b.transform.Find($"UpgradeButton{i}").GetComponent<Button>();
            btn.onClick.AddListener(() => BuyUpgrade(i));
            return btn;
        }).ToArray();

        destinationButton.onClick.AddListener(() => DestinationButton());
    }

    public void FindSuicidePanel(GameObject panel)
    {
        suicidePanel = panel;
        suicidePanel.SetActive(false);
    }

    private void FillUpgradeText(int buttonCount)
    {
        GameObject selectedButton = upgradeButtons[buttonCount];

        Button upgradeButton = selectedButton.transform.Find("UpgradeButton" + buttonCount).GetComponent<Button>();
        TMP_Text upgradeButtonText = upgradeButton.transform.Find("UpgradeButtonText" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextName = selectedButton.transform.Find("UpgradeTextName" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextAmount = selectedButton.transform.Find("UpgradeTextAmount" + buttonCount).GetComponent<TMP_Text>();
        TMP_Text upgradeTextMax = selectedButton.transform.Find("UpgradeTextMax" + buttonCount).GetComponent<TMP_Text>();

        selectedBomber.RecalculatePrice(buttonCount);
        upgradeButtonText.text = "Upgrade:\n$" + selectedBomber.upgradeCost[buttonCount];
        upgradeTextName.text = selectedBomber.upgrade[buttonCount].ToReadableString();
        selectedBomber.GetStat(buttonCount, out float stat);
        upgradeTextAmount.text = stat.ToString("0.##");
        upgradeTextMax.text = (selectedBomber.upgradeCount[buttonCount] + "/" + selectedBomber.upgradeMax[buttonCount]).ToString();

        if (selectedBomber.upgradeCount[buttonCount] >= selectedBomber.upgradeMax[buttonCount])
            DisableUpgradeButton(upgradeButton);
        else
            EnableUpgradeButton(upgradeButton);
    }

    private void BuyUpgrade(int upgradeIndex)
    {
        if (selectedBomber.isDisabled)
            return;

        int upgradeCost = selectedBomber.Upgrade(upgradeIndex, main.coinsAmount);
        if (upgradeCost > 0)
        {
            FillUpgradeText(upgradeIndex);
            main.coinsAmount -= upgradeCost;
            main.ip.RedrawCoinText(main.coinsAmount);
            selectedBomber.RecalculatePrice(upgradeIndex);
        }
    }

    private void DestinationButton()
    {
        if (selectedBomber.isDisabled)
            return;

        selectedBomber.FindNewDestination();
        destinationButton.interactable = false;
    }


    public void NewTowerSelected(BomberTower newTower)
    {
        suicidePanel.SetActive(true);
        selectedBomber = newTower;
        for (int i = 0; i < upgradeButtons.Count; i++)
            FillUpgradeText(i);
    }

    public void TowerDeselected()
    {
        suicidePanel.SetActive(false);
        selectedBomber = null;
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
