using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private Tower[] towers;

    private Main main;
    private Tower towerToPlace;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    CheckIfCanBuyTower(tower1);
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //    CheckIfCanBuyTower(tower2);
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //    CheckIfCanBuyTower(tower3);
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //    CheckIfCanBuyTower(tower4);

        for (var idx = 0; idx < towers.Length; idx++)
        {
            if (Input.GetKeyDown((KeyCode)(KeyCode.Alpha1 + idx)))
            {
                towerToPlace = towers[idx];
                CheckIfCanBuyTower(towerToPlace);
                break;
            }
        }
    }

    private void CheckIfCanBuyTower(Tower towerToBuy)
    {
        if (main.coinsAmount >= towerToPlace.price)
            FindPlaceLocation();
        else
            return;
    }

    private void FindPlaceLocation()
    {
        GetComponent<WhereToPlace>().needsToFindLocation = true;
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
