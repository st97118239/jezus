using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationBall : MonoBehaviour
{
    public BarracksTower tower;
    public MeshRenderer mesh;

    [SerializeField] private List<BaseUnit> unitsReached = new();
    [SerializeField] private bool hasEveryoneReachedLocation;

    private void Update()
    {
        List<BaseUnit> units = tower.spawnedUnits;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, tower.unitRange);
        List<Collider> hitCollidersList = hitColliders.ToList();

        List<BaseUnit> hitUnits = new();
        
        foreach (BaseUnit unit in units)
        {
            if (hitColliders.Any(h => h.gameObject == unit.gameObject))
            {
                hitUnits.Add(unit);
            }
        }

        if (!hasEveryoneReachedLocation && unitsReached.Count != units.Count)
        {
            foreach (BaseUnit unit in units.Where(u => !unitsReached.Contains(u)))
            {
                if (hitColliders.Any(h => h.gameObject == unit.gameObject))
                {
                    unit.IsInRange();
                    unitsReached.Add(unit);
                }
            }

            if (unitsReached.Count > 0 && unitsReached.Count == units.Count)
                hasEveryoneReachedLocation = true;
        }

        if (hitUnits.Count != units.Count)
        {
            hasEveryoneReachedLocation = false;
            foreach (BaseUnit unit in unitsReached.ToList())
            {
                if (!hitCollidersList.Contains(unit.boxCollider) && unitsReached.Contains(unit))
                {
                    hasEveryoneReachedLocation = false;
                    unit.atDestination = false;
                    unitsReached.Remove(unit);
                }
            }
        }
    }

    public void NewLocation()
    {
        hasEveryoneReachedLocation = false;
        unitsReached.Clear();
    }
}
