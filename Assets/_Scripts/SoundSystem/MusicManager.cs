using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicManager : MonoBehaviour
{
    private AudioManager _audioManager;

    #region Game State Variables
    public bool isIntro = true;
    public bool hasTriggeredIntro = false;

    public bool isNormal = false;

    public bool isBlackout = false;
    public bool hasTriggeredBlackout = false;

    public bool isEnding = false;
    public bool hasTriggeredTransitionToEnd = false;
    public bool hasTriggeredEndingSequence = false;
    #endregion

    #region Music State Variables
    private int currentTrack = -1;
    private bool isPlayingMusic = false;
    private bool isPlayingAmbient = false;

    #endregion

    #region Sound Instances
    private EventInstance music;
    private EventInstance backgroundAmbience;


    #endregion

    #region Event References
    //private EventReference drift = FMODEvents.instance.drift;
    //private EventReference ending = FMODEvents.instance.drift;
    private EventReference[] tracks = { FMODEvents.instance.drift, FMODEvents.instance.ending };

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
        
        //Por ahora se asume que se empieza siempre en el principio
        music = _audioManager.InitializeMusic(FMODEvents.instance.drift);
        backgroundAmbience = _audioManager.InitializeAmbience(FMODEvents.instance.lowHum);
    }

    // Update is called once per frame
    void Update()
    {
        //If is in normal conditions and it's not playing music, play track 0 (Drift)
        if (isNormal & !isPlayingMusic) { playTrack(tracks, 0); isPlayingMusic = true; }

        //If blackout occurs, modify music and ambience accordingly
        if (isBlackout & !hasTriggeredBlackout) { RuntimeManager.StudioSystem.setParameterByName("isBlackout", 1); hasTriggeredBlackout = true; }
        if (!isBlackout & hasTriggeredBlackout) { 
            RuntimeManager.StudioSystem.setParameterByName("isBlackout", 0); 
            //En teor�a solo hay uno, pero lo dejo para que se cambie a falso por si acaso
            hasTriggeredBlackout = false; 
        }

        if (isEnding & !hasTriggeredTransitionToEnd) { }



    }

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

    
}
