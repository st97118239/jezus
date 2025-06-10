using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    public List<Transform> selections;

    private Transform highlight;
    private RaycastHit raycastHit;

    void Update()
    {
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && !selections.Contains(highlight))
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                }
            }
            else
                highlight = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            if (highlight)
            {
                if (selections.Count > 0)
                {
                    foreach (var selection in selections)
                        selection.gameObject.GetComponent<Outline>().enabled = false;
                    selections.Clear();
                }
                selections.Add(raycastHit.transform);
                foreach (var selection in selections)
                    selection.gameObject.GetComponent<Outline>().enabled = true;
                highlight = null;
            }
            else
            {
                if (selections.Count > 0)
                {
                    foreach (var selection in selections)
                        selection.gameObject.GetComponent<Outline>().enabled = false;
                    selections.Clear();
                }
            }
        }
    }
}