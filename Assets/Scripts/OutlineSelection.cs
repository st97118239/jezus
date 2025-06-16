using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    public List<Transform> toAdd = new();
    public List<Transform> selections;
    public Transform selected;

    [SerializeField] private Transform highlight;
    [SerializeField] private Transform hitObject;
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
                    {
                        if (selection != null)
                            selection.gameObject.GetComponent<Outline>().enabled = false;
                        else
                            selections.Remove(selection);
                    }

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
                        selection.gameObject.AddComponent<Outline>();
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
        if (objectToAdd.gameObject.GetComponent<Outline>() == null)
        {
            Outline outline = objectToAdd.gameObject.AddComponent<Outline>();
            outline.enabled = true;
        }
        selections.Add(objectToAdd);
    }

    public void ChangeLayerOfAllDescendants(Transform tf, int layer)
    {
        if (!tf.CompareTag("NoLayerToggle") && !tf.CompareTag("SuicideBomberMaxHeight"))
            tf.gameObject.layer = layer;

        foreach (Transform child in tf)
        {
            ChangeLayerOfAllDescendants(child, layer);
        }
    }
}