using UnityEngine;

public class Predict : MonoBehaviour
{
    public GameObject projectilePrefab;

    [SerializeField] private float extraTargetAmount;
    
    private Enemy currentTarget;
    private Tower tower;
    
    public void Start()
    {
        tower = GetComponent<Tower>();
    }

    public void ShootVelocity(GameObject enemy, Vector3 velocity, bool arcedProjectiles, float damage, float time)
    {
        if (enemy)
        {
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent.tempHealth > 0)
            {
                enemyComponent.tempHealth -= damage;
                currentTarget = enemyComponent;

                FireProjectile(velocity, arcedProjectiles, damage, enemyComponent, time);
            }
        }
    }

    void FireProjectile(Vector3 velocity, bool arcedProjectiles, float damage, Enemy enemy, float time)
    {
        Transform spawnLocation = tower.shooter.projectileSpawner.transform;
        GameObject projectileGameObject = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);

        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.SetStats(tower.shooter, damage, enemy, time, tower.projectileSpeed);
        projectile.SetVelocity(velocity, arcedProjectiles, extraTargetAmount);
        projectileGameObject.SetActive(true);
    }
}
