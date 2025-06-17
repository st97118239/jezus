using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public Enemy enemy;
    public List<Transform> waypoints;
    public int currentWPIndex;
    public bool canMove;

    [SerializeField] private int waypointToStopAt;
    
    private NavMeshAgent navMeshAgent;
    private EnemySpawner es;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        es = FindObjectOfType<EnemySpawner>();

        waypoints = es.waypoints;
    }

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        if (waypoints.Count == 0)
            return;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!collider.gameObject.CompareTag("Waypoint")) 
            return;
        
        if (currentWPIndex >= waypoints.Count - 1)
        {
            enemy.ReachedCastle();
            return;
        }
        else if (waypointToStopAt != 0 && currentWPIndex >= waypointToStopAt)
        {
            navMeshAgent.isStopped = true;
            canMove = false;
            enemy.ReachedDest();
            return;
        }
            
        currentWPIndex++;

        if (canMove)
            navMeshAgent.SetDestination(waypoints[currentWPIndex].position);
    }
}
