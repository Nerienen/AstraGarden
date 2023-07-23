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

   [SerializeField] protected MeshRenderer renderer;
   [SerializeField] protected Material OnMaterial;
   [SerializeField] protected Material OffMaterial;
   public float currentEnergy { get; private set; }
   private List<Material> _materials;

   private void Start()
   {
      _materials = new List<Material>();
      _materials.AddRange(renderer.materials);
      
      if (isFinalDoor)
      {
         _materials[5] = OffMaterial;
         GetComponent<Collider>().enabled = false;
         
         FuseBox.Instance.OnPowerUp += () =>
         {
            _materials[5] = OnMaterial;
            GetComponent<Collider>().enabled = true;
            renderer.SetMaterials(_materials);
         };
      }
      else _materials[5] = OnMaterial;
      renderer.SetMaterials(_materials);


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
