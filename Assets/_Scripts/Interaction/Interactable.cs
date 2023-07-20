using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class Interactable : MonoBehaviour
{
    public Color outlineColor { get; protected set; }
    public abstract bool Interact();
    public abstract bool Interact(Transform grabPoint);
}
