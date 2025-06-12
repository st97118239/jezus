using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    public List<Tower> towersPlaced;

    [SerializeField] private Tower[] towers;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Vector3 towerDefaultSpawn;

    private Main main;
    private Tower towerToPlace;

    private void Start()
    {
        main = FindObjectOfType<Main>();

        for (int i = 0; i < towers.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => CheckIfCanBuyTower(towers[index]));
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            TMP_Text buttonText = buttons[i].transform.Find("Text").GetComponent<TMP_Text>();
            if (towers[i] != null)
                buttonText.text = towers[i].type.ToReadableString() + "\n$" + towers[i].price;
            else
                Destroy(buttons[i].gameObject);
        }
    }

    private void Update()
    {
        for (var idx = 0; idx < towers.Length; idx++)
        {
            if (Input.GetKeyDown((KeyCode)(KeyCode.Alpha1 + idx)))
            {
                CheckIfCanBuyTower(towers[idx]);
                break;
            }
        }
    }

    private void CheckIfCanBuyTower(Tower towerToBuy)
    {
        if (towerToBuy == null)
            return;

        towerToPlace = towerToBuy;

        if (main.coinsAmount >= towerToPlace.price)
            FindPlaceLocation();
    }

    private void FindPlaceLocation()
    {
        GameObject newTower = Instantiate(towerToPlace.gameObject, towerDefaultSpawn, Quaternion.identity);
        GetComponent<WhereToPlace>().StartSearch(newTower);
    }

    public void PlaceTower(Tower tower)
    {
        main.ChangeCoinAmount(-towerToPlace.price);
        if (!tower.hasNoShooter)
            tower.TurnShooterOn();
        towersPlaced.Add(tower);
        main.ChangeLayerOfAllDescendants(tower.transform, 10);
    }
}
