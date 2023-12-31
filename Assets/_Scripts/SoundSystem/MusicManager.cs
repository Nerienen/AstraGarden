using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MusicManager : MonoBehaviour
{
    private AudioManager _audioManager;
    
    #region Music State Variables
    private int currentTrack = -1;
    private bool isPlayingMusic = false;
    private bool isPlayingAmbient = false;

    #endregion

    #region Sound Instances
    public EventInstance music;
    private EventInstance backgroundAmbience;
    private EventInstance chargeWater;
    private EventInstance chargeEnergy;
    #endregion

    #region Event References
    //private EventReference drift = FMODEvents.instance.drift;
    //private EventReference ending = FMODEvents.instance.drift;
    private EventReference[] tracks;

    #endregion

    public static MusicManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioManager = AudioManager.instance;
        
        tracks = new [] { FMODEvents.instance.drift, FMODEvents.instance.ending };
    }
    
    public void InitializeMusic()
    {
        _audioManager = AudioManager.instance;
        if (FindObjectOfType<MainMenuUI>())
        {
            music = _audioManager.InitializeMusic(FMODEvents.instance.menu);
            return;
        }
        
        music = _audioManager.InitializeMusic(FMODEvents.instance.drift);
        backgroundAmbience = _audioManager.InitializeAmbience(FMODEvents.instance.lowHum);
        chargeEnergy = _audioManager.CreateInstance(FMODEvents.instance.chargeEnergy);
        chargeWater = _audioManager.CreateInstance(FMODEvents.instance.chargeWater);
        chargeEnergy.start();
        chargeWater.start();
    }
    

    // Update is called once per frame
    //void Update()
    //{
        // //If is in normal conditions and it's not playing music, play track 0 (Drift)
        // if (isNormal && !isPlayingMusic) { playTrack(tracks, 0); isPlayingMusic = true; }
        //
        // //If blackout occurs, modify music and ambience accordingly
        // if (isBlackout && !hasTriggeredBlackout) { RuntimeManager.StudioSystem.setParameterByName("isBlackout", 1); hasTriggeredBlackout = true; }
        // if (!isBlackout && hasTriggeredBlackout) { 
        //     RuntimeManager.StudioSystem.setParameterByName("isBlackout", 0); 
        //     //En teor�a solo hay uno, pero lo dejo para que se cambie a falso por si acaso
        //     hasTriggeredBlackout = false; 
        // }

        //if (isEnding & !hasTriggeredTransitionToEnd) { }
    //}

    private void playTrack(EventReference[] tracks, int trackNumber)
    {
        _audioManager.InitializeMusic(tracks[trackNumber]);
    }

    public void SetMusicParameter(string parameterName, float value)
    {
        music.setParameterByName(parameterName, value);
    }
    
    public void SetAmbienceParameter(string parameterName, float value)
    {
        backgroundAmbience.setParameterByName(parameterName, value);
    }
    
    public void SetChargeWaterParameter(string parameterName, float value)
    {
        //chargeWater.getPaused(out bool paused);
        //if (paused && value > 0) chargeWater.start();
        //else if (value <= 0) chargeWater.stop(STOP_MODE.ALLOWFADEOUT);
        chargeWater.setParameterByName(parameterName, value);
    }
    public void SetChargeEnergyParameter(string parameterName, float value)
    {
        //chargeEnergy.getPaused(out bool paused);
        //if (paused && value > 0) chargeEnergy.start();
        //else if (value <= 0) chargeEnergy.stop(STOP_MODE.ALLOWFADEOUT);
        chargeEnergy.setParameterByName(parameterName, value);
    }



}
