using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhereToPlace : MonoBehaviour
{
    public bool needsToFindLocation = false;

    [SerializeField] private float distanceThreshold = 1f;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color warningColor;

    private TransparencyScript transparentScript;
    private GameObject tower;
    private Tower towerComponent;
    private Vector3 cursorLocation;

    private void Start()
    {
        transparentScript = GetComponent<TransparencyScript>();
    }

    void Update()
    {
        if (needsToFindLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {;
                    towerComponent.RedrawRange();
                    cursorLocation = hit.point;
                    tower.transform.position = cursorLocation;

                    if (IsCloseToObjects())
                    {

                        if (towerComponent.type == TowerTypes.Barracks)
                            towerComponent.barracksTower.barrackModels[towerComponent.barracksTower.upgradeCount].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", warningColor);
                        else
                            tower.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", warningColor);
                    }
                    else
                    {

                        if (towerComponent.type == TowerTypes.Barracks)
                            towerComponent.barracksTower.barrackModels[towerComponent.barracksTower.upgradeCount].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", baseColor);
                        else
                            tower.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", baseColor);

                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!IsCloseToObjects())
                {
                    towerComponent.enabled = true;
                    transparentScript.NewObject(tower, 1f);
                    
                    if (towerComponent.type != TowerTypes.Barracks)
                        ChangeMaterialOfAllDescendants(tower.transform, true);
                    
                    tower.GetComponent<BoxCollider>().enabled = true;
                    if (towerComponent.shooter != null)
                        towerComponent.shooter.enabled = true;
                    GetComponent<TowerPlacement>().PlaceTower(towerComponent);
                    towerComponent.WhenPlaced();

                    tower = null;
                    cursorLocation = new Vector3(0, 0, 0);
                    needsToFindLocation = false;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                towerComponent.WhenPlaced();
                Destroy(tower);
                tower = null;
                cursorLocation = new Vector3(0, 0, 0);
                needsToFindLocation = false;
            }
        }
    }

    public void StartSearch(GameObject towerToPlace)
    {
        Destroy(tower);

        tower = towerToPlace;
        towerComponent = tower.GetComponent<Tower>();
        transparentScript.NewObject(tower, 0.5f); 
        ChangeMaterialOfAllDescendants(tower.transform, false);

        cursorLocation = new Vector3(0, 0, 0);
        needsToFindLocation = true;
    }

    private bool IsCloseToObjects()
    {
        return Physics.OverlapSphere(tower.transform.position, distanceThreshold).Any(h => !h.gameObject.CompareTag("Ground"));
    }

    public static void ChangeMaterialOfAllDescendants(Transform tf, bool toggle)
    {
        MeshRenderer mr = tf.GetComponent<MeshRenderer>();
        if (mr != null && !tf.GetComponent<Tower>() && !tf.CompareTag("NoMRToggle"))
        {
            mr.enabled = toggle;
        }

        foreach (Transform child in tf)
        {
            ChangeMaterialOfAllDescendants(child, toggle);
        }
    }
}