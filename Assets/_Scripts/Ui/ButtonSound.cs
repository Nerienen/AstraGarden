using System.Collections;
using System.Collections.Generic;
using ProjectUtils.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    private void PlaySelectSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.menuHover, Helpers.Camera.transform.position);
        }
    }

    private void PlaySubmitSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.menuSelect, Helpers.Camera.transform.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySelectSound();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlaySubmitSound();
    }

    public void OnSelect()
    {
        PlaySelectSound();
    }

    public void OnSubmit()
    {
        PlaySubmitSound();
    }
}
