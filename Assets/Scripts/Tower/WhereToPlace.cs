using UnityEngine;

public class WhereToPlace : MonoBehaviour
{
    public bool needsToFindLocation = false;

    [SerializeField] private float distanceThreshold = 1f;

    private Vector3 cursorLocation;
    private GameObject tower;

    void Update()
    {
        if (needsToFindLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    cursorLocation = hit.point;
                    tower.transform.position = cursorLocation;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (IsFurtherThanTwoMetersFromPath())
                {
                    tower.GetComponent<Tower>().enabled = true;
                    tower.GetComponent<Renderer>().material = tower.GetComponent<Tower>().defaultMaterial;
                    ChangeMaterialOfAllDescendants(tower.transform, true);
                    tower.GetComponent<BoxCollider>().enabled = true;
                    tower.transform.Find("Shooter").GetComponent<Shooter>().enabled = true;

                    tower = null;
                    cursorLocation = new Vector3(0, 0, 0);
                    GetComponent<TowerPlacement>().PlaceTower(cursorLocation);
                    needsToFindLocation = false;
                }
            }
        }
    }

    public void StartSearch(GameObject towerToPlace)
    {
        Destroy(tower);

        tower = towerToPlace;
        tower.GetComponent<Renderer>().material = tower.GetComponent<Tower>().transparentMaterial;
        ChangeMaterialOfAllDescendants(tower.transform, false);

        cursorLocation = new Vector3(0, 0, 0);
        needsToFindLocation = true;
    }

    private bool IsFurtherThanTwoMetersFromPath()
    {
        GameObject[] pathObjects = GameObject.FindGameObjectsWithTag("Path");

        foreach (GameObject path in pathObjects)
        {
            Collider pathCollider = path.GetComponent<Collider>();
            if (pathCollider != null)
            {
                Vector3 closestPoint = pathCollider.ClosestPoint(tower.transform.position);

                float distance = Vector3.Distance(tower.transform.position, closestPoint);

                if (distance < distanceThreshold)
                {
                    return false;
                }
            }
            else
            {
                float distanceToOrigin = Vector3.Distance(tower.transform.position, path.transform.position);

                if (distanceToOrigin < distanceThreshold)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static void ChangeMaterialOfAllDescendants(Transform tf, bool toggle)
    {
        MeshRenderer mr = tf.GetComponent<MeshRenderer>();
        if (mr != null && !tf.GetComponent<Tower>())
        {
            mr.enabled = toggle;
        }

        foreach (Transform child in tf)
        {
            ChangeMaterialOfAllDescendants(child, toggle);
        }
    }
}