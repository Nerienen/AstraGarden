using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Outline _outline;
    protected bool interacting;
    private void Start()
    {
        _outline = GetComponent<Outline>();
    }

    public virtual void Interact(Transform grabPoint)
    {
        interacting = !interacting;
        _outline.OutlineWidth = 0;
    }

    private void OnMouseOver()
    {
        if(interacting) return;
        
        _outline.OutlineWidth = 5;
    }
    
    private void OnMouseExit()
    {
        if(interacting) return;

        _outline.OutlineWidth = 0;
    }
}
