using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEventChecker : MonoBehaviour
{
    [SerializeField] private Plant initialSprout;
    [SerializeField] private Tank energyTank;
    [SerializeField] private FluidGun fluidGun;
    [SerializeField] private EnergyReceiver firstReceiver;
    
    private void Start()
    {
        //First Quest
        initialSprout.onGrabObject += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.GrabSprout);
        initialSprout.OnChangeTypeReceived += type =>
        {
            if (type == Plant.PlantTypes.OxygenPlant)
                QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.ChangeToOxygenType);
        };
        
        //Second Quest
        initialSprout.OnPlantHasBeenHolden += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.PlantSprout);
        initialSprout.OnPlantReceivedWater += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.WaterPlant);
        initialSprout.OnPlantFullyGrown += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.GrowPlant);

        //Third Quest
        energyTank.onGrabObject += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.GrabEnergyTank);
        energyTank.onGetLiquid += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.FillMachine);
        
        //Fourth Quest
        Plant[] plants = FindObjectsOfType<Plant>();
        foreach (var plant in plants)
        {
            plant.OnChangeTypeReceived += type =>
            {
                if (type != Plant.PlantTypes.EnergyPlant) return;
                
                QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.ChangeToEnergyType);
                FourthQuestAddEvents(plant);
            };
            
            if(plant.CurrentType == Plant.PlantTypes.EnergyPlant) FourthQuestAddEvents(plant);
        }
        
        //Fifth Quest
        fluidGun.OnChangeType += type =>
        {
            if (type == AmmoType.EnergyAmmo)
                QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.ChangeGunMode);
        };
        firstReceiver.onReceiveEnergy += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.ShootEnergyToEnergyReceiver);
    }

    private readonly HashSet<Plant> _plants = new();
    private void FourthQuestAddEvents(Plant plant)
    {
        if(_plants.Contains(plant)) return;
        
        _plants.Add(plant);
        plant.OnPlantHasBeenHolden += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.PlantEnergySprout);
        plant.OnCollectPlant += () => QuestManager.instance.TryCompleteCurrentQuest(QuestObjective.CollectEnergy);
    }
}
