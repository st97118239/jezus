using Unity.VisualScripting;
using UnityEngine;

public class Predict : MonoBehaviour
{
    public GameObject projectilePrefab;

    public void Shoot(Transform target, float damage)
    {
        if (target != null)
        {
            Vector3 predictedPosition = PredictEnemyPosition(target);

            Vector3 direction = (predictedPosition - transform.position).normalized;

            FireProjectile(direction, predictedPosition, damage);
        }
    }

    Vector3 PredictEnemyPosition(Transform enemy)
    {
        Vector3 enemyPos = enemy.position;

        EnemyVelocityTracker tracker = enemy.GetComponent<EnemyVelocityTracker>();
        if (tracker == null)
        {
            Debug.LogWarning("EnemyVelocityTracker not found on target.");
            return enemyPos;
        }

        Vector3 enemyVel = tracker.Velocity;

        Vector3 shooterPos = transform.position;
        float projectileSpeed = GetComponent<Tower>().projectileSpeed;

        Vector3 toEnemy = enemyPos - shooterPos;

        float a = Vector3.Dot(enemyVel, enemyVel) - projectileSpeed * projectileSpeed;
        float b = 2 * Vector3.Dot(enemyVel, toEnemy);
        float c = Vector3.Dot(toEnemy, toEnemy);

        float discriminant = b * b - 4 * a * c;

        if (discriminant < 0 || Mathf.Abs(a) < 0.001f)
            return enemyPos;

        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float t1 = (-b + sqrtDiscriminant) / (2 * a);
        float t2 = (-b - sqrtDiscriminant) / (2 * a);

        float interceptTime = Mathf.Min(t1, t2);
        if (interceptTime < 0)
            interceptTime = Mathf.Max(t1, t2);

        if (interceptTime < 0)
            return enemyPos;

        return enemyPos + enemyVel * interceptTime;
    }

    void FireProjectile(Vector3 direction, Vector3 predictedPosition, float damage)
    {
        Transform spawnLocation = transform.Find("Shooter").transform.Find("ProjSpawn").GetComponent<Transform>();
        GameObject projectile = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);

        float projectileSpeed = GetComponent<Tower>().projectileSpeed;
        projectile.GetComponent<Projectile>().Move(predictedPosition, projectileSpeed, damage);

        projectile.SetActive(true);
    }
}
