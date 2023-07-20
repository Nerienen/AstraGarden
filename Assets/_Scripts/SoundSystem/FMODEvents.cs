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

    //Play when plant data visualizer is opened
    [field: SerializeField] public EventReference openData { get; private set; }
    //Play while data visualizer is open
    [field: SerializeField] public EventReference dataViewerSound { get; private set; }
    //Play when data visualizer is closed
    [field: SerializeField] public EventReference closeData { get; private set; }


    [field: Header("Water SFX")]
    //Play on water rebound
    [field: SerializeField] public EventReference waterReboundOnWall { get; private set; }
    //Play on blob disappear
    [field: SerializeField] public EventReference waterDisipate { get; private set; }
    //Play while blob is active
    [field: SerializeField] public EventReference waterFloatingSound { get; private set; }


    [field: Header("Water SFX")]
    //Play on energy rebound
    [field: SerializeField] public EventReference energyReboundOnWall { get; private set; }
    //Play on blob disappear
    [field: SerializeField] public EventReference energyDisipate { get; private set; }
    //Play while blob is active
    [field: SerializeField] public EventReference energyFloatingSound { get; private set; }

    #endregion

    #region Interactable Sounds
    //Play when energy is absorbed
    [field: SerializeField] public EventReference energyAbsorb { get; private set; }
    //Play when max energy needed to open a door is reached
    [field: SerializeField] public EventReference doorGetMaxEnergy { get; private set; }
    //Play on door open
    [field: SerializeField] public EventReference doorOpen { get; private set; }

    #endregion

    #region Plant sounds
    //On plant growth, send a parameter "Plant Type" 0, 1, 2 (oxy, water, energy), then play
    [field: SerializeField] public EventReference plantGrow { get; private set; }
    //On water absorb, send a parameter "Plant Type" 0, 1, 2 (oxy, water, energy), then play
    [field: SerializeField] public EventReference waterAbsorb { get; private set; } //Planta absorbe agua
    //On bubble spawn, play sound
    [field: SerializeField] public EventReference oxyPlantBubble { get; private set; }

    #endregion

    #region Voices
    //
    [field: SerializeField] public EventReference voiceWarningLowOxygen { get; private set; }
    //
    [field: SerializeField] public EventReference voiceWarningVeryLowOxygen { get; private set; }
    //
    [field: SerializeField] public EventReference voiceNormalSystemsRestablished { get; private set; }
    //
    


    #endregion

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference footSteps { get; private set; }

    

    
    
    public static FMODEvents instance { get; private set; }
    
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    
}
