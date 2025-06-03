using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Shooter : MonoBehaviour
{
    public GameObject projectileSpawner;
    public bool canShoot = false;

    [SerializeField] private List<GameObject> shootableEnemies = new();
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private float rotationSpeed;

    private EnemySpawner es;
    private Tower tower;
    private float reloadSpeed;
    private float reloadTimer;
    private float range;
    private bool reloading;

    private void Start()
    {
        es = FindObjectOfType<EnemySpawner>();
        tower = transform.parent.GetComponent<Tower>();
        projectileSpawner = transform.Find("ProjSpawn").gameObject;
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
            if (canShoot)
            {
                FindEnemiesInRange(); // Vult shootableEnemies
                if (shootableEnemies.Count > 0)
                {
                    Predict p = tower.GetComponent<Predict>();

                    bool isFired = false;
                    foreach (GameObject enemy in shootableEnemies)
                    {
                        NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
                        for (int i = 0; i < enemyAgent.path.corners.Length; i++)
                        {
                            Vector3 targetPosition = enemyAgent.path.corners[i];
                            float tEnemy = GetTimeToReachPoint(targetPosition, enemyAgent);

                            if (tEnemy > 0)
                            {
                                Vector3 velocity = GetArrowVelocity(targetPosition, tEnemy);
                                if (velocity != Vector3.zero)
                                {
                                    ShootVelocity(enemy, velocity);
                                    isFired = true;
                                    break;
                                }
                            }

                        }

                        if (isFired)
                            break;

                        //Vector3? enemyPosition = p.NewPredictEnemyPosition(enemy.transform);
                        //if (enemyPosition != null)
                        //{
                        //    currentTarget = enemy;
                        //    Shoot(enemy, enemyPosition.Value);
                        //    break;
                        //}
                    }
                }
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

    private float GetTimeToReachPoint(Vector3 targetPosition, NavMeshAgent enemyAgent)
    {
        Vector3 displacement = targetPosition - enemyAgent.transform.position;
        float time = Vector3.Magnitude(displacement) / enemyAgent.velocity.magnitude;

        return time < 0.1f ? -1 : time;
    }

    //    public Vector3 GetGravityAwareArrowVelocity(
    //    Vector3 launchPoint,
    //    Vector3 targetPosition,
    //    float arrowSpeed,
    //    float gravity = Physics.gravity.magnitude,
    //    float arcScale = 1.0f
    //)
    //    {
    //        // Compute displacement and distance
    //        Vector3 displacement = targetPosition - launchPoint;
    //        float distanceToTarget = displacement.magnitude;

    //        // Handle edge case: no distance
    //        if (distanceToTarget < 0.01f) return Vector3.zero;

    //        // Step 1: Calculate horizontal velocity using distance and speed
    //        float t = distanceToTarget / arrowSpeed; // Time of flight (ignoring gravity)
    //        Vector3 horizontalVelocity = displacement.normalized * arrowSpeed;

    //        // Step 2: Calculate vertical velocity to counteract gravity
    //        float vy = (displacement.y + 0.5f * gravity * t * t) / t;

    //        // Step 3: Scale for flatter arc (adjust as needed)
    //        horizontalVelocity *= arcScale;
    //        vy *= arcScale;

    //        return new Vector3(horizontalVelocity.x, vy, horizontalVelocity.z);
    //    }

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

        // Calculate required velocity components
        float vx = displacement.x / t;
        float vz = displacement.z / t;

        // Solve for vy: y(t) = y0 + vy * t - 0.5 * g * t^2
        // Rearranged: vy = [y(t) - y0 + 0.5 * g * t^2] / t
        float vy = (targetPosition.y - launchPoint.position.y + 0.5f * Physics.gravity.magnitude * t * t) / t;

        return new Vector3(vx, vy, vz);
    }

    private Vector3 OldGetArrowVelocity(Vector3 targetPosition, float t, float arcScale = 1.0f)
    {
        Transform launchPoint = tower.shooter.projectileSpawner.transform;
        float arrowSpeed = tower.projectileSpeed;

        Vector3 displacement = targetPosition - launchPoint.position;
        float distanceToTarget = Vector3.Distance(launchPoint.position, targetPosition);

        float maxDistance = arrowSpeed * t;
        if (distanceToTarget > maxDistance)
            return Vector3.zero;

        Vector3 horizontalVelocity = displacement.normalized * arrowSpeed;

        float g = Physics.gravity.magnitude;
        float vy = (displacement.y + 0.5f * g * t * t) / t;

        horizontalVelocity *= arcScale;
        vy *= arcScale;

        return new Vector3(horizontalVelocity.x, vy, horizontalVelocity.z);
    }

    private Vector3 GetArrowVelocityArced(Vector3 targetPosition, float t)
    {
        Transform launchPoint = tower.shooter.projectileSpawner.transform;
        float arrowSpeed = tower.projectileSpeed;

        Vector3 displacement = targetPosition - launchPoint.position;
        float distanceToTarget = Vector3.Distance(launchPoint.position, targetPosition);

        float maxDistance = arrowSpeed * t;
        if (distanceToTarget > maxDistance)
            return Vector3.zero;

        float vx = displacement.x / t;
        float vz = displacement.z / t;

        // Physics baby!
        // vy: y(t) = y0 + vy * t - 0.5 * g * t^2
        float vy = (targetPosition.y - launchPoint.position.y + 0.5f * Physics.gravity.magnitude * t * t) / t;

        return new Vector3(vx, vy, vz);
    }

    private void FindEnemiesInRange()
    {
        shootableEnemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range); // Vergroten vanwege het feit dat enemies bewegen?

        foreach (GameObject enemy in es.activeEnemies)
        {
            if (hitColliders.Any(h => h.gameObject == enemy))
            {
                shootableEnemies.Add(enemy);
            }
        }
    }

    private void ShootVelocity(GameObject enemy, Vector3 velocity)
    {
        float damage = tower.damage;

        tower.GetComponent<Predict>().ShootVelocity(enemy, velocity, damage);

        shootableEnemies.Clear();
        reloadTimer = reloadSpeed;
        reloading = true;
    }

    private void Shoot(GameObject enemy, Vector3 destination)
    {
        float damage = tower.damage;
        
        tower.GetComponent<Predict>().Shoot(enemy, destination, damage);

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
}
