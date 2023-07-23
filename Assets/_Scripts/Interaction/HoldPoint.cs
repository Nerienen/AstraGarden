using UnityEngine;

[RequireComponent(typeof(Outline))]
public class HoldPoint : MonoBehaviour
{
    [SerializeField] private Transform holdTransform;
    [field: SerializeReference] public bool justForPlants { get; private set; }
    [field: SerializeReference] public bool isPot { get; private set; }
    private Outline _outline;
    
    [field: SerializeReference] public Grabbable CurrentHoldenObject { get; private set; }
    private Plant _currentPlant;

    private void Start()
    {
        _outline = GetComponent<Outline>();
        if (CurrentHoldenObject != null)
        {
            CurrentHoldenObject.GetComponent<Grabbable>().hasBeenHolded = true;
            HoldObject(CurrentHoldenObject.transform);
        }
    }

    public void HoldObject(Transform objectTransform)
    {
        CurrentHoldenObject = objectTransform.GetComponent<Grabbable>();
        CurrentHoldenObject.SetHolden(isPot);
        CurrentHoldenObject.onGrabObject += ResetHolden;

        objectTransform.position = holdTransform.position;
        objectTransform.rotation = holdTransform.rotation;

        objectTransform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        _currentPlant = objectTransform.GetComponent<Plant>();
        if (_currentPlant != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.placePlant, transform.position);
            _currentPlant.OnPlantDissolve += ResetHolden;
            if (TryGetComponent(out Machine _) && _currentPlant.GrowPercentage >= 1)
            {
                CurrentHoldenObject.transform.localScale = Vector3.one*0.4f;
            }
        }
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
        if (_currentPlant != null) _currentPlant.OnPlantDissolve -= ResetHolden;
        CurrentHoldenObject.onGrabObject -= ResetHolden;
        if (TryGetComponent(out Machine _))
        {
            CurrentHoldenObject.transform.localScale = Vector3.one;
        }
        CurrentHoldenObject = null;
        _currentPlant = null;
    }

    public bool IsHoldingObject => CurrentHoldenObject != null;
}
