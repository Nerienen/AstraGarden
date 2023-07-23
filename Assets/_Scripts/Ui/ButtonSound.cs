using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.menuHover, Helpers.Camera.transform.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.menuSelect, Helpers.Camera.transform.position);
        }
    }
}
