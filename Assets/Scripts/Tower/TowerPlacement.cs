using Unity.VisualScripting;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public Tower towerToPlace;

    private Main main;

    private void Start()
    {
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (towerToPlace == null)
                return;

            if (main.coinsAmount >= towerToPlace.price)
            {
                main.ChangeCoinAmount(-towerToPlace.price);
            }
            else
                return;
        }
    }
}
