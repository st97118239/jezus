using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public EnemyNavigation nav;
    public EnemyType enemyType; // 0 = Crawler, 1 = Demon, 2 = Necromancer, 3 = HellHound
    public int startingWave = 1;
    public int waveModulo = 1;
    public int enemyAmount = 1;
    public int coins;
    public int damage;
    public float amountFactor = 1;
    public float healthFactor = 1;
    public float damageFactor = 1;
    public float health;
    public float tempHealth;
    public float speed;
    public float range;
    public int amountOfEnemiesToSpawn;

    [SerializeField] private Enemy enemyToSpawn;
    [SerializeField] private float spawnForwardMultiplier;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float projectileSpeed;

    private EnemySpawner es;
    private Main main;
    private bool isSelected = false;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackTimer;

    private void Awake()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

        nav = GetComponent<EnemyNavigation>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        tempHealth = health;

        attackTimer = attackSpeed;
    }

    private void Update()
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
    }

    public void GotHit(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            if (isSelected)
                Deselect();

            es.EnemyKilled(coins, gameObject);
            Destroy(gameObject);
        }
    }

    public void ReachedCastle()
    {
        if (enemyType == EnemyType.Crawler)
        {
            int dmgToDo = (int)health;
            main.ReceiveDmg(dmgToDo);
        }
        else if (!isAttacking)
            Attack();

        if (isSelected)
            Deselect();

        es.activeEnemies.Remove(gameObject);
        if (enemyType == 0)
            Destroy(gameObject);
    }
    
    public void ReachedDest()
    {
        if (enemyType == EnemyType.Necromancer)
        {
            Attack();
        }
    }

    private void Summon()
    {
        for (int i = 0; i < amountOfEnemiesToSpawn; i++)
        {
            Enemy spawnedEnemy = Instantiate(enemyToSpawn, transform.position + transform.forward * spawnForwardMultiplier, transform.rotation);

            spawnedEnemy.nav.currentWPIndex = nav.currentWPIndex;
            es.activeEnemies.Add(spawnedEnemy.gameObject);
        }
    }

    public void Select()
    {
        main.ep.Activate(enemyType, (int)health);
        isSelected = true;
    }

    public void Deselect()
    {
        main.ep.Deactivate();
        isSelected = false;
    }

    private void Attack()
    {
        if (enemyType == EnemyType.Necromancer)
            Summon();
        else
            main.ReceiveDmg(damage);

        attackTimer = attackSpeed;
        isAttacking = true;
    }
}
