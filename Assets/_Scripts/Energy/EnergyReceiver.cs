using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyReceiver : MonoBehaviour
{
   [SerializeField] float oxygenRateToDecrease = 3f;

   [SerializeField] private GameObject door;

   [SerializeField] private Liquid _liquid;
   
   [SerializeField] private float energyNeeded;
   public float currentEnergy { get; private set; }

   private void OpenDoor()
   {
      door.GetComponent<Animator>().SetBool("Opened", true);
      GetComponent<Collider>().enabled = false;
      tag = "Untagged";
      
        if (OxygenController.Instance != null)
        {
            OxygenController.Instance.DecreaseOxygenRateBy(oxygenRateToDecrease);
        }
   }

   private void Update()
   {
      _liquid.fillAmount = Mathf.Lerp(_liquid.fillAmount, 1.5f - 2f * currentEnergy / energyNeeded, Time.deltaTime*5);
   }

   public void AddEnergy(float quantity)
   {
      currentEnergy += quantity;

      if (currentEnergy >= energyNeeded)
      {
         currentEnergy = energyNeeded;
         OpenDoor();
      }
   }
}
