using UnityEngine;

public class EnergyPlant : BasePlant
{
    Plant.PlantTypes typeToSwitch;
    bool needToChangeType;

    public EnergyPlant(Plant ctx, PlantFactory factory) : base(ctx, factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public override void Enter()
    {
        _ctx.OnChangeTypeReceived += OnChangeTypeReceived; 
        _ctx.OnPlantFullyGrown += OnFullyGrown;
        foreach (PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType == Plant.PlantTypes.EnergyPlant)
            {
                plantGroup.sproutPlantHolder.gameObject.SetActive(true);
                break;
            }
        }

        if(_ctx.hasGrownInitially)_ctx.GrowPercentage = 0;
        _ctx.FruitGrowPercentage = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchPlant();
    }

    public override void Exit()
    {
        _ctx.OnChangeTypeReceived -= OnChangeTypeReceived;
        _ctx.OnPlantFullyGrown -= OnFullyGrown;

        foreach (PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType == Plant.PlantTypes.EnergyPlant)
            {
                plantGroup.sproutPlantHolder.gameObject.SetActive(false);
                plantGroup.grownPlantHolder.gameObject.SetActive(false);
                break;
            }
        }
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
        if (ResourcesController.Instance == null) return;
        
        foreach (PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType != Plant.PlantTypes.EnergyPlant) continue;
                
            ResourcesController.Instance.OnFluidCollected(AmmoType.EnergyAmmo, plantGroup.resourceCapacity);
            return;
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
            if (plantGroup.plantType == Plant.PlantTypes.EnergyPlant)
            {
                plantGroup.sproutPlantHolder.gameObject.SetActive(false);
                plantGroup.grownPlantHolder.gameObject.SetActive(true);
                break;
            }
        }
    }
}
