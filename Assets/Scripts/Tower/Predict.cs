using UnityEngine;

public class Predict : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Enemy currentTarget;

    public void ShootVelocity(GameObject enemy, Vector3 velocity, float damage)
    {
        if (enemy != null)
        {
            if (enemy.GetComponent<Enemy>().tempHealth > 0)
            {
                enemy.GetComponent<Enemy>().tempHealth -= damage;

                currentTarget = enemy.GetComponent<Enemy>();

                FireProjectile(velocity, damage, enemy);
            }
        }
    }

    void FireProjectile(Vector3 velocity, float damage, GameObject target)
    {
        Transform spawnLocation = transform.Find("Shooter").transform.Find("ProjSpawn").GetComponent<Transform>();
        GameObject projectileGameObject = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);

        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.SetStats(GetComponent<Tower>().shooter, GetComponent<Tower>().damage, currentTarget);
        projectile.SetVelocity(velocity);
        projectileGameObject.SetActive(true);

        target.GetComponent<Enemy>().TowerHasShot(projectileGameObject, damage);
    }

    public void Shoot(GameObject enemy, Vector3 destination, float damage)
    {
        if (enemy != null)
        {
            if (enemy.GetComponent<Enemy>().tempHealth > 0)
            {
                enemy.GetComponent<Enemy>().tempHealth -= damage;

                Vector3 direction = (destination - enemy.transform.position).normalized;

                FireProjectile(direction, destination, damage, enemy);
            }
        }
    }

    internal Vector3? NewPredictEnemyPosition(Transform enemy)
    {
        // 1. Eerst berekenen hoelang de pijl onderweg KAN zijn
        // 2. Vervolgens berekenen we voor ELKE tijd tussen nu en (nu + tijd onderweg) wat de posities van de enemy is
        // 3. Bepaal voor elke van deze positie of de pijl een hit kan zijn
        // 4. Als het een hit kan zijn, geef dan de positie terug van de enemy op dat moment

        // 1. Bereken hoe lang een pijl onderweg kan zijn
        // Dat doen we door de maximale afstand van de toren te delen door de snelheid van de pijl. Dit geeft de maximale tijd dat een pijl onderweg kan zijn
        // Berekening in natuurkunde is afstand in meter gedeeld door snelheid in m/s geeft tijd in s
        Tower tower = GetComponent<Tower>();
        float maxFlyTime = tower.range / tower.projectileSpeed;

        Shooter shooter = tower.shooter;
        Vector3 startPositionArrow = shooter.projectileSpawner.transform.position;

        // 2. Voor elke tijd tussen nu en de maximale tijd bepalen we de positie
        float t = 0;
        while (t <= maxFlyTime)
        {
            // 3. Bepaal of het een hit kan zijn
            Vector3 targetPos = enemy.GetComponent<EnemyVelocityTracker>().PredictFuturePosition(t);
            Vector3 direction = (targetPos - startPositionArrow);

            Vector3 arrowPos = startPositionArrow + direction * tower.projectileSpeed * t;

            float distanceBetween = Vector3.Distance(arrowPos, targetPos);

            float timeToHit = distanceBetween / tower.projectileSpeed;

            direction = (targetPos - startPositionArrow) / (tower.projectileSpeed * timeToHit);

            Vector3 arrowPosNew = startPositionArrow + direction * tower.projectileSpeed * t;

            float distanceBetweenNew = Vector3.Distance(arrowPosNew, targetPos);

            Debug.DrawLine(startPositionArrow, arrowPosNew, Color.green, 5f);

            if (distanceBetweenNew <= 0.1f)
            {
                Debug.DrawLine(targetPos, targetPos + Vector3.up * 100, Color.red, 5f);
                // This is it!
                return targetPos;
            }
            //            Debug.DrawLine(targetPos, targetPos + Vector3.up * 100, Color.blue, 5f);

            //float distance = Vector3.Distance(startPositionArrow, targetPos);
            //float timeToFly = distance / tower.projectileSpeed;

            //Vector3 direction = (targetPos - startPositionArrow) / (tower.projectileSpeed * t);
            //Vector3 estimatedArrowPosition = startPositionArrow + t * tower.projectileSpeed * direction;

            //Debug.Log("TimeToFly: " + timeToFly + ", Max TimeToFly: " + maxFlyTime);
            //Debug.DrawLine(startPositionArrow, estimatedArrowPosition, Color.green, 10f);

            //float distanceBetweenArrowAndEnemy = Vector3.Distance(targetPos, estimatedArrowPosition);
            //Debug.Log("Target: " + targetPos + " Arrow: " + estimatedArrowPosition + ", Distance: " + distance + " Distance dest: " + distanceBetweenArrowAndEnemy + ", Direction: " + direction);

            //if (distanceBetweenArrowAndEnemy <= 1f) // Close range?
            //{
            //    Debug.DrawLine(targetPos, new Vector3(targetPos.x, targetPos.y + 100, targetPos.z), Color.red, 10f);
            //    // 4. Geef de positie terug van de enemy op moment nu + t
            //    return targetPos;
            //}
            //Debug.DrawLine(targetPos, new Vector3(targetPos.x, targetPos.y + 100, targetPos.z), Color.blue, 10f);

            t += 0.1f; // Time.fixedDeltaTime; // In game time obv fixed framerate
        }

        return null;

        //float projectileSpeed = GetComponent<Tower>().projectileSpeed;
        //float distance = Vector3.Distance(transform.position, enemy.position);

        //float estimatedInterceptTime = distance / projectileSpeed;

        //return enemy.GetComponent<EnemyVelocityTracker>().PredictFuturePosition(estimatedInterceptTime);
    }

    Vector3 PredictEnemyPosition(Transform enemy)
    {
        Vector3 enemyPos = enemy.position;

        if (!enemy.TryGetComponent<EnemyVelocityTracker>(out var tracker))
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

        return enemy.GetComponent<EnemyVelocityTracker>().PredictFuturePosition(interceptTime);
    }

    void FireProjectile(Vector3 direction, Vector3 predictedPosition, float damage, GameObject target)
    {
        Transform spawnLocation = transform.Find("Shooter").transform.Find("ProjSpawn").GetComponent<Transform>();
        GameObject projectile = Instantiate(projectilePrefab, spawnLocation.position, Quaternion.identity);

        float projectileSpeed = GetComponent<Tower>().projectileSpeed;
        projectile.GetComponent<Projectile>().Move(predictedPosition, projectileSpeed, damage, GetComponent<Tower>().projectileDespawnTime, GetComponent<Tower>().shooter);

        projectile.SetActive(true);

        target.GetComponent<Enemy>().TowerHasShot(projectile, damage);
    }
}
