using System;
using UnityEngine;

public class Plant : Grabbable
{
    public event Action OnPlantHasBeenHolden;
    public event Action OnPlantHasStoppedBeenHolden;
    public event Action<PlantTypes> OnChangeTypeReceived;
    public event Action OnPlantFullyGrown;

    public event Action OnPlantDissolve;

    [Header("Plant parameters")]
    [SerializeField] PlantTypes initialPlant = PlantTypes.OxygenPlant;
    [SerializeField] PlantGroup[] plantGroups;

    [Header("Fruit Parameters")]

    [SerializeField] protected AnimationCurve fruitGrowFactor;
    public float FruitGrowSpeed => fruitGrowSpeed;
    [SerializeField] protected float fruitGrowSpeed = 10f;
    [Tooltip("Fruit grow percentage increases by the FruitGrowSpeed value each second")]

    public float GrowSpeed => growSpeed;
    [Tooltip("Grow percentage increases by the GrowSpeed value each second")]
    [SerializeField] protected float growSpeed = 10f;
    [SerializeField] protected AnimationCurve growFactor;


    [SerializeField] protected float maxHealthPoints = 100f;
    [SerializeField] protected float healthPoints = 100f;
    [SerializeField] protected float dryingSpeed = 2f;
    [SerializeField] protected float waterSensitivity = 10f;

    [Header("Debug - Change plant type")]
    [SerializeField] bool changeType;
    [SerializeField] PlantTypes type;

    [Header("VFX-related parameters")]
    [SerializeField] float dissolveDuration = 1f;

    public PlantGroup[] PlantGroups { get { return plantGroups; } }
    private PlantTypes _currentType;
    public PlantTypes CurrentType { get => _currentType; }
    public bool Holden { get => holden; }

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

    private PlantInspector _plantInspector;

    float _fruitGrowPercentage = 0;
    public float FruitGrowPercentage { get => _fruitGrowPercentage; set => _fruitGrowPercentage = value; }

    float _growPercentage = 0;
    public float GrowPercentage { get => _growPercentage; set => _growPercentage = value; }

    BasePlant _currentPlant;
    public BasePlant CurrentPlant { get => _currentPlant; set => _currentPlant = value; }

    private PlantFactory _factory;

    private FruitHolder[] _fruitHolders;

    private bool _previousHolden;

    private MeshRenderer[] _renderers;
    private float _dissolvePercentage;

    private bool _isIlluminated = true;

    public PlantData PlantData
    {
        get
        {
            if(_plantData == null) _plantData = new PlantData(GetCurrentPlantName(), healthPoints, _growPercentage, _fruitGrowPercentage, _currentType);
            else _plantData.UpdatePlantData(GetCurrentPlantName(), healthPoints, _growPercentage, _fruitGrowPercentage, _currentType);
            return _plantData;
        }
    }

    private PlantData _plantData;
    
    private void OnValidate()
    {
        if (changeType)
        {
            ChangeType(type);
            changeType = false;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _plantInspector = GetComponent<PlantInspector>();

        _fruitHolders = GetComponentsInChildren<FruitHolder>();
        _renderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void Start()
    {
        _factory = new PlantFactory(this);

        HideAllPlantGroups();

        _currentPlant = _factory.GetConcretePlant(initialPlant);
        _currentPlant.Enter();
        _currentType = initialPlant;

        _plantInspector.IsInspectable = true;
        ResetFruitGrowing();

        if (FuseBox.Instance != null)
        {
            FuseBox.Instance.OnPowerDown += OnLightsOff;
            FuseBox.Instance.OnPowerUp += OnLightsOn;
        }

        onGrabObject += () =>
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.pickUpPlant, transform.position);
        };
    }

    private void OnDestroy()
    {
        if (FuseBox.Instance != null)
        {
            FuseBox.Instance.OnPowerDown -= OnLightsOff;
            FuseBox.Instance.OnPowerUp -= OnLightsOn;
        }
    }

