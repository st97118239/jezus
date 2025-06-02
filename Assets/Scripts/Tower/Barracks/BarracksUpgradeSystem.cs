using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarracksUpgradeSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgradeButtons;

    private Main main;
    private GameObject barracksPanel;
    private Button[] buttons;
    private BarracksTower selectedTower;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

        //buttons = upgradeButtons.Select((b, i) =>
        //{
        //    Button btn = b.transform.Find($"UpgradeButton{i + 1}").GetComponent<Button>();
        //    btn.onClick.AddListener(() => Upgrade(i));
        //    return btn;
        //}).ToArray();
    }

    public void FindBarracksPanel(GameObject panel)
    {
        barracksPanel = panel;
        barracksPanel.SetActive(false);
    }

    private void Upgrade(int upgradeIndex)
    {

    }

    public void NewTowerSelected(BarracksTower newTower)
    {
        barracksPanel.SetActive(true);
        selectedTower = newTower;
    }

    public void TowerDeselected()
    {
        barracksPanel.SetActive(false);
        selectedTower = null;
    }
}
