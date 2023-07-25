using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [field: Header("Volume")] 
    [field: Range(0, 1)] 
    [SerializeField]
    private float masterVolume = 0.8f;
    public float MasterVolume {
        get => PlayerPrefs.GetFloat("MasterVolume", masterVolume);
        set 
        {   
            PlayerPrefs.SetFloat("MasterVolume", value);
            _masterBus.setVolume(value); 
        }
    }
    
    [field: Range(0,1)]
    [SerializeField] 
    private float musicVolume = 0.8f;
    public float MusicVolume {
        get => PlayerPrefs.GetFloat("MusicVolume", musicVolume);
        set 
        {   
            PlayerPrefs.SetFloat("MusicVolume", value);
            _musicBus.setVolume(value); 
        }
    }

    [field: Range(0, 1)]
    [field: SerializeReference]
    private float sfxVolume = 0.8f;
    public float SfxVolume {
        get => PlayerPrefs.GetFloat("SfxVolume", sfxVolume);
        set 
        {   
            PlayerPrefs.SetFloat("SfxVolume", value);
            _sfxBus.setVolume(value); 
        }
    }

    [field: Range(0, 1)]
    [field: SerializeReference]
    private float ambienceVolume = 0.8f;
    public float AmbienceVolume {
        get => PlayerPrefs.GetFloat("AmbienceVolume", ambienceVolume);
        set 
        {   
            PlayerPrefs.SetFloat("AmbienceVolume", value);
            _ambienceBus.setVolume(value); 
        }
    }

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

        _eventEmitters = new List<StudioEventEmitter>();
        _eventInstances = new List<EventInstance>();

        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        
        // #if UNITY_EDITOR
        //     MasterVolume = masterVolume;
        //     AmbienceVolume = ambienceVolume;
        //     SfxVolume = sfxVolume;
        //     MusicVolume = musicVolume;
        // #else
            MasterVolume = MasterVolume;
            AmbienceVolume = AmbienceVolume;
            SfxVolume = SfxVolume;
            MusicVolume = MusicVolume;
        //#endif
        
        DontDestroyOnLoad(gameObject);
    }

    private void Initialize(Scene arg0, Scene arg1)
    {
        CleanUp();
        MusicManager.Instance.InitializeMusic();
    }

    private void Start()
    {
        CleanUp();
        MusicManager.Instance.InitializeMusic();
        SceneManager.activeSceneChanged += Initialize;
        //InitializeAmbience(FMODEvents.instance.ambience);
        //InitializeMusic(FMODEvents.instance.drift);
    }

    public void SetMasterVolume(float value) { MasterVolume = value; }  
    public void SetMusicVolume(float value) { MusicVolume = value; } 
    public void SetSfxVolume(float value) { SfxVolume = value; }  
    public void SetAmbienceVolume(float value) { AmbienceVolume = value; }

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
        _eventInstances ??= new List<EventInstance>();
        _eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (var eventInstance in _eventInstances)
        {
            _musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
            _musicEventInstance.release();
            _ambienceEventInstance.release();
        }

        _eventEmitters.AddRange(FindObjectsOfType<StudioEventEmitter>());
        foreach (var eventEmitter in _eventEmitters)
        {
            eventEmitter.Stop();
        }
    }
    
}