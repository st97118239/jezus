using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public bool canMove;

    [SerializeField] private int dmg = 3;

    private List<Transform> waypoints;
    private NavMeshAgent navMeshAgent;
    private NavTestMain ntm;
    private PawnSpawnerTest pst;
    private int currentWPIndex = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        pst = FindObjectOfType(typeof(PawnSpawnerTest)).GetComponent<PawnSpawnerTest>();
        ntm = FindObjectOfType(typeof(NavTestMain)).GetComponent<NavTestMain>();

        waypoints = pst.waypoints;

        if (!ntm.isDead)
            canMove = true;
        
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (waypoints.Count == 0)
            return;

        float distanceToWP = Vector3.Distance(new Vector3(waypoints[currentWPIndex].position.x, 0, waypoints[currentWPIndex].position.z), new Vector3(transform.position.x, 0, transform.position.z));

        if (distanceToWP <= 0.25)
        {
            if (currentWPIndex >= waypoints.Count - 1)
            {
                AttackMain();
                return;
            }
            currentWPIndex = (currentWPIndex + 1) % waypoints.Count;
        }
        if (canMove)
            navMeshAgent.SetDestination(waypoints[currentWPIndex].position);

    }

    private void AttackMain()
    {
        ntm.ReceiveDmg(dmg);
        Destroy();
    }

    private void Destroy()
    {
        print(gameObject);
        pst.activePawns.Remove(gameObject);
        Destroy(gameObject);
    }
}
