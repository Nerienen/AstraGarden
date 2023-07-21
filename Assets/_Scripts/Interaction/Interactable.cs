using System;
using UnityEngine;
[RequireComponent(typeof(Outline))]
public abstract class Interactable : MonoBehaviour
{
    protected Outline outline;
    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public abstract bool Interact();
    public abstract bool Interact(Transform grabPoint);

    public void SetOutlineWidth(float value)
    {
        outline.OutlineWidth = value;
    }
}
