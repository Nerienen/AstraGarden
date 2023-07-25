using System;
using System.IO;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using System.Diagnostics;

public class OxygenController : MonoBehaviour
{
    public static OxygenController Instance { get; private set; }

    public event Action OnOxygenFinished;

    [SerializeField] float maxAmount = 100f;
    [SerializeField] float currentAmount = 70f;

    [SerializeField] float oxygenRate = 0f;

    public float MaxAmount {  get { return maxAmount; } }
    public float CurrentAmount {  get { return currentAmount; } set {  currentAmount = value; } }

    public EventInstance emitter;
    private int count = 1;

    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            UnityEngine.Debug.LogWarning($"Another instance of {nameof(OxygenController)} exists. This one has been eliminated");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        #endregion
    }

    private void Start()
    {
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        emitter = RuntimeManager.CreateInstance(FMODEvents.instance.breath);
        emitter.start();

    }

    private void Update()
    {
        if(!CutsceneController.Instance.isPlayingCutscene) UpdateOxygenOverTime();
    }

    void UpdateOxygenOverTime()
    {
        currentAmount += oxygenRate * Time.deltaTime;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        CheckOxygenFinished();

        count += 1;

        if (count > 25)
        {
//            UnityEngine.Debug.Log("Current Oxygen Calc: " + currentAmount / maxAmount);
            emitter.setParameterByName("Oxygen", currentAmount / maxAmount); 
            //           emitter.getParameterByName("Oxygen", out float check);
//            UnityEngine.Debug.Log("Current Oxygen Parameter: " + check);
            if (MusicManager.Instance != null)
            {
                MusicManager.Instance.SetMusicParameter("Oxygen",currentAmount/maxAmount);
            }
            count = 0;
        }
        
    }

    void CheckOxygenFinished()
    {
        if (currentAmount <= 0)
        {
            OnOxygenFinished?.Invoke();
        }
    }

    public void IncreaseBy(float amount)
    {
        currentAmount += amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        CheckOxygenFinished();
    }

    public void DecreaseBy(float amount)
    {
        currentAmount -= amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        CheckOxygenFinished();
    }

    public void IncreaseOxygenRateBy(float amount)
    {
        oxygenRate += amount;
    }

    public void DecreaseOxygenRateBy(float amount)
    {
        oxygenRate -= amount;
    }
}
