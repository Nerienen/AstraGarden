using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyReceiver : MonoBehaviour
{
   [SerializeField] float oxygenRateToDecrease = 3f;

   [SerializeField] private GameObject door;
   
   [SerializeField] private float energyNeeded;
   public float currentEnergy { get; private set; }

   private void OpenDoor()
   {
      door.GetComponent<Animator>().SetBool("Opened", true);
      gameObject.SetActive(false);
      tag = "Untagged";
      
        if (OxygenController.Instance != null)
        {
            OxygenController.Instance.DecreaseOxygenRateBy(oxygenRateToDecrease);
        }
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
