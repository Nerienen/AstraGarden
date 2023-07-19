using UnityEngine;

public class EnergyPlant : BasePlant
{
    public EnergyPlant(Plant ctx, PlantFactory factory) : base(ctx, factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void CheckSwitchPlant()
    {
        // Process here when the plant should change its type to another one
    }
    protected override void Recollect()
    {
        Debug.Log($"Energy recollected");
    }
}
