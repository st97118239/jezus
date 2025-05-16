using UnityEngine;

public class ClickDetection : MonoBehaviour
{
    private Vector3 clickPosition;
    public GameObject selectedObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick();
        }
    }

    void DetectClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            clickPosition = hit.point;

            if (selectedObject != null && selectedObject.GetComponent<Tower>())
            {
                selectedObject.GetComponent<Tower>().Deselect();
            }

            if (hit.collider.gameObject.GetComponent<Tower>() && !hit.collider.gameObject.GetComponent<Tower>().recentlyBuilt)
            {
                if (hit.collider.gameObject.GetComponent<Tower>().enabled)
                {
                    selectedObject = hit.collider.gameObject;
                    selectedObject.GetComponent<Tower>().Select();
                }
            }
            else
            {
                selectedObject = null;
            }
        }
    }
}