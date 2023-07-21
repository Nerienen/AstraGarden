using UnityEngine;

[System.Serializable]
public struct PlantGroup
{
    public Plant.PlantTypes plantType;
    public string plantName;
    public Transform grownPlantHolder;
    public Transform sproutPlantHolder;
}
