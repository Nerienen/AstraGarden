using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectUtils.Helpers;
using TMPro;
using UnityEngine;

public class PlantStatsUI : MonoBehaviour
{
    [SerializeField] private LocalizedDynamicText nameVariableLocalizedText;
    [SerializeField] private LocalizedDynamicText typeVariableLocalizedText;
    [SerializeField] private LocalizedDynamicText statusVariableLocalizedText;
    [SerializeField] private LocalizedDynamicText lightRequirementVariableLocalizedText;

    [SerializeField] private TMP_Text nameVariableText;
    [SerializeField] private TMP_Text typeVariableText;
    [SerializeField] private TMP_Text statusVariableText;
    [SerializeField] private TMP_Text grownPercentageVariableText;
    [SerializeField] private TMP_Text fruitsGrownPercentageVariableText;
    [SerializeField] private TMP_Text lightRequirementVariableText;

    [SerializeField] private TMP_Text nameTypeStatus;
    [SerializeField] private TMP_Text percentages;

    [SerializeField] private float expandedScale = 0.9f;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetData(PlantData plantData)
    {

        nameVariableLocalizedText.DisplayLine(plantData.name);
        typeVariableLocalizedText.DisplayLine(GetType(plantData.type));
        statusVariableLocalizedText.DisplayLine(GetStatus(plantData.health));
        grownPercentageVariableText.text = GetPercentage(plantData.growPercentage);
        fruitsGrownPercentageVariableText.text = GetPercentage(plantData.fruitsGrowPercentage);
        lightRequirementVariableLocalizedText.DisplayLine(GetRequiresLight(plantData.needsLightToGrow));
    }

    private string GetType(Plant.PlantTypes type)
    {
        return type switch
        {
            Plant.PlantTypes.WaterPlant => "type_water",
            Plant.PlantTypes.EnergyPlant => "type_energy",
            Plant.PlantTypes.OxygenPlant => "type_oxygen"
        };
    }

    private string GetStatus(float health)
    {
        return health switch
        {
            >= 85 => "status_perfect",
            >= 50 => "status_needs_water",
            > 0 => "status_dehydrated",
            <= 0 => "status_dead"
        };
    }

    private string GetRequiresLight(bool requiresLight)
    {
        if (requiresLight) { return "light_requirement_yes"; }
        return "light_requirement_no";
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
        await transform.DoScaleAsync(Vector3.one * expandedScale, 0.15f, Transitions.TimeScales.Scaled);
    }

    public async Task DisableAsync()
    {
        await transform.DoScaleAsync(Vector3.zero, 0.15f, Transitions.TimeScales.Scaled);
        gameObject.SetActive(false);
        
    }
}
