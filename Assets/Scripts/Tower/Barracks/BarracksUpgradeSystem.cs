using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarracksUpgradeSystem : MonoBehaviour
{
    public Button upgradeButton;
    public Button destinationButton;
    public Button spawnButton;

    private Main main;
    private GameObject barracksPanel;
    private Button[] buttons;
    private BarracksTower selectedBarracks;

    private void Start()
    {
        main = FindObjectOfType<Main>();

        upgradeButton.onClick.AddListener(() => Upgrade());
        destinationButton.onClick.AddListener(() => DestinationButton());
        spawnButton.onClick.AddListener(() => Spawn());
    }

    public void FindBarracksPanel(GameObject panel)
    {
        barracksPanel = panel;
        barracksPanel.SetActive(false);
    }

    public void FillUpgradeButton()
    {
        Transform upgradeParent = upgradeButton.transform.parent;

        TMP_Text upgradeButtonText = upgradeButton.transform.Find("UpgradeButtonText").GetComponent<TMP_Text>();
        TMP_Text upgradeTextUnit = upgradeParent.transform.Find("UpgradeTextName").GetComponent<TMP_Text>();
        TMP_Text upgradeTextMax = upgradeParent.transform.Find("UpgradeTextMax").GetComponent<TMP_Text>();

        selectedBarracks.RecalculateUpgradePrice();

        upgradeButtonText.text = "Upgrade:\n$" + selectedBarracks.upgradePrice;
        upgradeTextUnit.text = selectedBarracks.units[selectedBarracks.upgradeCount].type.ToReadableString();
        upgradeTextMax.text = (selectedBarracks.upgradeCount + "/" + (selectedBarracks.units.Count - 1)).ToString();

        if (selectedBarracks.upgradeCount >= (selectedBarracks.units.Count - 1))
            DisableUpgradeButton();
        else
            EnableUpgradeButton();
    }

    public void FillSpawnButton()
    {
        Transform spawnParent = spawnButton.transform.parent;

        TMP_Text spawnButtonText = spawnButton.transform.Find("SpawnButtonText").GetComponent<TMP_Text>();
        TMP_Text spawnTextUnit = spawnParent.transform.Find("SpawnTextName").GetComponent<TMP_Text>();
        TMP_Text spawnTextMax = spawnParent.transform.Find("SpawnTextMax").GetComponent<TMP_Text>();

        selectedBarracks.RecalculateSpawnPrice();
        spawnButtonText.text = "Spawn:\n$" + selectedBarracks.spawnPrice;
        spawnTextUnit.text = selectedBarracks.units[selectedBarracks.upgradeCount].type.ToReadableString();
        spawnTextMax.text = (selectedBarracks.spawnedUnits.Count + "/" + (selectedBarracks.maxUnits)).ToString();

        if (selectedBarracks.spawnedUnits.Count >= selectedBarracks.maxUnits)
            DisableSpawnButton();
        else
            EnableSpawnButton();
    }

    private void Upgrade()
    {
        Debug.Log("upgrade");

        if (selectedBarracks.upgradeCount >= (selectedBarracks.unitsUpgradePrice.Count))
        {
            Debug.Log("max upgrades");
        }

        if (main.coinsAmount < selectedBarracks.upgradePrice)
            return;

        main.ChangeCoinAmount(-selectedBarracks.upgradePrice);
        selectedBarracks.Upgrade();
    }

    private void Spawn()
    {
        if (selectedBarracks.units.Count >= selectedBarracks.maxUnits)
            return;

        if (main.coinsAmount < selectedBarracks.spawnPrice)
            return;

        main.ChangeCoinAmount(-selectedBarracks.spawnPrice);
        selectedBarracks.Spawn();
    }

    private void DestinationButton()
    {
        selectedBarracks.FindNewDestination();
        destinationButton.interactable = false;
    }


    public void NewTowerSelected(BarracksTower newTower)
    {
        barracksPanel.SetActive(true);
        selectedBarracks = newTower;
        FillUpgradeButton();
        FillSpawnButton();
    }

    public void TowerDeselected()
    {
        barracksPanel.SetActive(false);
        selectedBarracks = null;
    }

    public void DisableUpgradeButton()
    {
        upgradeButton.interactable = false;
    }

    private void EnableUpgradeButton()
    {
        upgradeButton.interactable = true;
    }

    public void DisableSpawnButton()
    {
        spawnButton.interactable = false;
    }

    private void EnableSpawnButton()
    {
        spawnButton.interactable = true;
    }
}
