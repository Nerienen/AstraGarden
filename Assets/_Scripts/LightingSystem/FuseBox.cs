using System;
using FMODUnity;
using UnityEngine;

public class FuseBox : MonoBehaviour
{
    public static FuseBox Instance { get; private set; }

    public event Action OnPowerDown;
    public event Action OnPowerUp;

    [SerializeField] float oxygenLimit = 90f;

    [Header("Debug properties - Power on")]
    [SerializeField] bool powerOn;

    bool canBePoweredOn;
    bool hasGoneDown;
    
    private void OnValidate()
    {
        if (powerOn)
        {
            powerOn = false;
            PowerOn();
        }
    }

    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            Debug.LogWarning($"Another instance of {nameof(FuseBox)} exists. This one has been eliminated");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion
    }

    private void Update()
    {
        if (FuseBox.Instance != null && !hasGoneDown && OxygenController.Instance != null)
        {
            OxygenController oxygenController = OxygenController.Instance;
            
            if (oxygenController.CurrentAmount >= oxygenLimit)
            {
                OnPowerDown?.Invoke();
                RuntimeManager.StudioSystem.setParameterByName("isBlackOut", 1);
                canBePoweredOn = true;
                hasGoneDown = true;
            }
        }
    }

    public void PowerOn()
    {
        OnPowerUp?.Invoke();
        RuntimeManager.StudioSystem.setParameterByName("isBlackOut", 0);
    }
}
