using UnityEngine;

public class Plant : Grabbable
{
    [Header("Fruit Parameters")]
    [SerializeField] Transform fruitHolderParent;

    [SerializeField] PlantTypes initialPlant = PlantTypes.EnergyPlant;
    
    public float FruitGrowSpeed => fruitGrowSpeed;
    [Tooltip("Fruit grow percentage increases by the FruitGrowSpeed value each second")]
    [SerializeField] protected float fruitGrowSpeed = 10f;
    [SerializeField] protected AnimationCurve fruitGrowFactor;
    
    public float GrowSpeed => growSpeed;
    [Tooltip("Grow percentage increases by the GrowSpeed value each second")]
    [SerializeField] protected float growSpeed = 10f;
    [SerializeField] protected AnimationCurve growFactor;
    

    [SerializeField] protected float maxHealthPoints = 100f;
    [SerializeField] protected float healthPoints = 100f;
    [SerializeField] protected float dryingSpeed = 2f;
    [SerializeField] protected float waterSensitivity = 10f;

    [Tooltip("By resource we understand the element this plant produce (Oxygen, Water or Energy)")]
    [SerializeField] float resourceCapacity = 10f;
    
    public float ResourceCapacity => resourceCapacity;

    public enum PlantTypes
    {
        WaterPlant,
        EnergyPlant,
        OxygenPlant
    }
    
    public enum PlantState
    {
        Sprout,
        FullyGrown,
        Dead,
    }
    
    private PlantState _currentPlantState = PlantState.Sprout;
    
    float _fruitGrowPercentage = 0;
    public float FruitGrowPercentage { get => _fruitGrowPercentage; set => _fruitGrowPercentage = value; } 
    
    float _growPercentage = 0;
    public float GrowPercentage { get => _growPercentage; set => _growPercentage = value; }

    BasePlant _currentPlant;
    public BasePlant CurrentPlant { get => _currentPlant; set => _currentPlant = value; }

    private PlantFactory _factory;

    private FruitHolder[] fruitHolders;

    protected override void Awake()
    {
        base.Awake();
        
        if (fruitHolderParent)
        {
            fruitHolders = fruitHolderParent.GetComponentsInChildren<FruitHolder>();
        }
    }

    private void Start()
    {
        _factory = new PlantFactory(this);
        _currentPlant = _factory.GetConcretePlant(initialPlant);
        
        ResetFruitGrowing();
    }

    private void Update()
    {
        DryOverTime();
        if(!holden) return;
        
        GrowOverTime();
        GrowFruitOverTime();
    }
    
    protected virtual void GrowOverTime()
    {
        if (_growPercentage >= 1) return;
        
        _growPercentage += Time.deltaTime * growSpeed * growFactor.Evaluate(healthPoints/100f) / 100f ;
        
        if (_growPercentage >= 1)
        {
            _growPercentage = 1;
            SetState(PlantState.FullyGrown);
        }
    }

    protected virtual void GrowFruitOverTime()
    {
        //Return if the plant is not fully grown
        if(_growPercentage < 1) return;
        
        _fruitGrowPercentage += Time.deltaTime * fruitGrowSpeed * fruitGrowFactor.Evaluate(healthPoints/100f) / 100f ;

        if (_fruitGrowPercentage >= 1)
        {
            grabbable = _grabbing;
            outline.OutlineColor = Color.green;
            _fruitGrowPercentage = 1;
        }
        else outline.OutlineColor = Color.white;

        if (fruitHolders.Length > 0)
        {
            foreach (FruitHolder fruitHolder in fruitHolders)
            {
                fruitHolder.transform.localScale = new Vector3(_fruitGrowPercentage, _fruitGrowPercentage, _fruitGrowPercentage);
            }
        }
    }

    protected virtual void SetState(PlantState state)
    {
        _currentPlantState = state;
        
        
    }

    protected virtual void DryOverTime()
    {
        healthPoints -= dryingSpeed * Time.deltaTime;
        healthPoints = Mathf.Max(0, healthPoints);

        if (healthPoints == 0) { _currentPlantState = PlantState.Dead; }
    }

    public override bool Interact()
    {
        if (_fruitGrowPercentage < 1 || _grabbing)
            return false;

        ResetFruitGrowing();
        _currentPlant.Interact();
        return true;
    }

    void ResetFruitGrowing()
    {
        _fruitGrowPercentage = 0f;

        grabbable = true;
        if (fruitHolders.Length < 0)
            return;

        foreach (FruitHolder fruitHolder in fruitHolders)
        {
            fruitHolder.transform.localScale = new Vector3(_fruitGrowPercentage, _fruitGrowPercentage, _fruitGrowPercentage);
        }
    }

    /// <summary>
    /// Water the plant, increasing its health points
    /// </summary>
    /// <param name="amount">Health points to increase</param>
    public void Water(float amount)
    {
        healthPoints += amount * waterSensitivity;
        healthPoints = Mathf.Min(healthPoints, maxHealthPoints);
    }
}
