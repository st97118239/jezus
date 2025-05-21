using System;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrades : MonoBehaviour
{
    public List<Upgrades> upgrade; // ReloadSpeed, AttackDamage, Range, ProjectileSpeed
    public List<int> upgradeCount;
    public List<int> upgradeMax;
    [SerializeField] private List<float> upgradeFactor;
    [SerializeField] private List<int> upgradeBaseCost;
    [SerializeField] private List<float> upgradeCostFactor;
    public List<int> upgradeCost;

    public void RecalculatePrice(int upgrade)
    {
        upgradeCost[upgrade] = (int)Math.Floor(upgradeBaseCost[upgrade] + upgradeCostFactor[upgrade] * upgradeCount[upgrade]);
    }

    public void GetStat(int upgradeStat, out float statToSend)
    {
        Upgrades upgradeToCheck = upgrade[upgradeStat - 1];

        if ((int)upgradeToCheck == 0)
            statToSend = GetComponent<Tower>().reloadSpeed;
        else if ((int)upgradeToCheck == 1)
            statToSend = GetComponent<Tower>().damage;
        else if ((int)upgradeToCheck == 2)
            statToSend = GetComponent<Tower>().range;
        else if ((int)upgradeToCheck == 3)
            statToSend = GetComponent<Tower>().projectileSpeed;
        else
            statToSend = 0f;

    }
}
