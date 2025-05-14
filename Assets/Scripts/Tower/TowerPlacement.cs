using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private Tower tower1;
    [SerializeField] private Tower tower2;
    [SerializeField] private Tower tower3;
    [SerializeField] private Tower tower4;

    private Main main;
    private Tower towerToPlace;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CheckIfCanBuyTower(tower1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            CheckIfCanBuyTower(tower2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            CheckIfCanBuyTower(tower3);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            CheckIfCanBuyTower(tower4);
    }

    private void CheckIfCanBuyTower(Tower towerToBuy)
    {
        towerToPlace = towerToBuy;

        if (main.coinsAmount >= towerToPlace.price)
            BuyTower();
        else
            return;
    }

    private void BuyTower()
    {
        main.ChangeCoinAmount(-towerToPlace.price);
    }
}
