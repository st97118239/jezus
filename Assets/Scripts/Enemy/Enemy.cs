using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
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

    [SerializeField] private float attackSpeed;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool gotShotAt;
    [SerializeField] private GameObject projectileThatShot;
    [SerializeField] private float projectileDamage;

    private EnemySpawner es;
    private Main main;
    private bool isSelected = false;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackTimer;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();

        GetComponent<NavMeshAgent>().speed = speed;
        tempHealth = health;

        attackTimer = attackSpeed;
    }

    private void Update()
    {
        if (gotShotAt && projectileThatShot == null)
        {
            GotHit(projectileDamage);
            Debug.Log("The projectile that was aiming for " + enemyType + " is gone, removed " + projectileDamage + " from health. Health is now " + health);
        }

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
    }

    public void GotHit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (isSelected)
                Deselect();

            es.EnemyKilled(coins, gameObject);
            Destroy(gameObject);
        }

        gotShotAt = false;
    }

    public void ReachedCastle()
    {
        if (enemyType == 0)
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

    public void TowerHasShot(GameObject projectile, float damage)
    {
        gotShotAt = true;
        projectileThatShot = projectile;
        projectileDamage = damage;
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
        main.ReceiveDmg(damage);
        attackTimer = attackSpeed;
        isAttacking = true;
    }
}
