using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    public List<Transform> selections;
    public Transform selected;

    [SerializeField] private Transform highlight;
    [SerializeField] private Transform hitObject;
    [SerializeField] private List<Transform> toAdd = new();
    [SerializeField] private RaycastHit raycastHit;

    void Update()
    {
        hitObject = null;
        if (highlight != null)
        {
            if (!selections.Contains(highlight))
                highlight.gameObject.GetComponent<Outline>().enabled = false;

            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            hitObject = highlight;
            if (highlight.CompareTag("Selectable") && highlight != selected)
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
                    selected = null;
                }

                selected = highlight;
                selections.Add(raycastHit.transform);

                foreach (Transform obj in toAdd)
                    selections.Add(obj);

                foreach (var selection in selections)
                {
                    if (selection.gameObject.GetComponent<Outline>() == null)
                    {
                        Outline outline = selection.gameObject.AddComponent<Outline>();
                        outline.enabled = true;
                    }
                    selection.gameObject.GetComponent<Outline>().enabled = true;
                }

                highlight = null;
                toAdd.Clear();
            }
            else
            {
                if (selections.Count > 0)
                {
                    foreach (Transform selection in selections)
                    {
                        if (!selections.Contains(hitObject))
                            selection.gameObject.GetComponent<Outline>().enabled = false;
                    }
                    if (!selections.Contains(hitObject))
                    {
                        selected = null;
                        selections.Clear();
                    }
                    else
                    {
                        selected = hitObject;
                        selections.Clear();
                        selections.Add(selected);
                    }
                }
            }
        }
    }

    public void AddNewSelection(Transform objectToAdd)
    {
        toAdd.Add(objectToAdd);
    }
}