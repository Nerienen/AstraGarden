using UnityEngine;

public class OxygenPlant : BasePlant
{
    Plant.PlantTypes typeToSwitch;
    bool needToChangeType;

    public OxygenPlant(Plant ctx, PlantFactory factory) : base(ctx, factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public override void Enter()
    {
        _ctx.OnPlantHasBeenHolden += OnStartProducingOxygen;
        _ctx.OnPlantHasStoppedBeenHolden += OnStopProducingOxygen;
        _ctx.OnChangeTypeReceived += OnChangeTypeReceived;
        _ctx.OnPlantFullyGrown += OnFullyGrown;

        foreach (PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType == Plant.PlantTypes.OxygenPlant)
            {
                plantGroup.sproutPlantHolder.gameObject.SetActive(true);
                break;
            }
        }

        _ctx.GrowPercentage = 0;
        _ctx.FruitGrowPercentage = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchPlant();
    }

    public override void Exit()
    {
        _ctx.OnPlantHasBeenHolden -= OnStartProducingOxygen;
        _ctx.OnPlantHasStoppedBeenHolden -= OnStopProducingOxygen;
        _ctx.OnChangeTypeReceived -= OnChangeTypeReceived;
        _ctx.OnPlantFullyGrown -= OnFullyGrown;

        foreach (PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType == Plant.PlantTypes.OxygenPlant)
            {
                plantGroup.sproutPlantHolder.gameObject.SetActive(false);
                plantGroup.grownPlantHolder.gameObject.SetActive(false);
                break;
            }
        }

        needToChangeType = false;
    }

    public override void CheckSwitchPlant()
    {
        if (needToChangeType)
        {
            needToChangeType = false;
            SwitchState(_factory.GetConcretePlant(typeToSwitch));
        }
    }

    protected override void Recollect()
    {
        // Not intented to do anything here because Oxygen can't be recollected
    }
    
    void OnStartProducingOxygen()
    {
        if (OxygenController.Instance != null && _ctx.GrowPercentage >= 1)
        {
            OxygenController.Instance.IncreaseOxygenRateBy(_ctx.ResourceCapacity);
        }
    }

    void OnStopProducingOxygen()
    {
        if (OxygenController.Instance != null && _ctx.GrowPercentage >= 1)
        {
            OxygenController.Instance.DecreaseOxygenRateBy(_ctx.ResourceCapacity);
        }
    }

    void OnChangeTypeReceived(Plant.PlantTypes newType)
    {
        typeToSwitch = newType;
        needToChangeType = true;
    }

    void OnFullyGrown()
    {
        foreach (PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType == Plant.PlantTypes.OxygenPlant)
            {
                plantGroup.sproutPlantHolder.gameObject.SetActive(false);
                plantGroup.grownPlantHolder.gameObject.SetActive(true);
                break;
            }
        }

        if(_ctx.Holden)
        {
            OnStartProducingOxygen();
        }
    }
}
