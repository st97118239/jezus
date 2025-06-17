using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BarracksTower : MonoBehaviour
{
    public List<BaseUnit> units;
    public List<BaseUnit> spawnedUnits;
    public List<GameObject> barrackModels;
    public OutlineSelection os;
    public GameObject destinationBall;
    public DestinationBall ballComponent;
    public GameObject destinationRangeObject;
    public Vector3 destination;
    public Vector3 spawnOffset;
    public int upgradeCount;
    public int maxUnits;
    public int spawnCount;
    public float unitRange;
    public bool isDisabled;

    public List<int> unitsUpgradePrice;
    public int upgradePrice;
    public int spawnPrice;

    private OutlineSelection outlineSelection;
    private Main main;
    private Tower tower;
    private Vector3 cursorLocation;
    private bool needsToFindLocation;

    private void Start()
    {
        outlineSelection = FindObjectOfType<OutlineSelection>();
        main = FindObjectOfType<Main>();
        os = FindObjectOfType<OutlineSelection>();
        GameObject newBall = Instantiate(destinationBall, transform.position, Quaternion.identity);
        tower = GetComponent<Tower>();
        ballComponent = newBall.GetComponent<DestinationBall>();
        ballComponent.tower = this;
        ballComponent.mesh = ballComponent.GetComponent<MeshRenderer>();
        ballComponent.mesh.enabled = false;
        destinationBall = newBall;
        HideOtherModels();
        maxUnits = units[upgradeCount].maxUnits;
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

    public void HideOtherModels()
    {
        foreach (GameObject model in barrackModels.Skip(1))
        {
            ChangeMaterialOfAllDescendants(model.transform, false);
        }
    }

    private void SpawnUnit()
    {
        BaseUnit newUnit = Instantiate(units[0 + upgradeCount], transform.position + spawnOffset, Quaternion.identity);
        NavMeshAgent newUnitAgent = newUnit.agent;

        newUnitAgent.SetDestination(destination);
        spawnedUnits.Add(newUnit);
        newUnit.tower = this;

        newUnit.NewDestinationPoint(destination);
        newUnit.agent.isStopped = false;

        newUnit.Select(false);
    }

    public void FindNewDestination()
    {
        needsToFindLocation = true;
        ballComponent.mesh.enabled = true;
    }

    private void CancelNewDestination()
    {
        needsToFindLocation = false;
        destinationBall.transform.position = destination;
        ballComponent.mesh.enabled = false;
        main.bus.destinationButton.interactable = true;
    }

    private void FoundNewDestination()
    {
        destination = cursorLocation;

        needsToFindLocation = false;
        cursorLocation = Vector3.zero;
        ballComponent.mesh.enabled = false;

        foreach (BaseUnit unit in spawnedUnits)
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
        if (upgradeCount < (unitsUpgradePrice.Count - 1))
            upgradePrice = unitsUpgradePrice[upgradeCount + 1];
    }

    public void RecalculateSpawnPrice()
    {
        spawnPrice = units[upgradeCount].price;
    }

    public void Upgrade()
    {
        upgradeCount++;
        maxUnits = units[upgradeCount].maxUnits;
        main.bus.FillUpgradeButton();
        RemoveAllUnits();
        main.bus.FillSpawnButton();
        if (upgradeCount >= (units.Count - 1))
            main.bus.DisableUpgradeButton();

        ChangeMaterialOfAllDescendants(barrackModels[upgradeCount].transform, true);
        ChangeMaterialOfAllDescendants(barrackModels[upgradeCount - 1].transform, false);

        if (units[upgradeCount].type == UnitType.DaVinciTank)
            unitRange = units[upgradeCount].destinationRange;
    }

    public void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnUnit();
        }
        main.bus.FillSpawnButton();

        if (spawnedUnits.Count >= maxUnits)
            main.bus.DisableSpawnButton();
    }

    private void RemoveAllUnits()
    {
        foreach (BaseUnit unit in spawnedUnits.ToList())
        {
            unit.Remove();
        }

        spawnedUnits.Clear();
    }

    public static void ChangeMaterialOfAllDescendants(Transform tf, bool toggle)
    {
        MeshRenderer mr = tf.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.enabled = toggle;
        }

        foreach (Transform child in tf)
        {
            ChangeMaterialOfAllDescendants(child, toggle);
        }
    }

    public void Selected(bool runUnitsFunction)
    {
        BaseUnit unit = units[upgradeCount];
        float barracksRange = unitRange + unit.extraDistanceToFindEnemiesIn;
        destinationRangeObject.transform.position = destinationBall.transform.position;
        destinationRangeObject.transform.localScale = new Vector3(barracksRange * 2, 0.1f, barracksRange * 2);
        destinationRangeObject.GetComponent<MeshRenderer>().enabled = true;
        destinationRangeObject.layer = 9;
        outlineSelection.ChangeLayerOfAllDescendants(transform, 9);

        if (runUnitsFunction)
        {
            foreach (var u in spawnedUnits)
            {
                u.Select(false);
            }
        }
    }

    public void Deselected(bool runUnitsFunction)
    {
        main.bus.TowerDeselected();
        destinationRangeObject.transform.localScale = new Vector3(0f, 0f, 0f);
        destinationRangeObject.GetComponent<MeshRenderer>().enabled = false;
        destinationRangeObject.layer = 0;
        outlineSelection.ChangeLayerOfAllDescendants(transform, 10);

        if (runUnitsFunction)
        {
            foreach (var u in spawnedUnits)
                u.Deselect(false);
        }
    }

    public void DisableTower()
    {
        isDisabled = true;
        needsToFindLocation = false;
        CancelNewDestination();

        foreach (var unit in spawnedUnits)
            unit.DisableUnit();
    }
}
