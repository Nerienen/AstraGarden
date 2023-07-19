using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public abstract class Interactable : MonoBehaviour
{
    public bool showOutline { get; protected set; }
    public abstract void Interact(Transform grabPoint);
}
