using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public List<Transform> waypoints;
    public NavMeshAgent navMeshAgent;
    public bool canMove;
    public int currentWPIndex = 0;
    public NavTestMain ntm;
    public PawnSpawnerTest pst;
    public int dmg = 3;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
