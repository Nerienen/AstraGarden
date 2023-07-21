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

    public abstract void UpdateState();

    protected virtual void GrowOverTime()
    {
        _ctx.FruitGrowPercentage += (1f / _ctx.FruitGrowPercentage) * Time.deltaTime;

        _ctx.FruitGrowPercentage = Mathf.Clamp01(_ctx.FruitGrowPercentage);

        if (_ctx.FruitGrowPercentage < 1) Debug.Log($"GrowPercentage: {_ctx.FruitGrowPercentage}");
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
