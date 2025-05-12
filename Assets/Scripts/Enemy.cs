using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health;
    public int coins;

    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float projectileSpeed;

    private void Start()
    {
        GetComponent<NavMeshAgent>().speed = speed;
    }
}
