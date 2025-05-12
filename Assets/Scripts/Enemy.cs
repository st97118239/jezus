using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health;

    [SerializeField] private int damage;
    [SerializeField] private float speed;

    private void Start()
    {
        GetComponent<NavMeshAgent>().speed = speed;
    }
}
