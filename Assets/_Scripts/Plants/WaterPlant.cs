using UnityEngine;

public class WaterPlant : BasePlant
{
    Plant.PlantTypes typeToSwitch;
    bool needToChangeType;

    public WaterPlant(Plant ctx, PlantFactory factory) : base(ctx, factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public override void Enter()
    {
        _ctx.OnChangeTypeReceived += OnChangeTypeReceived;

        foreach(PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType == Plant.PlantTypes.WaterPlant)
            {
                plantGroup.plantHolder.gameObject.SetActive(true);
                break;
            }
        }
    }

    public override void UpdateState()
    {
        CheckSwitchPlant();
    }

    public override void Exit()
    {
        _ctx.OnChangeTypeReceived -= OnChangeTypeReceived;

        foreach (PlantGroup plantGroup in _ctx.PlantGroups)
        {
            if (plantGroup.plantType == Plant.PlantTypes.WaterPlant)
            {
                plantGroup.plantHolder.gameObject.SetActive(false);
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
        if (WaterController.Instance != null)
        {
            WaterController.Instance.FillFluidGunBy(_ctx.ResourceCapacity);
        }
    }

    void OnChangeTypeReceived(Plant.PlantTypes newType)
    {
        typeToSwitch = newType;
        needToChangeType = true;
    }
}
