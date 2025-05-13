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

    private PawnSpawnerTest pst;

    private void Start()
    {
        pst = FindObjectOfType(typeof(PawnSpawnerTest)).GetComponent<PawnSpawnerTest>();
        GetComponent<NavMeshAgent>().speed = speed;
    }

    public void GotHit(float damage)
    {
        health = health - damage;
        if (health <= 0)
        {
            pst.activePawns.Remove(gameObject);
            Destroy(gameObject);
        }
    }
}
