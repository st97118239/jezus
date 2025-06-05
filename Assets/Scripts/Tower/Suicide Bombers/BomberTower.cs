using System;
using System.Collections.Generic;
using UnityEngine;

public class BomberTower : MonoBehaviour
{
    public GameObject projectileSpawner;
    public GameObject landingBall;
    public LandingBall ballComponent;
    public Vector3 landingPos;

    public List<Upgrades> upgrade; // ReloadSpeed, AttackDamage, Range, ProjectileSpeed
    public List<int> upgradeCount; // times it has been upgraded (keep at 0)
    public List<int> upgradeMax; // max amount of times it can be upgraded
    [SerializeField] private List<float> upgradeFactor; // multiplier for upgrading
    [SerializeField] private List<int> upgradeBaseCost; // the standard cost of the upgrade
    [SerializeField] private List<float> upgradeCostFactor; // multiplier for the cost of the upgrade
    public List<int> upgradeCost; // the price of the upgrade (keep at 0)

    private Main main;
    private Tower tower;
    private Vector3 cursorLocation;
    private bool needsToFindLocation;
    private bool isReloading;
    private bool hasTargetPos;
    private float reloadTimer;
    private float reloadSpeed;

    private void Start()
    {
        main = FindObjectOfType<Main>();
        GameObject newBall = Instantiate(landingBall, transform.position, Quaternion.identity);
        ballComponent = newBall.GetComponent<LandingBall>();
        ballComponent.tower = this;
        ballComponent.mesh = ballComponent.GetComponent<MeshRenderer>();
        ballComponent.mesh.enabled = false;
        landingBall = newBall;
        tower = GetComponent<Tower>();
        reloadSpeed = tower.reloadSpeed;
        reloadTimer = reloadSpeed;
        isReloading = true;
    }

    private void Update()
    {
        if (isReloading)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }
            else if (hasTargetPos)
            {
                Shoot();
            }
        }

        if (needsToFindLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    cursorLocation = hit.point;
                    landingBall.transform.position = cursorLocation;

                    if (Input.GetMouseButtonDown(0))
                    {
                        FoundNewDestination();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelNewDestination();
            }
        }
    }

    public void FindNewDestination()
    {
        needsToFindLocation = true;
        ballComponent.mesh.enabled = true;
    }

    private void CancelNewDestination()
    {
        needsToFindLocation = false;
        landingBall.transform.position = landingPos;
        ballComponent.mesh.enabled = false;
        main.sus.destinationButton.interactable = true;
    }

    private void FoundNewDestination()
    {
        landingPos = cursorLocation;

        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        ballComponent.mesh.enabled = false;
        hasTargetPos = true;

        ballComponent.NewLocation();

        main.sus.destinationButton.interactable = true;
    }

    public void RecalculatePrice(int upgrade)
    {
        if (upgradeCount[upgrade] > 0)
            upgradeCost[upgrade] = (int)Math.Floor(upgradeBaseCost[upgrade] * upgradeCostFactor[upgrade] * upgradeCount[upgrade]);
        else
            upgradeCost[upgrade] = upgradeBaseCost[upgrade];
    }

    public void GetStat(int upgradeStat, out float statToSend)
    {
        Upgrades upgradeToCheck = upgrade[upgradeStat - 1];

        statToSend = 0f;

        switch (upgradeToCheck)
        {
            case Upgrades.ReloadSpeed:
                statToSend = tower.reloadSpeed;
                break;
            case Upgrades.AttackDamage:
                statToSend = tower.damage;
                break;
            case Upgrades.Range:
                {
                    statToSend = tower.range;
                    tower.RedrawRange();
                    break;
                }
            case Upgrades.ProjectileSpeed:
                statToSend = tower.projectileSpeed;
                break;
        }
    }

    public int Upgrade(int indexToUpgrade, int coins)
    {
        int baseCost = upgradeBaseCost[indexToUpgrade];
        int cost = upgradeCost[indexToUpgrade];

        if (coins < cost)
            return -1;

        int max = upgradeMax[indexToUpgrade];
        int count = upgradeCount[indexToUpgrade];

        if (count >= max)
            return -1;

        upgradeCount[indexToUpgrade]++;
        count++;
        float factor = upgradeFactor[indexToUpgrade];

        Upgrades upgradeToCheck = upgrade[indexToUpgrade];
        Tower tower = GetComponent<Tower>();
        switch (upgradeToCheck)
        {
            case Upgrades.ReloadSpeed:
                {
                    tower.reloadSpeed = tower.reloadSpeedBase - factor * count;
                    if (tower.reloadSpeed < 0)
                        tower.reloadSpeed = 0;
                    break;
                }
            case Upgrades.AttackDamage:
                tower.damage = tower.damageBase + factor * count;
                break;
            case Upgrades.Range:
                tower.range = tower.rangeBase + factor * count;
                break;
            case Upgrades.ProjectileSpeed:
                tower.projectileSpeed = tower.projectileSpeedBase + factor * count;
                break;
        }

        return upgradeCost[indexToUpgrade];
    }

    private void Shoot()
    {
        Debug.Log("shoot");
        isReloading = true;
        reloadTimer = reloadSpeed;
    }
}
