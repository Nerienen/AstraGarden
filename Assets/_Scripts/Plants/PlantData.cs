using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantData
{
    public string name { get; private set; }
    public float health { get; private set; }
    public float growPercentage { get; private set; }
    public float fruitsGrowPercentage { get; private set; }
    public bool needsLightToGrow { get; private set; }
    public Plant.PlantTypes type { get; private set; }

    public PlantData(string name, float health, float growPercentage, float fruitsGrowPercentage, bool needsLightToGrow, Plant.PlantTypes type)
    {
        this.name = name;
        this.health = health;
        this.growPercentage = growPercentage;
        this.fruitsGrowPercentage = fruitsGrowPercentage;
        this.needsLightToGrow = needsLightToGrow;
        this.type = type;
    }

    public void UpdatePlantData(string name, float health, float growPercentage, float fruitsGrowPercentage, bool needsLightToGrow, Plant.PlantTypes type)
    {
        this.name = name;
        this.health = health;
        this.growPercentage = growPercentage;
        this.fruitsGrowPercentage = fruitsGrowPercentage;
        this.needsLightToGrow = needsLightToGrow;
        this.type = type;
    }
}
