using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int coins;
    public float health;
    public float tempHealth;

    [SerializeField] private EnemyType enemyType; // 0 = Crawler, 1 = Demon, 2 = Necromancer, 3 = HellHound
    [SerializeField] private int damage;
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
            health -= projectileDamage;
            Debug.Log("projectile is gone, removed " + projectileDamage + " from health. Health is now " + health);
        }
    }

    public void GotHit(float damage)
    {
        health = health - damage;

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

        es.activePawns.Remove(gameObject);
        Destroy(gameObject);
    }

    public void TowerHasShot(GameObject projectile, float damage)
    {
        gotShotAt = true;
        projectileThatShot = projectile;
    }
}
