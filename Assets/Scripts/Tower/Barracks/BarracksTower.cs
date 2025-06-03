using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BarracksTower : MonoBehaviour
{
    public List<Unit> units;
    public List<Unit> spawnedUnits;
    public Vector3 destination;
    public Vector3 spawnOffset;
    public int upgradeCount;
    public int maxUnits;

    private Main main;
    private GameObject destinationBall;
    private Vector3 cursorLocation;
    private bool needsToFindLocation;

    private void Start()
    {
        main = FindObjectOfType<Main>();
        destinationBall = FindObjectOfType<DestinationBall>().gameObject;
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
    }

    private void CancelNewDestination()
    {
        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        destinationBall.transform.position = cursorLocation;
    }

    private void FoundNewDestination()
    {
        destination = cursorLocation;
        Debug.Log(destination);

        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        destinationBall.transform.position = cursorLocation;

        foreach (Unit unit in spawnedUnits)
        {
            unit.GetComponent<NavMeshAgent>().SetDestination(destination);
        }
    }
}
