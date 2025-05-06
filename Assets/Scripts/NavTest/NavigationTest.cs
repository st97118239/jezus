using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTest : MonoBehaviour
{
    public List<Transform> waypoints;
    public NavMeshAgent navMeshAgent;
    public int currentWPIndex = 0;
    public NavTestMain ntm;
    public int dmg = 3;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (waypoints.Count == 0)
        {
            return;
        }

        float distanceToWP = Vector3.Distance(waypoints[currentWPIndex].position, transform.position);

        if (distanceToWP <= 1)
        {
            if (currentWPIndex >= waypoints.Count - 1)
            {
                AttackMain();
                return;
            }
            currentWPIndex = (currentWPIndex + 1) % waypoints.Count;
        }

            navMeshAgent.SetDestination(waypoints[currentWPIndex].position);
    }

    private void AttackMain()
    {
        ntm.ReceiveDmg(dmg);
        Destroy(gameObject);
    }
}
