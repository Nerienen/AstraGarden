using UnityEngine;

public class OxygenController : MonoBehaviour
{
    public static OxygenController Instance { get; private set; }

    [SerializeField] float maxAmount = 100f;
    [SerializeField] float currentAmount = 70f;

    [SerializeField] float oxygenRate = 0f;

    public float MaxAmount {  get { return maxAmount; } }
    public float CurrentAmount {  get { return currentAmount; } set {  currentAmount = value; } }

    private void Awake()
    {
        #region Singleton declaration
        if (Instance != null)
        {
            Debug.LogWarning($"Another instance of {nameof(OxygenController)} exists. This one has been eliminated");
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

    private void Update()
    {
        UpdateOxygenOverTime();
    }

    void UpdateOxygenOverTime()
    {
        currentAmount += oxygenRate * Time.deltaTime;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        
        MusicManager.Instance.SetMusicParameter("Oxygen",currentAmount/maxAmount);
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

    public void IncreaseOxygenRateBy(float amount)
    {
        oxygenRate += amount;
    }

    public void DecreaseOxygenRateBy(float amount)
    {
        oxygenRate -= amount;
    }
}
