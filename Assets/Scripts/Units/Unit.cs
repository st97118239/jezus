using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : BaseUnit
{
    public bool isFollowingEnemy;

    [SerializeField] private List<GameObject> reachableEnemies = new();
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private float maxDistanceToLeave = 10;

    protected override void OnStart()
    {
        // Speciale code die alleen voor deze classe geldt
    }

    private void Update()
    {
        if (hasReachedDestination)
        {
            if (isAttacking)
            {
                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    Attack();
                }
            }
            else
                isAttacking = true;

            if (currentTarget)
            {
                Vector3 targetDirection = currentTarget.transform.position - transform.position;
                targetDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }

        if (isSelected)
            rangeObject.transform.position = transform.position;

        if (isFollowingEnemy)
            FollowEnemy();
    }

    protected override void Attack()
    {
        if (hasReachedDestination && Vector3.Distance(destination, transform.position) <= maxDistanceToLeave)
            FindEnemiesInRange(range);

        if (!currentTarget || Vector3.Distance(currentTarget.transform.position, transform.position) > range)
        {
            MoveToEnemies();
            return;
        }

        Enemy enemy = currentTarget.GetComponent<Enemy>();

        if (Vector3.Distance(transform.position, currentTarget.transform.position) > range)
            return;

        enemy.GotHit(damage);
        reachableEnemies.Clear();
        attackTimer = attackSpeed;
        isAttacking = true;
    }

    private void MoveToEnemies()
    {
        FindEnemiesInRangeOfDestination(tower.unitRange + extraDistanceToFindEnemiesIn);

        if (!currentTarget)
        {
            if (!atDestination)
                GoBackToDestination();

            return;
        }

        if (Vector3.Distance(currentTarget.transform.position, destination) > range)
        {
            agent.isStopped = false;
            isFollowingEnemy = true;
        }
    }

    private void FollowEnemy()
    {
        if (!currentTarget)
        {
            currentTarget = null;
            return;
        }

        Vector3 enemyPosition = currentTarget.transform.position;

        if (Vector3.Distance(enemyPosition, transform.position) > maxDistanceToLeave)
        {
            GoBackToDestination();
            return;
        }

        agent.SetDestination(enemyPosition);
    }

    private void GoBackToDestination()
    {
        isFollowingEnemy = false;
        NewDestination(destination);
        agent.SetDestination(destination);
    }

    private void FindEnemiesInRange(float radius)
    {
        reachableEnemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (GameObject enemy in es.activeEnemies)
        {
            if (hitColliders.Any(h => h.gameObject == enemy))
            {
                reachableEnemies.Add(enemy);
            }
        }

        reachableEnemies = reachableEnemies.OrderBy((d) => (d.gameObject.transform.position - transform.position).sqrMagnitude).ToList();

        if (reachableEnemies.Count > 0)
            currentTarget = reachableEnemies[0];
    }

    private void FindEnemiesInRangeOfDestination(float radius)
    {
        reachableEnemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(destination, radius);

        foreach (GameObject enemy in es.activeEnemies)
        {
            if (hitColliders.Any(h => h.gameObject == enemy))
            {
                reachableEnemies.Add(enemy);
            }
        }

        reachableEnemies = reachableEnemies.OrderBy((d) => (d.gameObject.transform.position - transform.position).sqrMagnitude).ToList();

        if (reachableEnemies.Count > 0)
            currentTarget = reachableEnemies[0];
    }
}
