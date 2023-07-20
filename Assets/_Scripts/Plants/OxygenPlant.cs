using UnityEngine;

public class OxygenPlant : BasePlant
{
    public OxygenPlant(Plant ctx, PlantFactory factory) : base(ctx, factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public override void Enter()
    {
        if (OxygenController.Instance != null)
        {
            OxygenController.Instance.IncreaseOxygenRateBy(_ctx.ResourceCapacity);
        }
    }

    public override void Exit()
    {
        if (OxygenController.Instance != null)
        {
            OxygenController.Instance.DecreaseOxygenRateBy(_ctx.ResourceCapacity);
        }
    }

    public override void CheckSwitchPlant()
    {
        // Process here when the plant should change its type to another one
    }

    protected override void Recollect()
    {
        // Not intented to do anything here because Oxygen can't be recollected
    }
}
