using System;
using System.Collections.Generic;
using UnityEngine;

public class BomberTower : MonoBehaviour
{
    public GameObject projectileSpawner;
    public GameObject landingBall;
    public GameObject projectilePrefab;
    public LandingBall ballComponent;
    public Collider maxHeightCollider;
    public Vector3 landingPos;
    public bool isFlyingUp;

    [SerializeField] private Vector3 flyUpVelocity;

    public List<Upgrades> upgrade; // ReloadSpeed, AttackDamage, Range, ProjectileSpeed
    public List<int> upgradeCount; // times it has been upgraded (keep at 0)
    public List<int> upgradeMax; // max amount of times it can be upgraded
    [SerializeField] private List<float> upgradeFactor; // multiplier for upgrading
    [SerializeField] private List<int> upgradeBaseCost; // the standard cost of the upgrade
    [SerializeField] private List<float> upgradeCostFactor; // multiplier for the cost of the upgrade
    public List<int> upgradeCost; // the price of the upgrade (keep at 0)

    private Main main;
    private SuicideBomber bomber;
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
        if (isFlyingUp)
        {
            GiveBomberVelocity(flyUpVelocity);
        }

        if (isReloading)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }
            else if (hasTargetPos)
            {
                FlyUp();
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

    private void FlyUp()
    {
        Shoot(flyUpVelocity);
        isFlyingUp = true;
    }

    

    private void Shoot(Vector3 velocity)
    {
        float damage = tower.damage;

        FireProjectile(velocity, damage);

        reloadTimer = reloadSpeed;
        isReloading = true;
    }

    private void FireProjectile(Vector3 velocity, float damage)
    {
        Transform spawnLocation = projectileSpawner.transform;
        GameObject projectileGameObject = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);

        bomber = projectileGameObject.GetComponent<SuicideBomber>();
        bomber.SetStats(tower.damage, landingPos, this);
        bomber.SetVelocity(velocity);
        projectileGameObject.SetActive(true);
    }

    private void GiveBomberVelocity(Vector3 velocity)
    {
        bomber.SetVelocity(velocity);
    }
}
