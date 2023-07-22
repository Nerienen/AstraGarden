using UnityEngine;
using UnityEngine.UI;

public class OxygenMeterDisplay : MonoBehaviour
{
    [SerializeField] Image progressBarFill;
    [SerializeField] Image[] colorSensitiveImages;

    [SerializeField] Color highOxygenColor = Color.cyan;
    [SerializeField] Color lowOxygenColor = Color.red;

    [SerializeField] AnimationCurve colorCurve;

    float percentage;

    private void Update()
    {
        if (OxygenController.Instance != null)
        {
            UpdateProgressBarFillAmount();
            UpdateColor();
        }
    }

    private void UpdateProgressBarFillAmount()
    {
        OxygenController oxygenController = OxygenController.Instance;

        percentage = oxygenController.CurrentAmount / oxygenController.MaxAmount;
        progressBarFill.fillAmount = percentage;
    }

    private void UpdateColor()
    {
        foreach (Image image in colorSensitiveImages)
        {
            image.color = Color.Lerp(lowOxygenColor, highOxygenColor, colorCurve.Evaluate(percentage));
        }
    }
}
