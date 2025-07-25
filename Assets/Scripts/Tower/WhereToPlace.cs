using System.Linq;
using UnityEngine;

public class WhereToPlace : MonoBehaviour
{
    public Terrain terrain;
    public int grassTextureIndex = 0;
    public float raycastDistance = 10f;
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (IsOverStone())
            {
                Debug.Log("Object is over stone!");
            }
            else
            {
                Debug.Log("Object is NOT over stone.");
            }
        }

        if (needsToFindLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    towerComponent.RedrawRange();
                    cursorLocation = hit.point;
                    tower.transform.position = cursorLocation;

                    if (!IsOverStone() && !IsCloseToObjects())
                    {
                        if (towerComponent.type == TowerType.Barracks)
                            towerComponent.barracksTower.barrackModels[towerComponent.barracksTower.upgradeCount].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", baseColor);
                        else
                            tower.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", baseColor);

                    }
                    else
                    {
                        if (towerComponent.type == TowerType.Barracks)
                            towerComponent.barracksTower.barrackModels[towerComponent.barracksTower.upgradeCount].GetComponent<MeshRenderer>().material.SetColor("_BaseColor", warningColor);
                        else
                            tower.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", warningColor);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!IsOverStone() && !IsCloseToObjects())
                {
                    towerComponent.enabled = true;
                    transparentScript.NewObject(tower, 1f);

                    if (towerComponent.type != TowerType.Barracks)
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

    bool IsOverStone()
    {
        Vector3 origin = tower.transform.position;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.gameObject == terrain.gameObject)
            {
                Vector3 terrainPos = GetNormalizedPosition(hit.point, terrain);

                float[,,] splatmapData = terrain.terrainData.GetAlphamaps(
                    (int)(terrainPos.x * terrain.terrainData.alphamapWidth),
                    (int)(terrainPos.z * terrain.terrainData.alphamapHeight),
                    1, 1
                );

                float grassWeight = splatmapData[0, 0, grassTextureIndex];
                return grassWeight > 0.1f;
            }
        }

        return false;
    }

    Vector3 GetNormalizedPosition(Vector3 worldPos, Terrain terrain)
    {
        Vector3 relativePos = worldPos - terrain.transform.position;
        return new Vector3(
            relativePos.x / terrain.terrainData.size.x,
            0,
            relativePos.z / terrain.terrainData.size.z
        );
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