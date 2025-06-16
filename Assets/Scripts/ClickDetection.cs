using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetection : MonoBehaviour
{
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (selectedObject != null && selectedObject.GetComponent<Tower>())
                selectedObject.GetComponent<Tower>().Deselect();
            else if (selectedObject != null && selectedObject.GetComponent<Enemy>())
                selectedObject.GetComponent<Enemy>().Deselect();
            else if (selectedObject != null && selectedObject.GetComponent<BaseUnit>())
                selectedObject.GetComponent<BaseUnit>().Deselect(true);
            else if (selectedObject != null && selectedObject.GetComponent<SuicideBomber>())
                selectedObject.GetComponent<SuicideBomber>().Deselect(true);

            if (hit.collider.gameObject.GetComponent<Tower>())
            {
                if (hit.collider.gameObject.GetComponent<Tower>().enabled)
                {
                    selectedObject = hit.collider.gameObject;
                    selectedObject.GetComponent<Tower>().Select();
                }
            }
            else if (hit.collider.gameObject.GetComponent<Enemy>())
            {
                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<Enemy>().Select();
            }
            else if (hit.collider.gameObject.GetComponent<BaseUnit>())
            {
                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<BaseUnit>().Select(true);
            }
            else if (hit.collider.gameObject.GetComponent<SuicideBomber>())
            {
                selectedObject = hit.collider.gameObject;
                selectedObject.GetComponent<SuicideBomber>().Select(true);
            }
            else
            {
                selectedObject = null;
            }
        }
    }
}