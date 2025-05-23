using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class TowerUpgrades : MonoBehaviour
{
    public List<Upgrades> upgrade; // ReloadSpeed, AttackDamage, Range, ProjectileSpeed
    public List<int> upgradeCount; // times it has been upgraded (keep at 0)
    public List<int> upgradeMax; // max amount of times it can be upgraded
    [SerializeField] private List<float> upgradeFactor; // multiplier for upgrading
    [SerializeField] private List<int> upgradeBaseCost; // the standard cost of the upgrade
    [SerializeField] private List<float> upgradeCostFactor; // multiplier for the cost of the upgrade
    public List<int> upgradeCost; // the price of the upgrade (keep at 0)

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
                statToSend = GetComponent<Tower>().reloadSpeed;
                break;
            case Upgrades.AttackDamage:
                statToSend = GetComponent<Tower>().damage;
                break;
            case Upgrades.Range:
                {
                    statToSend = GetComponent<Tower>().range;
                    GetComponent<Tower>().RedrawRange();
                    break;
                }
            case Upgrades.ProjectileSpeed:
                statToSend = GetComponent<Tower>().projectileSpeed;
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
}
