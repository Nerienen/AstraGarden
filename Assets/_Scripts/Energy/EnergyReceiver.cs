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
   [SerializeField] private bool isFinalDoor;

   [SerializeField] private MeshRenderer renderer;
   [SerializeField] private Material OnMaterial;
   [SerializeField] private Material OffMaterial;
   public float currentEnergy { get; private set; }

   private void Start()
   {
      if (isFinalDoor)
      {
         renderer.materials[5] = OffMaterial;
         GetComponent<Collider>().enabled = false;
      }
      renderer.materials[5] = OnMaterial;
   }

   private void OpenDoor()
   {
      AudioManager.instance.PlayOneShot(FMODEvents.instance.doorGetMaxEnergy, transform.position);
      AudioManager.instance.PlayOneShot(FMODEvents.instance.doorOpen, transform.position);
      door.GetComponent<Animator>().SetBool("Opened", true);
      GetComponent<Collider>().enabled = false;
      tag = "Untagged";
      if(isFinalDoor) MusicManager.Instance.SetMusicParameter("endPiece", 1);
      
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
      AudioManager.instance.PlayOneShot(FMODEvents.instance.energyAbsorb, transform.position);
      currentEnergy += quantity;

      if (currentEnergy >= energyNeeded)
      {
         currentEnergy = energyNeeded;
         OpenDoor();
      }
   }
}