    private void Update()
    {
        if(grabbable) DryOverTime();
        if (_currentPlantState == PlantState.Dead)
        {
            outline.OutlineWidth = 0;
            Dissolve();
            return;
        }

        _currentPlant.UpdateState();
        CheckHoldenStatusChanges();

        if(!holden) return;

        GrowOverTime();
        GrowFruitOverTime();
    }

    private void HideAllPlantGroups()
    {
        foreach (PlantGroup plantGroup in plantGroups)
        {
            plantGroup.grownPlantHolder.gameObject.SetActive(false);
            plantGroup.sproutPlantHolder.gameObject.SetActive(false);
        }
    }

    private void CheckHoldenStatusChanges()
    {
        if (_previousHolden != holden)
        {
            if (holden) OnPlantHasBeenHolden?.Invoke();
            else OnPlantHasStoppedBeenHolden?.Invoke();
        }

        _previousHolden = holden;
    }

    protected virtual void GrowOverTime()
    {
        if (!_isIlluminated) return;
        if (_growPercentage >= 1) return;

        _growPercentage += Time.deltaTime * growSpeed * growFactor.Evaluate(healthPoints / 100f) / 100f;

        if (_growPercentage >= 1)
        {
            _growPercentage = 1;
            SetState(PlantState.FullyGrown);
            OnPlantFullyGrown?.Invoke();
        }
    }

    protected virtual void GrowFruitOverTime()
    {
        //Return if the plant is not fully grown
        if (_growPercentage < 1 || _currentType == PlantTypes.OxygenPlant) return;

        _fruitGrowPercentage += Time.deltaTime * fruitGrowSpeed * fruitGrowFactor.Evaluate(healthPoints / 100f) / 100f;

        if (_fruitGrowPercentage >= 1)
        {
            grabbable = _grabbing;
            outline.OutlineColor = Color.green;

            _fruitGrowPercentage = 1;
        }
        else outline.OutlineColor = Color.white;

        if (_fruitHolders.Length > 0)
        {
            foreach (FruitHolder fruitHolder in _fruitHolders)
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

        foreach (MeshRenderer renderer in _renderers)
        {
            renderer.material.SetFloat("_RotAmount", 1 - (healthPoints / maxHealthPoints));
        }

        if (healthPoints <= 0) 
        { 
            _currentPlantState = PlantState.Dead; 
        }
    }

    public override bool Interact()
    {
        if (_fruitGrowPercentage < 1 || _grabbing || _currentType == PlantTypes.OxygenPlant || _currentPlantState == PlantState.Dead)
            return false;

        ResetFruitGrowing();
        _currentPlant.Interact();
        return true;
    }

    void ResetFruitGrowing()
    {
        _fruitGrowPercentage = 0f;

        grabbable = true;
        if (_fruitHolders.Length < 0)
            return;

        foreach (FruitHolder fruitHolder in _fruitHolders)
        {
            fruitHolder.transform.localScale = new Vector3(_fruitGrowPercentage, _fruitGrowPercentage, _fruitGrowPercentage);
        }
    }

    void Dissolve()
    {
        _dissolvePercentage += (1f / dissolveDuration) * Time.deltaTime;
        _dissolvePercentage = Mathf.Clamp(_dissolvePercentage, 0, 1);

        float dissolveValue = Mathf.Lerp(0, 1, _dissolvePercentage);

        foreach (MeshRenderer renderer in _renderers)
        {
            renderer.material.SetFloat("_DissolveStrength", dissolveValue);
        }

        if (_dissolvePercentage == 1)
        {
            OnPlantDissolve?.Invoke();
            gameObject.SetActive(false);
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

    public void ChangeType(PlantTypes newType)
    {
        OnChangeTypeReceived?.Invoke(newType);
        _currentType = newType;
    }

    public string GetCurrentPlantName()
    {
        foreach (PlantGroup plantGroup in PlantGroups)
        {
            if (plantGroup.plantType == _currentType)
            {
                return plantGroup.plantName;
            }
        }

        return "Unknown plant";
    }

    void OnLightsOn()
    {
        _isIlluminated = true;
    }

    void OnLightsOff()
    {
        _isIlluminated = false;
    }
}
