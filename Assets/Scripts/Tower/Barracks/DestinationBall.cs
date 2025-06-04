using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationBall : MonoBehaviour
{
    public BarracksTower tower;
    public MeshRenderer mesh;
    public float range = 1.5f;

    private bool hasEveryoneReachedLocation;

    private void Update()
    {
        List<Unit> units = tower.spawnedUnits;

        if (!hasEveryoneReachedLocation && units.Count == tower.maxUnits)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, range);

            List<Unit> unitsReached = new List<Unit>();

            foreach (Unit unit in units)
            {
                if (hitColliders.Any(h => h.gameObject == unit.gameObject))
                {
                    Debug.Log(unit + " is in range");
                    unit.IsInRange();
                    unitsReached.Add(unit);
                }
            }

            if (unitsReached.Count > 0)
                Debug.Log(unitsReached.Count);

            if (unitsReached.Count == tower.maxUnits)
                hasEveryoneReachedLocation = true;
        }
    }

    public void NewLocation()
    {
        hasEveryoneReachedLocation = false;
    }
}
