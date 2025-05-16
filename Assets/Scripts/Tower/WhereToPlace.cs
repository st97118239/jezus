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
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    cursorLocation = hit.point;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                GetComponent<TowerPlacement>().PlaceTower(cursorLocation);
                needsToFindLocation = false;
            }
        }
    }

    public void StartSearch()
    {
        cursorLocation = new Vector3(0, 0, 0);
        needsToFindLocation = true;
    }
}