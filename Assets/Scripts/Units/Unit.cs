using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public UnitType type; // 0 = Spearman, 1 = Knight, 2 = DaVinciTank
    public BarracksTower tower;
    public Vector3 destination;
    public bool reachedDestination = false;
    public int coins;
    public int damage;
    public float health;
    public float speed;
    public float range;

    [SerializeField] private List<GameObject> reachableEnemies = new();
    [SerializeField] private GameObject currentTarget;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackTimer;
    [SerializeField] private float rotationSpeed;

    private Main main;
    private EnemySpawner es;
    private Range rangeObject;
    private bool isSelected = false;

    private void Start()
    {
        main = FindObjectOfType<Main>();
        es = FindObjectOfType<EnemySpawner>();
        rangeObject = FindObjectOfType<Range>();
        GetComponent<NavMeshAgent>().speed = speed;

        attackTimer = attackSpeed;
    }

    private void Update()
    {
        //if (transform.position == destination)
        //{
        //    reachedDestination = true;
        //}

        if (reachedDestination)
        {
            if (isAttacking)
            {
                Debug.Log(attackTimer);

                if (attackTimer > 0)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    Attack();
                }
            }

            if (currentTarget != null)
            {
                Vector3 targetDirection = currentTarget.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else
                FindEnemiesInRange();

            if (isSelected)
                rangeObject.transform.position = transform.position;
        }
    }

    public void Select()
    {
        main.up.Activate(type, (int)health);
        isSelected = true;
        RedrawRange();
    }

    public void Deselect()
    {
        main.ep.Deactivate();
        isSelected = false;
        rangeObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void Attack()
    {
        FindEnemiesInRange();

        Enemy enemy = currentTarget.GetComponent<Enemy>();

        enemy.GotHit(damage);

        reachableEnemies.Clear();
        attackTimer = attackSpeed;
        isAttacking = true;
    }

    private void FindEnemiesInRange()
    {
        reachableEnemies.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range); // Vergroten vanwege het feit dat enemies bewegen?

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

    public void IsInRange()
    {
        GetComponent<NavMeshAgent>().isStopped = true;
        reachedDestination = true;
    }

    public void RedrawRange()
    {
        rangeObject.transform.position = transform.position;
        rangeObject.transform.localScale = new Vector3(range * 2, 0.1f, range * 2);
        rangeObject.GetComponent<MeshRenderer>().enabled = true;
    }

    public void Die()
    {
        if (isSelected)
            Deselect();


        tower.spawnedUnits.Remove(this);
        Destroy(gameObject);
    }
}
