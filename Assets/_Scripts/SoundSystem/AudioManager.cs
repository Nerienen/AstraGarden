using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [field: Header("Volume")] 
    [field: Range(0,1)]
    [field: SerializeReference] 
    public float masterVolume { get; private set; } = 1;
    [field: Range(0,1)]
    [field: SerializeReference] 
    public float musicVolume { get; private set; } = 1 ;

    [field: Range(0, 1)]
    [field: SerializeReference]
    public float sfxVolume { get; private set; } = 1;

    [field: Range(0, 1)]
    [field: SerializeReference]
    public float ambienceVolume { get; private set; } = 1;

    private Bus _masterBus;
    private Bus _musicBus;
    private Bus _sfxBus;
    private Bus _ambienceBus;
    
    private List<EventInstance> _eventInstances;
    private List<StudioEventEmitter> _eventEmitters;

    private EventInstance _ambienceEventInstance;
    private EventInstance _musicEventInstance;
    
    public static AudioManager instance { get; private set; }
    
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
       // DontDestroyOnLoad(gameObject);

        _eventEmitters = new List<StudioEventEmitter>();
        _eventInstances = new List<EventInstance>();

        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        
        SetMasterVolume(masterVolume);
        SetAmbienceVolume(ambienceVolume);
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);
    }

    private void Start()
    {
        //InitializeAmbience(FMODEvents.instance.ambience);
        //InitializeMusic(FMODEvents.instance.drift);
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        _masterBus.setVolume(masterVolume);
    }  
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        _musicBus.setVolume(musicVolume);
    } 
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        _sfxBus.setVolume(sfxVolume);
    }  
    public void SetAmbienceVolume(float value)
    {
        ambienceVolume = value;
        _ambienceBus.setVolume(ambienceVolume);
    }

    public EventInstance InitializeAmbience(EventReference instanceAmbience)
    {
        _ambienceEventInstance = CreateInstance(instanceAmbience);
        _ambienceEventInstance.start();
        return _ambienceEventInstance;
    }
    
    public EventInstance InitializeMusic(EventReference instanceMusic)
    {
        _musicEventInstance = CreateInstance(instanceMusic);
        _musicEventInstance.start();
        return _musicEventInstance;
    }
    

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    { 
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (var eventInstance in _eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (var eventEmitter in _eventEmitters)
        {
            eventEmitter.Stop();
        }
    }
    
    private void OnDestroy()
    {
        CleanUp(); 
    }
}