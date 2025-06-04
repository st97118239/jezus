using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationBall : MonoBehaviour
{
    public BarracksTower tower;
    public MeshRenderer mesh;
    public float range = 1.5f;

    [SerializeField] private List<Unit> unitsReached = new List<Unit>();
    [SerializeField] private bool hasEveryoneReachedLocation;

    private void Update()
    {
        List<Unit> units = tower.spawnedUnits;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);
        List<Collider> hitCollidersList = hitColliders.ToList();

        if (!hasEveryoneReachedLocation && unitsReached.Count != units.Count)
        {
            foreach (Unit unit in units.Where(u => !unitsReached.Contains(u)))
            {
                if (hitColliders.Any(h => h.gameObject == unit.gameObject))
                {
                    Debug.Log(unit);
                    unit.IsInRange();
                    unitsReached.Add(unit);
                }
            }

            if (unitsReached.Count > 0)
                Debug.Log(unitsReached.Count);

            if (unitsReached.Count == units.Count)
                hasEveryoneReachedLocation = true;
        }
        else if (hasEveryoneReachedLocation && unitsReached.Count != units.Count)
        {
            hasEveryoneReachedLocation = false;
            foreach (Unit unit in unitsReached)
            {
                if (!ListContainsColliders(hitCollidersList, unit.boxCollider) && ListContains(unitsReached, unit))
                {
                    hasEveryoneReachedLocation = false;
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
