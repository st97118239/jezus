using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public UnitType type; // 0 = Spearman, 1 = Knight, 2 = DaVinciTank
    public BarracksTower tower;
    public NavMeshAgent agent;
    public BoxCollider boxCollider;
    public Vector3 destination;
    public bool atDestination;
    public bool hasReachedDestination;
    public bool isFollowingEnemy;
    public int damage;
    public int price;
    public float health;
    public float speed;
    public float range;
    public float extraDistanceToFindEnemiesIn = 3;

    [SerializeField] private List<GameObject> reachableEnemies = new();
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private bool isAttacking;
    [SerializeField] private float maxDistanceToLeave = 10;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackTimer;
    [SerializeField] private float rotationSpeed;

    private Main main;
    private EnemySpawner es;
    private Range rangeObject;
    private MeshRenderer rangeRenderer;
    private bool isSelected;

    private void Start()
    {
        main = FindObjectOfType<Main>();
        es = FindObjectOfType<EnemySpawner>();
        rangeObject = FindObjectOfType<Range>();
        GetComponent<NavMeshAgent>().speed = speed;
        rangeRenderer = rangeObject.GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        attackTimer = attackSpeed;
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

            if (currentTarget)
            {
                Vector3 targetDirection = currentTarget.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            //else
            //    FindEnemiesInRange(range);
        }

        if (isSelected)
            rangeObject.transform.position = transform.position;

        if (isFollowingEnemy)
            FollowEnemy();
    }

    public void Select()
    {
        main.up.Activate(type, (int)health);
        isSelected = true;
        RedrawRange();
    }

    public void Deselect()
    {
        main.up.Deactivate();
        isSelected = false;
        rangeRenderer.enabled = false;
    }

    private void Attack()
    {
        if (hasReachedDestination && Vector3.Distance(destination, transform.position) <= maxDistanceToLeave)
            FindEnemiesInRange(range);

        if (!currentTarget)
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
                Debug.Log(enemy);
            }
        }

        reachableEnemies = reachableEnemies.OrderBy((d) => (d.gameObject.transform.position - transform.position).sqrMagnitude).ToList();

        if (reachableEnemies.Count > 0)
            currentTarget = reachableEnemies[0];
    }

    public void IsInRange()
    {
        agent.isStopped = true;
        atDestination = true;
        hasReachedDestination = true;
    }

    public void NewDestination(Vector3 position)
    {
        atDestination = false;
        isAttacking = false;
        destination = position;
    }

    public void NewDestinationPoint(Vector3 position)
    {
        atDestination = false;
        hasReachedDestination = false;
        isAttacking = false;
        destination = position;
    }

    public void RedrawRange()
    {
        rangeObject.transform.position = transform.position;
        rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
        rangeRenderer.enabled = true;
    }

    public void Die()
    {
        if (isSelected)
            Deselect();


        tower.spawnedUnits.Remove(this);
        Destroy(gameObject);
    }
}
