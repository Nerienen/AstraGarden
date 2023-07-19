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
        Debug.Log($"Can't recollect from an oxygen plant");
    }
}
