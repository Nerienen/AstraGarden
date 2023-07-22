using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBoxEnergyReceiver : MonoBehaviour
{
   [SerializeField] protected MeshRenderer renderer;
   [SerializeField] protected Material OnMaterial;
   [SerializeField] protected Material OffMaterial;
   
   private void Start()
   {
      
      renderer.SetMaterials(new List<Material>(){renderer.materials[0], OffMaterial});
      //GetComponent<Collider>().enabled = false;
      GetComponentInParent<FuseBox>().OnPowerDown += EnableCollider;
   }

   public void EnableCollider()
   {
      GetComponent<Collider>().enabled = true;
   }

   public void AddEnergy()
   {
      AudioManager.instance.PlayOneShot(FMODEvents.instance.energyAbsorb, transform.position);
      renderer.SetMaterials(new List<Material>(){renderer.materials[0], OnMaterial});
      GetComponent<Collider>().enabled = false;
      
      GetComponentInParent<FuseBox>().PowerOn();
   }
}
