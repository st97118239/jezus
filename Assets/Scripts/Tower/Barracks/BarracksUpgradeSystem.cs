using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarracksUpgradeSystem : MonoBehaviour
{
    public Button upgradeButton;
    public Button destinationButton;

    private Main main;
    private GameObject barracksPanel;
    private Button[] buttons;
    private BarracksTower selectedBarracks;

    private void Start()
    {
        main = FindObjectOfType<Main>();

        upgradeButton.onClick.AddListener(() => Upgrade());
        destinationButton.onClick.AddListener(() => DestinationButton());
    }

    public void FindBarracksPanel(GameObject panel)
    {
        barracksPanel = panel;
        barracksPanel.SetActive(false);
    }

    private void Upgrade()
    {
        Debug.Log("upgrade");
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
    }

    public void TowerDeselected()
    {
        barracksPanel.SetActive(false);
        selectedBarracks = null;
    }
}
