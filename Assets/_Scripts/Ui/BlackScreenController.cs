using System.Collections;
using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        Transitions.DoChangeColorAsync(image, Color.black, fadeOutDuration, Transitions.TimeScales.Fixed);
        StartCoroutine(MovetoMenu());
    }

    private IEnumerator MovetoMenu()
    {
        yield return new WaitForSeconds(fadeOutDuration * 2);
        SceneManager.LoadScene(0);
    }
}
