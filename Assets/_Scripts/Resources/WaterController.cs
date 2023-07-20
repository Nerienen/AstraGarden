using UnityEngine;
using UnityEngine.Events;

public class WaterController : MonoBehaviour
{
    public UnityEvent<AmmoType, float> OnWaterIncreased;
    public static WaterController Instance { get; private set; }

    [SerializeField] float maxAmount = 100f;
    [SerializeField] float currentAmount = 50f;

    public float CurrentAmount {  get { return currentAmount; } set {  currentAmount = value; } }

    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            Debug.LogWarning($"Another instance of {nameof(WaterController)} exists. This one has been destroyed");
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

    public void FillFluidGunBy(float amount)
    {
        GetFluidGun().ReloadAmmo(AmmoType.WaterAmmo, amount);
    }
}
