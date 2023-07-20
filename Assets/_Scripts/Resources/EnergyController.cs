using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public static EnergyController Instance { get; private set; }

    [SerializeField] float maxAmount = 100f;
    [SerializeField] float currentAmount = 30f;

    public float CurrentAmount { get { return currentAmount; } set { currentAmount = value; } }

    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            Debug.LogWarning($"Another instance of {nameof(EnergyController)} exists. This one has been destroyed");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion
    }

    private void Start()
    {
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
    }

    FluidGun GetFluidGun()
    {
        return PlayerInteract.instance.gameObject.GetComponentInChildren<FluidGun>();
    }

    public void IncreaseBy(float amount)
    {
        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
    }

    public void DecreaseBy(float amount)
    {
        currentAmount -= amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
    }

    public void FillFluidGunBy(float amount)
    {
        GetFluidGun().ReloadAmmo(AmmoType.EnergyAmmo, amount);
    }
}
