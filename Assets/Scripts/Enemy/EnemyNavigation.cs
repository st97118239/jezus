using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public Enemy enemy;
    public List<Transform> waypoints;
    public bool canMove;
    public int currentWPIndex;

    [SerializeField] private int waypointToStopAt;
    
    private NavMeshAgent navMeshAgent;
    private Main main;
    private EnemySpawner es;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        es = FindObjectOfType(typeof(EnemySpawner)).GetComponent<EnemySpawner>();
        main = FindObjectOfType(typeof(Main)).GetComponent<Main>();
        

        waypoints = es.waypoints;

        if (!main.isDead)
            canMove = true;
    }

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        if (waypoints.Count == 0)
            return;

        //float distanceToWP = Vector3.Distance(new Vector3(waypoints[currentWPIndex].position.x, 0, waypoints[currentWPIndex].position.z), new Vector3(transform.position.x, 0, transform.position.z));

        //if (distanceToWP <= 0.25)
        //{
        //    if (currentWPIndex >= waypoints.Count - 1)
        //    {
        //        GetComponent<Enemy>().ReachedCastle();
        //        return;
        //    }
        //    currentWPIndex = (currentWPIndex + 1) % waypoints.Count;
        //}
        //if (canMove)
        //    navMeshAgent.SetDestination(waypoints[currentWPIndex].position);
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
            
        currentWPIndex = (currentWPIndex + 1);

        if (canMove)
            navMeshAgent.SetDestination(waypoints[currentWPIndex].position);
    }
}
