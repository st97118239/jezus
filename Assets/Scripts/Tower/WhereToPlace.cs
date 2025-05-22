using UnityEngine;

public class WhereToPlace : MonoBehaviour
{
    public bool needsToFindLocation = false;

    [SerializeField] private float distanceThreshold = 1f;

    private TransparencyScript transparentScript;
    private GameObject tower;
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
                    transparentScript.NewObject(tower, 1f);
                    ChangeMaterialOfAllDescendants(tower.transform, true);
                    tower.GetComponent<BoxCollider>().enabled = true;
                    tower.transform.Find("Shooter").GetComponent<Shooter>().enabled = true;

                    tower = null;
                    cursorLocation = new Vector3(0, 0, 0);
                    GetComponent<TowerPlacement>().PlaceTower();
                    needsToFindLocation = false;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
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
        transparentScript.NewObject(tower, 0.5f);
        ChangeMaterialOfAllDescendants(tower.transform, false);

        cursorLocation = new Vector3(0, 0, 0);
        needsToFindLocation = true;
    }

    private bool IsFurtherThanTwoMetersFromPath()
    {
        GameObject[] pathObjects = GameObject.FindGameObjectsWithTag("Path");

        foreach (GameObject path in pathObjects)
        {
            if (path.TryGetComponent<Collider>(out var pathCollider))
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