using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Shooter : MonoBehaviour
{
    public Tower tower;
    public AudioSource shootSound;
    public GameObject projectileSpawner;
    public bool canShoot;

    [SerializeField] private TargetType targetType;
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private bool arcedProjectiles;
    [SerializeField] private int waypointsToLookAhead = 5;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float verticalArcFactor;
    
    private readonly List<KeyValuePair<GameObject, NavMeshAgent>> shootableEnemies = new();
    private EnemySpawner es;
    private Predict predict;
    private bool reloading;
    private float projectileSpeed;
    private float reloadSpeed;
    private float reloadTimer;
    private float range;
    
    private void Start()
    {
        shootSound = GetComponent<AudioSource>();
        es = FindObjectOfType<EnemySpawner>();
        tower = transform.parent.GetComponent<Tower>();
        predict = tower.GetComponent<Predict>();
        projectileSpawner = transform.Find("ProjSpawn").gameObject;
        reloadSpeed = tower.reloadSpeed;
        range = tower.range;
        projectileSpeed = tower.projectileSpeed;
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
            if (canShoot)
            {
                FindEnemiesInRange();
                if (shootableEnemies.Count > 0)
                {
                    bool isFired = false;
                    foreach (KeyValuePair<GameObject, NavMeshAgent> enemy in shootableEnemies)
                    {
                        EnemyNavigation nav = enemy.Key.GetComponent<EnemyNavigation>();
                        NavMeshAgent enemyAgent = enemy.Value;
                        Enemy enemyComponent = nav.enemy;

                        if (enemyComponent.tempHealth <= 0)
                            return;

                        if (!nav.canMove)
                        {
                            float tEnemy = Vector3.Distance(nav.transform.position, projectileSpawner.transform.position) / projectileSpeed;
                            
                            Vector3 velocity = GetArrowVelocity(nav.transform.position, tEnemy);
                            if (velocity != Vector3.zero)
                            {
                                ShootVelocity(enemy.Key, velocity, tEnemy);
                                isFired = true;
                                currentTarget = enemy.Key;
                                break;
                            }
                        }
                        
                        int startIdx = Math.Max(1, nav.currentWPIndex - 1);
                        for (var i = startIdx; i < Math.Min(nav.waypoints.Count, nav.currentWPIndex + waypointsToLookAhead); i++)
                        {
                            Vector3 targetPosition = i < startIdx + enemyAgent.path.corners.Length ? enemyAgent.path.corners[i - startIdx] : nav.waypoints[i].position;
                            float tEnemy = GetTimeToReachPoint(targetPosition, enemyAgent);

                            if (tEnemy > 0)
                            {
                                Vector3 velocity = GetArrowVelocity(targetPosition, tEnemy);
                                if (velocity != Vector3.zero)
                                {
                                    ShootVelocity(enemy.Key, velocity, tEnemy);
                                    isFired = true;
                                    currentTarget = enemy.Key;
                                    break;
                                }
                            }
                        }

                        if (isFired)
                            break;
                    }
                }
            }
        }

        if (currentTarget)
        {
            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            if (arcedProjectiles)
                targetDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
            currentTarget = GetClosestEnemy(shootableEnemies, transform.position);
    }

    private float GetTimeToReachPoint(Vector3 targetPosition, NavMeshAgent enemyAgent)
    {
        Vector3 displacement = targetPosition - enemyAgent.transform.position;

        float speed = enemyAgent.velocity.magnitude;
        if (speed <= 0.001f)
            return -1;

        float time = Vector3.Magnitude(displacement) / speed;
        return time < 0.1f ? -1 : time;
    }
    
    Vector3 GetArrowVelocity(Vector3 targetPosition, float t)
    {
        Transform launchPoint = tower.shooter.projectileSpawner.transform;
        float arrowSpeed = tower.projectileSpeed;

        // Calculate the required velocity vector to hit the point at time t
        Vector3 displacement = targetPosition - launchPoint.position;
        float distanceToTarget = Vector3.Distance(launchPoint.position, targetPosition);

        // Check if the arrow can reach the target at time t
        float maxDistance = arrowSpeed * t;
        if (distanceToTarget > maxDistance)
            return Vector3.zero;

        if (!arcedProjectiles)
        {
            Vector3 direction = displacement.normalized;
            Vector3 velocity = direction * arrowSpeed;
            return velocity;
        }

        // Calculate required velocity components
        float vx = displacement.x / t;
        float vz = displacement.z / t;

        // Solve for vy: y(t) = y0 + vy * t - 0.5 * g * t^2
        // Rearranged: vy = [y(t) - y0 + 0.5 * g * t^2] / t
        float vy = (targetPosition.y - launchPoint.position.y + 0.5f * Physics.gravity.magnitude * t * t) / t;
        
        return new Vector3(vx, vy * verticalArcFactor, vz);
    }

    private void FindEnemiesInRange()
    {
        shootableEnemies.Clear();

        List<GameObject> tempEnemiesList = new();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range); // Vergroten vanwege het feit dat enemies bewegen?

        foreach (GameObject enemy in es.activeEnemies)
        {
            if (hitColliders.Any(h => h.gameObject == enemy))
            {
                tempEnemiesList.Add(enemy);
            }
        }

        if (targetType == TargetType.Closest)
            tempEnemiesList = tempEnemiesList.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToList();

        foreach (GameObject enemy in tempEnemiesList)
        {
            shootableEnemies.Add(new KeyValuePair<GameObject, NavMeshAgent>(enemy, enemy.GetComponent<NavMeshAgent>()));
        }
    }

    private void ShootVelocity(GameObject enemy, Vector3 velocity, float time)
    {
        float damage = tower.damage;

        predict.ShootVelocity(enemy, velocity, arcedProjectiles, damage, time);

        shootableEnemies.Clear();
        reloadTimer = reloadSpeed;
        reloading = true;
    }

    GameObject GetClosestEnemy(List<KeyValuePair<GameObject, NavMeshAgent>> enemies, Vector3 towerPosition)
    {
        GameObject closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (KeyValuePair<GameObject, NavMeshAgent> enemy in enemies)
        {
            if (!enemy.Key) continue;

            float distanceSqr = (enemy.Key.transform.position - towerPosition).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestEnemy = enemy.Key;
            }
        }

        return closestEnemy;
    }

    public void PlayShootSoundEffect()
    {
        if (shootSound && shootSound.clip)
            shootSound.Play();
    }
}
