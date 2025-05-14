using UnityEngine;

public class WhereToPlace : MonoBehaviour
{
    private Vector3 cursorLocation;
    public bool needsToFindLocation = false;

    void Update()
    {
        if (needsToFindLocation)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                cursorLocation = hit.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<TowerPlacement>().PlaceTower(cursorLocation);
                needsToFindLocation = false;
            }
        }
    }
}