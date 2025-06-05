using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationBall : MonoBehaviour
{
    public BarracksTower tower;
    public MeshRenderer mesh;

    [SerializeField] private List<Unit> unitsReached = new List<Unit>();
    [SerializeField] private bool hasEveryoneReachedLocation;

    private void Update()
    {
        List<Unit> units = tower.spawnedUnits;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, tower.unitRange);
        List<Collider> hitCollidersList = hitColliders.ToList();

        List<Unit> hitUnits = new List<Unit>();

        foreach (Unit unit in units)
        {
            if (hitColliders.Any(h => h.gameObject == unit.gameObject))
            {
                hitUnits.Add(unit);
            }
        }

        if (!hasEveryoneReachedLocation && unitsReached.Count != units.Count)
        {
            foreach (Unit unit in units.Where(u => !unitsReached.Contains(u)))
            {
                if (hitColliders.Any(h => h.gameObject == unit.gameObject))
                {
                    unit.IsInRange();
                    unitsReached.Add(unit);
                }
            }

            if (unitsReached.Count > 0)

            if (unitsReached.Count == units.Count)
                hasEveryoneReachedLocation = true;
        }

        if (hitUnits.Count != units.Count)
        {
            hasEveryoneReachedLocation = false;
            foreach (Unit unit in unitsReached.ToList())
            {
                if (!ListContainsColliders(hitCollidersList, unit.boxCollider) && ListContains(unitsReached, unit))
                {
                    hasEveryoneReachedLocation = false;
                    unit.atDestination = false;
                    unitsReached.Remove(unit);
                }
            }
        }
    }

    private bool ListContains(List<Unit> list, Unit obj)
    {
        if (list.Contains(obj))
            return true;
        else
            return false;
    }

    private bool ListContainsColliders(List<Collider> list, Collider obj)
    {
        if (list.Contains(obj))
            return true;
        else
            return false;
    }

    public void NewLocation()
    {
        hasEveryoneReachedLocation = false;
        unitsReached.Clear();
    }
}
