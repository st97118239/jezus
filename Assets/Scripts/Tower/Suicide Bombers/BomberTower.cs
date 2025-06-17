using System;
using System.Collections.Generic;
using UnityEngine;

public class BomberTower : MonoBehaviour
{
    public List<SuicideBomber> spawnedBombers;
    public Main main;
    public Tower tower;
    public GameObject projectileSpawner;
    public GameObject landingBall;
    public GameObject projectilePrefab;
    public GameObject destinationRangeObject;
    public MeshRenderer ballRenderer;
    public Collider maxHeightCollider;
    public Vector3 landingPos;
    public bool isFlyingUp;
    public bool isDisabled;

    [SerializeField] private Vector3 flyUpVelocity;
    [SerializeField] private float destinationRange;

    public List<Upgrades> upgrade; // ReloadSpeed, AttackDamage, Range, ProjectileSpeed
    public List<int> upgradeCount; // times it has been upgraded (keep at 0)
    public List<int> upgradeMax; // max amount of times it can be upgraded
    [SerializeField] private List<float> upgradeFactor; // multiplier for upgrading
    [SerializeField] private List<int> upgradeBaseCost; // the standard cost of the upgrade
    [SerializeField] private List<float> upgradeCostFactor; // multiplier for the cost of the upgrade
    public List<int> upgradeCost; // the price of the upgrade (keep at 0)

    private SuicideBomber bomber;
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
        ballRenderer = newBall.GetComponent<MeshRenderer>();
        ballRenderer.enabled = false;
        landingBall = newBall;
        tower = GetComponent<Tower>();
        reloadSpeed = tower.reloadSpeed;
        reloadTimer = reloadSpeed;
        isReloading = true;
    }

    private void Update()
    {
        if (isFlyingUp)
            GiveBomberVelocity(flyUpVelocity);

        if (isReloading)
        {
            if (reloadTimer > 0)
                reloadTimer -= Time.deltaTime;
            else if (hasTargetPos && !isDisabled)
                Shoot();
        }

        if (needsToFindLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    cursorLocation = hit.point;

                    if (Vector3.Distance(new Vector3(cursorLocation.x, 0f, cursorLocation.z), new Vector3(transform.position.x, 0f, transform.position.z)) <= tower.range)
                    {
                        landingBall.transform.position = cursorLocation;

                        if (Input.GetMouseButtonDown(0))
                            FoundNewDestination();
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                            CancelNewDestination();
                    }

                }
            }

            if (Input.GetMouseButtonDown(1))
                CancelNewDestination();
        }
    }

    public void FindNewDestination()
    {
        needsToFindLocation = true;
        ballRenderer.enabled = true;
    }

    private void CancelNewDestination()
    {
        needsToFindLocation = false;
        landingBall.transform.position = landingPos;
        ballRenderer.enabled = false;
        main.sus.destinationButton.interactable = true;
    }

    private void FoundNewDestination()
    {
        landingPos = cursorLocation;

        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        ballRenderer.enabled = false;
        hasTargetPos = true;

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
        Upgrades upgradeToCheck = upgrade[upgradeStat];

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
        Transform spawnLocation = projectileSpawner.transform;
        GameObject projectileGameObject = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);

        bomber = projectileGameObject.GetComponent<SuicideBomber>();
        bomber.SetStats(tower.damage, tower.projectileSpeed, landingPos, this);
        bomber.SetVelocity(flyUpVelocity);
        projectileGameObject.SetActive(true);
        spawnedBombers.Add(bomber);

        isFlyingUp = true;
        reloadTimer = reloadSpeed;
        isReloading = true;
    }

    private void GiveBomberVelocity(Vector3 velocity)
    {
        bomber.SetVelocity(velocity);
    }

    public void Selected(bool extraFunctions)
    {
        destinationRangeObject.transform.position = ballRenderer.transform.position;
        destinationRangeObject.transform.localScale = new Vector3(destinationRange, 0.1f, destinationRange);
        destinationRangeObject.GetComponent<MeshRenderer>().enabled = true;
        destinationRangeObject.layer = 9;
        main.os.ChangeLayerOfAllDescendants(transform, 9);

        if (extraFunctions)
            main.sus.NewTowerSelected(this);
    }

    public void Deselected()
    {
        main.sus.TowerDeselected();
        destinationRangeObject.transform.localScale = Vector3.zero;
        destinationRangeObject.GetComponent<MeshRenderer>().enabled = false;
        destinationRangeObject.layer = 0;
        main.os.ChangeLayerOfAllDescendants(transform, 10);
    }

    public void DisableTower()
    {
        isDisabled = true;
        needsToFindLocation = false;
        CancelNewDestination();
    }
}
