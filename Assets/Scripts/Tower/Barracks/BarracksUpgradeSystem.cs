using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BarracksUpgradeSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> upgradeButtons;

    private Main main;
    private GameObject upgradePanel;
    private Button[] buttons;
    private BarracksTower selectedTower;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

        buttons = upgradeButtons.Select((b, i) =>
        {
            Button btn = b.transform.Find($"UpgradeButton{i + 1}").GetComponent<Button>();
            btn.onClick.AddListener(() => Upgrade(i));
            return btn;
        }).ToArray();
    }

    private void Upgrade(int upgradeIndex)
    {

    }
}
