using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyReceiver : MonoBehaviour
{
   [SerializeField] private GameObject door;
   
   [SerializeField] private float energyNeeded;
   public float currentEnergy { get; private set; }

   private void OpenDoor()
   {
      door.GetComponent<Animator>().SetBool("Opened", true);
      tag = "Untagged";
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
