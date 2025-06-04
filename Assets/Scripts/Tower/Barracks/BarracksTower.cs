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
    public int spawnCount;
    public float unitRange;

    public List<int> unitsUpgradePrice;
    public int upgradePrice;
    public int spawnPrice;

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
    }

    private void SpawnUnit()
    {
        Unit newUnit = Instantiate(units[0 + upgradeCount], transform.position + spawnOffset, Quaternion.identity);
        newUnit.agent = newUnit.GetComponent<NavMeshAgent>();
        NavMeshAgent newUnitAgent = newUnit.agent;

        newUnitAgent.SetDestination(destination);
        spawnedUnits.Add(newUnit);
        newUnit.tower = this;

        newUnit.NewDestinationPoint(destination);
        newUnit.agent.isStopped = false;
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

        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        ballComponent.mesh.enabled = false;

        foreach (Unit unit in spawnedUnits)
        {
            unit.agent.SetDestination(destination);
            unit.NewDestinationPoint(destination);
            unit.agent.isStopped = false;
        }

        ballComponent.NewLocation();

        main.bus.destinationButton.interactable = true;
    }

    public void RecalculateUpgradePrice()
    {
        upgradePrice = unitsUpgradePrice[upgradeCount + 1];
    }

    public void RecalculateSpawnPrice() 
    {
        spawnPrice = units[upgradeCount].price;
    }

    public void Upgrade()
    {
        upgradeCount++;
        main.bus.FillUpgradeButton();
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnUnit();
        }
        main.bus.FillSpawnButton();
    }
}
