using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
   
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference lowHum { get; private set; } //Done, plays by code
    [field: SerializeField] public EventReference fan { get; private set; } //Done, plays by emmitter
    [field: SerializeField] public EventReference rattlingAC { get; private set; } //Done, plays by emmitter
    [field: SerializeField] public EventReference machine { get; private set; } //Done, plays by emmitter
    [field: SerializeField] public EventReference alarm { get; private set; } //Done, plays by emmitter

    [field: Header("Music")]
    [field: SerializeField] public EventReference drift { get; private set; } //Done, plays by code, code pending
    [field: SerializeField] public EventReference ending { get; private set; } //Pending, plays by code, code pending


    #region Gun and blob sounds

    [field: Header("Gun SFX")]
    //Charge sounds: Play on hold left click, one parameter from 0 to 1 indicating relative size to max size of blob
    //PARAMETER NAME: "BubbleGrowth"
    [field: SerializeField] public EventReference chargeWater { get; private set; } //Done
    [field: SerializeField] public EventReference chargeEnergy{ get; private set; } //Done
    //Shoot sounds: Play on press right click, same parameter from 0 to 1 indicating relative size
    [field: SerializeField] public EventReference shootWater { get; private set; }
    [field: SerializeField] public EventReference shootEnergy { get; private set; }

    //Play on press change mode, disable change mode for the duration of the sound
    [field: SerializeField] public EventReference changeMode { get; private set; }

 

    [field: Header("Water SFX")]
    //Play on water rebound
    [field: SerializeField] public EventReference waterReboundOnWall { get; private set; }
    //Play while blob is active
    [field: SerializeField] public EventReference waterFloatingSound { get; private set; }


    [field: Header("Energy SFX")]
    //Play on energy rebound
    [field: SerializeField] public EventReference energyReboundOnWall { get; private set; }
    //Play while blob is active
    [field: SerializeField] public EventReference energyFloatingSound { get; private set; }

    #endregion

    #region Interactable Sounds

    [field: Header("Door SFX")]
    //Play when energy is absorbed
    [field: SerializeField] public EventReference energyAbsorb { get; private set; }
    //Play when max energy needed to open a door is reached
    [field: SerializeField] public EventReference doorGetMaxEnergy { get; private set; }
    //Play on door open
    [field: SerializeField] public EventReference doorOpen { get; private set; }
    //
    [field: SerializeField] public EventReference doorBroken { get; private set; } //Done, plays by emmitter
    [field: SerializeField] public EventReference doorBang{ get; private set; } //Done, plays by emmitter


    [field: Header("Data viewer SFX")]

    //Play when plant data visualizer is opened
    [field: SerializeField] public EventReference openData { get; private set; }
    //Play while data visualizer is open
    //[field: SerializeField] public EventReference dataViewerSound { get; private set; }
    //Play when data visualizer is closed
    [field: SerializeField] public EventReference closeData { get; private set; }

    [field: Header("Machine SFX")]
    [field: SerializeField] public EventReference buttonPress { get; private set; }
    [field: SerializeField] public EventReference processingMachine { get; private set; }
    [field: SerializeField] public EventReference placePlant { get; private set; }
    #endregion

    #region Player

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference footSteps { get; private set; }
    [field: SerializeField] public EventReference pickUpPlant { get; private set; }
    #endregion

    #region Plant sounds
    [field: Header("Plant SFX")]
    //On plant growth, send a parameter "Plant Type" 0, 1, 2 (oxy, water, energy), then play
    [field: SerializeField] public EventReference plantGrow { get; private set; }
    //On water absorb, send a parameter "Plant Type" 0, 1, 2 (oxy, water, energy), then play
    [field: SerializeField] public EventReference waterAbsorb { get; private set; } //Planta absorbe agua
    //On bubble spawn, play sound
    [field: SerializeField] public EventReference oxyPlantBubble { get; private set; }
    [field: SerializeField] public EventReference plantDeath { get; private set; }

    #endregion

    #region Voices
    [field: Header("Voicelines - Scripted")]
    [field: SerializeField] public EventReference voiceRebootingSystem { get; private set; }
    [field: SerializeField] public EventReference voiceLifeSignsDetected { get; private set; }
    [field: SerializeField] public EventReference voiceHelloGardener { get; private set; }
    [field: SerializeField] public EventReference voiceOxyPlantTutorial { get; private set; }
    [field: SerializeField] public EventReference voiceGunTutorial { get; private set; }
    [field: SerializeField] public EventReference voiceWaterPlantTutorial { get; private set; }
    [field: SerializeField] public EventReference voiceOpenMaintenance { get; private set; }
    [field: SerializeField] public EventReference voiceGeneticManipulator { get; private set; }

    [field: Header("Voicelines - Not Scripted")]
    [field: SerializeField] public EventReference voiceCriticalSituation { get; private set; }
    [field: SerializeField] public EventReference voiceSystemsNominal { get; private set; }
    [field: SerializeField] public EventReference voiceOxygenLow { get; private set; }
    [field: SerializeField] public EventReference voiceEnergyLow { get; private set; }
    [field: SerializeField] public EventReference voiceEnergySystemsEmpty{ get; private set; }

    #endregion

    [field: Header("Menu and UI")]
    [field: SerializeField] public EventReference menuHover { get; private set; }
    [field: SerializeField] public EventReference menuSelect { get; private set; }






    public static FMODEvents instance { get; private set; }
    
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    
}
