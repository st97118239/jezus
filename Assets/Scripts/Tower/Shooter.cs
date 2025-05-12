using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private PawnSpawnerTest pst;
    private Tower tower;
    [SerializeField] private List<GameObject> shootableEnemies = new();
    private float reloadSpeed;
    private float reloadTimer;
    private bool reloading;
    private float range;

    private void Start()
    {
        pst = FindObjectOfType(typeof(PawnSpawnerTest)).GetComponent<PawnSpawnerTest>();
        tower = transform.parent.GetComponent<Tower>();
        reloadSpeed = tower.reloadSpeed;
        range = tower.range;
    }

    private void Update()
    {
        if (reloading)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= Time.deltaTime;
            }
            else
                reloading = false;
        }
        else
            Shoot();
    }

    private void FindEnemiesInRange()
    {
        shootableEnemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (GameObject enemy in pst.activePawns)
        {
            if (hitColliders.Any(h => h.gameObject == enemy))
            {
                shootableEnemies.Add(enemy);
            }
        }
    }

    private void Shoot()
    {
        FindEnemiesInRange();
    }
}
