using System;
using UnityEngine;

[Serializable]
public class AmmoData
{
   [Header("Ammo")]
   public float currentAmmo;
   public float maxAmmo;

   [Header("Prefab")]
   public GameObject bulletPrefab;
}
