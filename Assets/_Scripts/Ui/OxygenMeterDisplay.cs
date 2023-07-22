using UnityEngine;
using UnityEngine.UI;

public class OxygenMeterDisplay : MonoBehaviour
{
    [SerializeField] Image progressBarFill;
    [SerializeField] Image[] colorSensitiveImages;

    [SerializeField, GradientUsage(false)] private Gradient gradient;

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
            image.color = gradient.Evaluate(percentage);
        }
    }
}
