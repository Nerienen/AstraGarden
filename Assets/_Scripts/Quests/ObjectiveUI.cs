using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUI : MonoBehaviour
{
   [SerializeField] private TMP_Text descriptionText;
   [SerializeField] private Toggle toggle;
   
   public void SetCheck(bool value)
   {
      toggle.isOn = value;
      descriptionText.color = value ? new Color(0.9f, 0.9f, 0.9f, 0.5f) : Color.white;
   }

   public void SetText(string text)
   {
      descriptionText.text = text;
      descriptionText.ForceMeshUpdate();
      GetComponent<RectTransform>().sizeDelta = descriptionText.GetRenderedValues();
   }
}
