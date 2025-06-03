using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarracksTower : MonoBehaviour
{
    public List<Unit> units;
    public List<Unit> spawnedUnits;
    public GameObject destinationBall;
    public Vector3 destination;
    public Vector3 spawnOffset;
    public int upgradeCount;
    public int maxUnits;

    private Main main;
    private Vector3 cursorLocation;
    private bool needsToFindLocation;

    private void Start()
    {
        main = FindObjectOfType<Main>();
        GameObject newBall = Instantiate(destinationBall, transform.position + spawnOffset, Quaternion.identity);
        newBall.GetComponent<DestinationBall>().tower = this;
        newBall.GetComponent<MeshRenderer>().enabled = false;
        destinationBall = newBall;
    }

    void Update()
    {
        if (needsToFindLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    cursorLocation = hit.point;
                    destinationBall.transform.position = cursorLocation;

                    if (Input.GetMouseButtonDown(0))
                    {
                        FoundNewDestination();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelNewDestination();
            }
        }

        if (destination != Vector3.zero)
        {
            if (spawnedUnits.Count < maxUnits)
            {
                SpawnUnit();
            }
        }
    }

    private void SpawnUnit()
    {
        Unit newUnit = Instantiate(units[0 + upgradeCount], transform.position + spawnOffset, Quaternion.identity);
        NavMeshAgent newUnitAgent = newUnit.GetComponent<NavMeshAgent>();

        newUnitAgent.SetDestination(destination);
        spawnedUnits.Add(newUnit);
    }

    public void FindNewDestination()
    {
        needsToFindLocation = true;
        destinationBall.GetComponent<MeshRenderer>().enabled = true;
    }

    private void CancelNewDestination()
    {
        needsToFindLocation = false;
        destinationBall.transform.position = cursorLocation;
        destinationBall.GetComponent<MeshRenderer>().enabled = false;
    }

    private void FoundNewDestination()
    {
        destination = cursorLocation;
        Debug.Log(destination);

        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        destinationBall.GetComponent<MeshRenderer>().enabled = false;

        foreach (Unit unit in spawnedUnits)
        {
            unit.GetComponent<NavMeshAgent>().SetDestination(destination);
            unit.reachedDestination = false;
            unit.GetComponent<NavMeshAgent>().isStopped = false;
        }

        destinationBall.GetComponent<DestinationBall>().NewLocation();
    }
}
