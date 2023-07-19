using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ambience")]
    //[field: SerializeReference] public EventReference ambience { get; private set; }
    
    [field: Header("Music")]
    [field: SerializeField] public EventReference drift { get; private set; }
    [field: SerializeField] public EventReference ending { get; private set; }
    
    [field: Header("Water SFX")]
    //[field: SerializeReference] public EventReference inflationSound { get; private set; }
    //[field: SerializeReference] public EventReference blobSound { get; private set; }
    
    [field: Header("Player SFX")]
    //[field: SerializeReference] public EventReference footStepsSound { get; private set; }

    
    
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
