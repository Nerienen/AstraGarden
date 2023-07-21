using UnityEngine;

[RequireComponent(typeof(Outline))]
public class HoldPoint : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    [field: SerializeReference] public bool justForPlants { get; private set; }
    [field: SerializeReference] public bool isPot { get; private set; }
    private Outline _outline;
    
    [field: SerializeReference] public Grabbable CurrentHoldenObject { get; private set; }

    private void Start()
    {
        _outline = GetComponent<Outline>();
        if(CurrentHoldenObject != null) HoldObject(CurrentHoldenObject.transform);
    }

    public void HoldObject(Transform objectTransform)
    {
        CurrentHoldenObject = objectTransform.GetComponent<Grabbable>();
        CurrentHoldenObject.SetHolden(isPot);
        CurrentHoldenObject.onGrabObject += ResetHolden;

        objectTransform.position = holdTransform.position;
        objectTransform.rotation = holdTransform.rotation;

        objectTransform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    public void SetOutlineWidth(float value, Interactable interactable)
    {
        Plant plant = interactable.GetComponent<Plant>();
        if(plant == null && justForPlants) return;
                
        _outline.enabled = value != 0;
        _outline.OutlineWidth = value;
    }
    
    public void SetOutlineColor(Color value)
    {
        _outline.OutlineColor = value;
    }

    public void SetOutlineWidth(float value)
    {
        _outline.enabled = value != 0;

        _outline.OutlineWidth = value;
    }

    private void ResetHolden()
    {
        CurrentHoldenObject.onGrabObject -= ResetHolden;
        CurrentHoldenObject = null;
    }

    public bool IsHoldingObject => CurrentHoldenObject != null;
}
