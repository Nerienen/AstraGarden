using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenController : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] float fadeOutDuration = 0.6f;

    void Start()
    {
        if (OxygenController.Instance != null)
        {
            OxygenController.Instance.OnOxygenFinished += Show;
        }
    }

    void Show()
    {
        Transitions.DoChangeColor(image, Color.black, fadeOutDuration, Transitions.TimeScales.Fixed);
    }
}
