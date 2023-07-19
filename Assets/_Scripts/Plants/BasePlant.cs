using UnityEngine;

public abstract class BasePlant
{
    protected Plant _ctx;
    protected PlantFactory _factory;

    public BasePlant(Plant ctx, PlantFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public abstract void Enter();

    protected virtual void GrowOverTime()
    {
        _ctx.GrowPercentage += (1f / _ctx.GrowPercentage) * Time.deltaTime;

        _ctx.GrowPercentage = Mathf.Clamp01(_ctx.GrowPercentage);

        if (_ctx.GrowPercentage < 1) Debug.Log($"GrowPercentage: {_ctx.GrowPercentage}");
    }

    public abstract void Exit();

    public abstract void CheckSwitchPlant();

    protected void SwitchState(BasePlant newPlant)
    {
        Exit();
        newPlant.Enter();

        _ctx.CurrentPlant = newPlant;
    }

    protected abstract void Recollect();

    public void Interact()
    {
        Recollect();
    }
}
