using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectUtils.Helpers;
using TMPro;
using UnityEngine;

public class PlantStatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text nameTypeStatus;
    [SerializeField] private TMP_Text percentages;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetData(PlantData plantData)
    {
        nameTypeStatus.text = $"Name: {plantData.name}\n\n• Type:   {GetType(plantData.type)}\n\n• Status: {GetStatus(plantData.health)}";
        percentages.text = $"-------------------------\n\nGrown Percentage\n{GetPercentage(plantData.growPercentage)}\n\n-------------------------\n\nFruits Grown Percentage\n{GetPercentage(plantData.fruitsGrowPercentage)}";
    }

    private string GetType(Plant.PlantTypes type)
    {
        return type switch
        {
            Plant.PlantTypes.WaterPlant => $"<color={GetColorHex(new Color(0, 8f * 16f / 255f, 1))}>Water</color>",
            Plant.PlantTypes.EnergyPlant => $"<color={GetColorHex(new Color(15f * 16f / 255f, 0.8f, 0))}>Energy</color>",
            Plant.PlantTypes.OxygenPlant => $"<color={GetColorHex(new Color(0.5f, 1f, 1f))}>Oxygen</color>"
        };
    }

    private string GetStatus(float health)
    {
        return health switch
        {
            >= 85 => $"<color={GetColorHex(new Color(0f, 1f, 0.3f))}>Perfect</color>",
            >= 50 => $"<color={GetColorHex(new Color(1f, 0.6f, 0))}>Needs Water</color>",
            > 0 => $"<color={GetColorHex(new Color(1, 0.1f, 0))}>Dehydrated</color>",
            <= 0 => $"<color={GetColorHex(new Color(0.2f, 0.2f, 0.2f))}>Dead</color>"
        };
    }

    private string GetPercentage(float value)
    {
        return $"{(int)(value * 100)}%";
    }

    private string GetColorHex(Color color)
    {
        return $"#{FloatToHex(255f * color.r)}{FloatToHex(255f * color.g)}{FloatToHex(255f * color.b)}";
    }

    private string FloatToHex(float value)
    {
        return $"{NumToHex((int)value / 16)}{NumToHex((int)value % 16)}";

        string NumToHex(int value)
        {
            return value switch
            {
                15 => "F",
                14 => "E",
                13 => "D",
                12 => "C",
                11 => "B",
                10 => "A",
                _ => value + ""
            };
        }
    }

    public async Task ShowStatsAsync()
    {
        transform.localScale = Vector3.zero;
      
        gameObject.SetActive(true);
        await transform.DoScaleAsync(Vector3.one*0.9f, 0.15f, Transitions.TimeScales.Scaled);
    }

    public async Task DisableAsync()
    {
        await transform.DoScaleAsync(Vector3.zero, 0.15f, Transitions.TimeScales.Scaled);
        gameObject.SetActive(false);
        
    }
}
