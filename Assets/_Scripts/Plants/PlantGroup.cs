using UnityEngine;

[System.Serializable]
public struct PlantGroup
{
    public Plant.PlantTypes plantType;
    public string plantName;
    public Transform grownPlantHolder;
    public Transform sproutPlantHolder;

    [Tooltip("By resource we understand the element this plant produce (Oxygen, Water or Energy)")]
    public float resourceCapacity;
}
