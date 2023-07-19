using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
   
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference lowHum { get; private set; }
    [field: SerializeField] public EventReference fan { get; private set; }
    [field: SerializeField] public EventReference rattlingAC { get; private set; }


    [field: Header("Music")]
    [field: SerializeField] public EventReference drift { get; private set; }
    [field: SerializeField] public EventReference ending { get; private set; }


    #region Gun and blob sounds

    [field: Header("Gun SFX")]
    [field: SerializeField] public EventReference chargeWater { get; private set; }
    [field: SerializeField] public EventReference chargeEnergy{ get; private set; }
    [field: SerializeField] public EventReference shootWater { get; private set; }
    [field: SerializeField] public EventReference shootEnergy { get; private set; }

    [field: SerializeField] public EventReference changeMode { get; private set; }


    [field: Header("Water SFX")]
    [field: SerializeField] public EventReference waterReboundOnWall { get; private set; }
    [field: SerializeField] public EventReference waterDisipate { get; private set; }
    [field: SerializeField] public EventReference waterFloatingSound { get; private set; }


    [field: Header("Water SFX")]
    [field: SerializeField] public EventReference energyReboundOnWall { get; private set; }
    [field: SerializeField] public EventReference energyDisipate { get; private set; }
    [field: SerializeField] public EventReference energyFloatingSound { get; private set; }

    #endregion



    #region Misc Object Sounds


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
