using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int coins;
    public float health;

    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float projectileSpeed;

    private EnemySpawner es;

    private void Start()
    {
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
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
}
