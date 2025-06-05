using UnityEngine;

public class Predict : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Enemy currentTarget;

    private Tower tower;
    
    public void Start()
    {
        tower = GetComponent<Tower>();
    }

    public void ShootVelocity(GameObject enemy, Vector3 velocity, bool arcedProjectiles, float damage)
    {
        if (enemy)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent.tempHealth > 0)
            {
                enemyComponent.tempHealth -= damage;
                currentTarget = enemyComponent;

                FireProjectile(velocity, arcedProjectiles, damage, enemyComponent);
            }
        }
    }

    void FireProjectile(Vector3 velocity, bool arcedProjectiles, float damage, Enemy enemy)
    {
        Transform spawnLocation = tower.shooter.projectileSpawner.transform;
        GameObject projectileGameObject = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);

        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.SetStats(tower.shooter, tower.damage, currentTarget);
        projectile.SetVelocity(velocity, arcedProjectiles);
        projectileGameObject.SetActive(true);

        enemy.TowerHasShot(projectileGameObject, damage);
    }
}
