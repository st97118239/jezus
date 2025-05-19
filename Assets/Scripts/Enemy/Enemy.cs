using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType; // 0 = Crawler, 1 = Demon, 2 = Necromancer, 3 = HellHound
    public int startingWave = 1;
    public int waveModulo = 1;
    public int enemyAmount = 1;
    public float amountFactor = 1;
    public int coins;
    public int damage;
    public float health;
    public float tempHealth;

    [SerializeField] private float speed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool gotShotAt;
    [SerializeField] private GameObject projectileThatShot;
    [SerializeField] private float projectileDamage;

    private EnemySpawner es;
    private Main main;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
        GetComponent<NavMeshAgent>().speed = speed;
        tempHealth = health;
    }

    private void Update()
    {
        if (gotShotAt && projectileThatShot == null)
        {
            GotHit(projectileDamage);
            Debug.Log("The projectile that was aiming for " + enemyType + " is gone, removed " + projectileDamage + " from health. Health is now " + health);
        }
    }

    public void GotHit(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
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
        else
            main.ReceiveDmg(damage);

        es.activeEnemies.Remove(gameObject);
        Destroy(gameObject);
    }

    public void TowerHasShot(GameObject projectile, float damage)
    {
        gotShotAt = true;
        projectileThatShot = projectile;
        projectileDamage = damage;
    }
}
