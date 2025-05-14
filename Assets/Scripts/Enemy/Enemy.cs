using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int coins;
    public float health;

    [SerializeField] private EnemyType enemyType; // 0 = Crawler, 1 = Demon, 2 = Necromancer, 3 = HellHound
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float projectileSpeed;

    private EnemySpawner es;
    private Main main;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
        GetComponent<NavMeshAgent>().speed = speed;
    }

    public void GotHit(float damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            es.EnemyKilled(coins, gameObject);
            Destroy(gameObject);
        }
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
}
