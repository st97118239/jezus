using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private List<GameObject> shootableEnemies = new();
    [SerializeField] private float rotationSpeed;

    private EnemySpawner es;
    private Tower tower;
    [SerializeField] private GameObject currentTarget;
    private float reloadSpeed;
    private float reloadTimer;
    private float range;
    private bool reloading;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
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
        {
            FindEnemiesInRange();
            if (shootableEnemies.Count > 0)
            {
                currentTarget = GetClosestEnemy(shootableEnemies, transform.position);
                Shoot();
            }
        }

        if (currentTarget != null)
        {
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
            currentTarget = GetClosestEnemy(shootableEnemies, transform.position);
    }

    private void FindEnemiesInRange()
    {
        shootableEnemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

        foreach (GameObject enemy in es.activeEnemies)
        {
            if (hitColliders.Any(h => h.gameObject == enemy))
            {
                shootableEnemies.Add(enemy);
            }
        }
    }

    private void Shoot()
    {
        float damage = tower.damage;
        
        GameObject closest = GetClosestEnemy(shootableEnemies, transform.position);
        tower.GetComponent<Predict>().Shoot(closest.transform, damage);

        shootableEnemies.Clear();
        reloadTimer = reloadSpeed;
        reloading = true;
    }

    GameObject GetClosestEnemy(List<GameObject> enemies, Vector3 towerPosition)
    {
        GameObject closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distanceSqr = (enemy.transform.position - towerPosition).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    public void Missed()
    {
        currentTarget.GetComponent<Enemy>().tempHealth += tower.damage;
    }
}
