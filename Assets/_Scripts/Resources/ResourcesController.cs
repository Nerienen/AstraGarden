using System;
using UnityEngine;

public class ResourcesController : MonoBehaviour
{
    public Action<AmmoType, float> OnFluidCollected;
    public static ResourcesController Instance { get; private set; }
    
    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            Debug.LogWarning($"Another instance of {nameof(ResourcesController)} exists. This one has been destroyed");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion
    }
    public void FillFluidGunBy(AmmoType ammoType, float amount)
    {
        OnFluidCollected?.Invoke(ammoType, amount);
    }
}