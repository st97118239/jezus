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
                    ChangeMaterialOfAllDescendants(tower.transform, tower.GetComponent<Tower>().defaultMaterial);
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
        tower = towerToPlace;
        ChangeMaterialOfAllDescendants(tower.transform, tower.GetComponent<Tower>().transparentMaterial);

        cursorLocation = new Vector3(0, 0, 0);
        needsToFindLocation = true;
    }

    private bool IsFurtherThanTwoMetersFromPath()
    {
        // Find all GameObjects with the "Path" tag
        GameObject[] pathObjects = GameObject.FindGameObjectsWithTag("Path");

        // Check the distance to each object with the "Path" tag
        foreach (GameObject path in pathObjects)
        {
            // Check if the path has a collider (could be BoxCollider, SphereCollider, etc.)
            Collider pathCollider = path.GetComponent<Collider>();
            if (pathCollider != null)
            {
                // Calculate the closest point on the collider from this GameObject
                Vector3 closestPoint = pathCollider.ClosestPoint(tower.transform.position);

                // Debugging: Log the distance and positions
                Debug.DrawLine(tower.transform.position, closestPoint, Color.red, 2f); // Draw line in the scene view
                Debug.Log($"Distance from {tower.name} to path {path.name}: {Vector3.Distance(tower.transform.position, closestPoint)} meters");

                // Calculate the distance to the closest point
                float distance = Vector3.Distance(tower.transform.position, closestPoint);

                // If the distance is less than the threshold, return false (it is within range)
                if (distance < distanceThreshold)
                {
                    return false; // The GameObject is within 2 meters of a "Path" object
                }
            }
            else
            {
                // If no collider is found, fallback to checking the distance to the object's origin (same as before)
                float distanceToOrigin = Vector3.Distance(tower.transform.position, path.transform.position);

                // Debugging: Log the fallback distance
                Debug.Log($"Fallback distance from {tower.name} to path {path.name}: {distanceToOrigin} meters");

                if (distanceToOrigin < distanceThreshold)
                {
                    return false;
                }
            }
        }

        // If all checks pass and no object is within range, return true
        return true;
    }

    public static void ChangeMaterialOfAllDescendants(Transform parentTransform, Material newMaterial)
    {
        // Change the material of the current object
        Renderer renderer = parentTransform.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial; // Set the new material
        }

        // Recursively change material for all children
        foreach (Transform child in parentTransform)
        {
            ChangeMaterialOfAllDescendants(child, newMaterial); // Recurse into each child
        }
    }
}