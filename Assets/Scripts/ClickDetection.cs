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

            if (hit.collider.gameObject.CompareTag("Tower"))
            {
                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<Tower>().Select();
            }
            else
            {
                if (selectedObject.CompareTag("Tower"))
                {
                    selectedObject.GetComponent<Tower>().Deselect();
                }
                selectedObject = null;
            }
        }
    }
}