using UnityEngine;

public class WaterPlant : BasePlant
{
    public WaterPlant(Plant ctx, PlantFactory factory) : base(ctx, factory)
    {
        this._ctx = ctx;
        this._factory = factory;
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
        Debug.Log($"Water extracted");
    }
}
