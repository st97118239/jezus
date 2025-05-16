using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private Tower[] towers;
    [SerializeField] private Button[] buttons;

    private Main main;
    private Tower towerToPlace;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

        for (int i = 0; i < towers.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => CheckIfCanBuyTower(towers[index]));
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
        towerToPlace = towerToBuy;

        if (main.coinsAmount >= towerToPlace.price)
            FindPlaceLocation();
        else
            return;
    }

    private void FindPlaceLocation()
    {
        GetComponent<WhereToPlace>().StartSearch();
    }

    public void PlaceTower(Vector3 whereToPlace)
    {
        GameObject newTower = Instantiate(towerToPlace.gameObject, whereToPlace, Quaternion.identity);
        BuyTower();
    }

    private void BuyTower()
    {
        main.ChangeCoinAmount(-towerToPlace.price);
    }
}
