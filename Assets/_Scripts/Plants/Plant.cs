using UnityEngine;

public class Plant : Interactable
{
    [SerializeField] Transform fruitHolderParent;

    [SerializeField] PlantTypes initialPlant = PlantTypes.EnergyPlant;
    [SerializeField] protected float growDuration = 10f;

    public enum PlantTypes
    {
        WaterPlant,
        EnergyPlant,
        OxygenPlant
    }

    float _growPercentage = 0;
    public float GrowPercentage { get {  return _growPercentage; } set { _growPercentage = value; } }

    BasePlant _currentPlant;
    public BasePlant CurrentPlant { get { return _currentPlant; } set { _currentPlant = value; } }

    private PlantFactory _factory;

    private FruitHolder[] fruitHolders;

    private void Awake()
    {
        if (fruitHolderParent)
        {
            fruitHolders = fruitHolderParent.GetComponentsInChildren<FruitHolder>();
        }
    }

    private void Start()
    {
        showOutline = true;
        _factory = new PlantFactory(this);
        _currentPlant = _factory.GetConcretePlant(initialPlant);
    }

    private void Update()
    {
        GrowOverTime();
    }

    protected virtual void GrowOverTime()
    {
        _growPercentage += (1f / growDuration) * Time.deltaTime;

        _growPercentage = Mathf.Clamp01(_growPercentage);

        if (fruitHolders.Length > 0)
        {
            foreach (FruitHolder fruitHolder in fruitHolders)
            {
                fruitHolder.transform.localScale = new Vector3(_growPercentage, _growPercentage, _growPercentage);
            }
        }
    }

    public override void Interact(Transform grabPoint)
    {
        PlayerInteract.instance.interacting = !PlayerInteract.instance.interacting;

        if (_growPercentage < 1)
            return;

        ResetGrowing();
        _currentPlant.Interact();
    }

    void ResetGrowing()
    {
        _growPercentage = 0f;

        if (fruitHolders.Length < 0)
            return;

        foreach (FruitHolder fruitHolder in fruitHolders)
        {
            fruitHolder.transform.localScale = new Vector3(_growPercentage, _growPercentage, _growPercentage);
        }
    }
}
