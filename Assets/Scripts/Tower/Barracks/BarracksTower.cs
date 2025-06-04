using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BarracksTower : MonoBehaviour
{
    public List<Unit> units;
    public List<Unit> spawnedUnits;
    public GameObject destinationBall;
    public DestinationBall ballComponent;
    public Vector3 destination;
    public Vector3 spawnOffset;
    public int upgradeCount;
    public int maxUnits;

    private Main main;
    private Tower tower;
    private Vector3 cursorLocation;
    private bool needsToFindLocation;

    private void Start()
    {
        main = FindObjectOfType<Main>();
        GameObject newBall = Instantiate(destinationBall, transform.position + spawnOffset, Quaternion.identity);
        ballComponent = newBall.GetComponent<DestinationBall>();
        ballComponent.tower = this;
        ballComponent.mesh = ballComponent.GetComponent<MeshRenderer>();
        ballComponent.mesh.enabled = false;
        destinationBall = newBall;
        tower = GetComponent<Tower>();
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
        newUnit.agent = newUnit.GetComponent<NavMeshAgent>();
        NavMeshAgent newUnitAgent = newUnit.agent;

        newUnitAgent.SetDestination(destination);
        spawnedUnits.Add(newUnit);
        newUnit.tower = this;
    }

    public void FindNewDestination()
    {
        needsToFindLocation = true;
        ballComponent.mesh.enabled = true;
    }

    private void CancelNewDestination()
    {
        needsToFindLocation = false;
        destinationBall.transform.position = cursorLocation;
        ballComponent.mesh.enabled = false;
        main.bus.destinationButton.interactable = true;
    }

    private void FoundNewDestination()
    {
        destination = cursorLocation;
        Debug.Log(destination);

        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        ballComponent.mesh.enabled = false;

        foreach (Unit unit in spawnedUnits)
        {
            unit.agent.SetDestination(destination);
            unit.NewDestination(destination);
            unit.agent.isStopped = false;
        }

        ballComponent.NewLocation();

        main.bus.destinationButton.interactable = true;
    }
}
