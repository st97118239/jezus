using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarracksUpgradeSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgradeButtons;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button destinationButton;

    private Main main;
    private GameObject barracksPanel;
    private Button[] buttons;
    private BarracksTower selectedBarracks;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

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
