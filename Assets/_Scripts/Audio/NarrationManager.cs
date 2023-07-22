using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD.Studio;
using FMODUnity;
using TMPro;
using UnityEngine;

public class NarrationManager : MonoBehaviour
{
    private AudioManager _audioManager;
    private FMODEvents _events;

    private LinkedList<Narration> _soundsToPlay;
    [SerializeField] private TMP_Text subtitles;

    private float _delay;
    private float _timeToStopSubs;

    private StudioEventEmitter _emitter;

    void Start()
    {
        _emitter = GetComponent<StudioEventEmitter>();
        _audioManager = AudioManager.instance;
        _events = FMODEvents.instance;
        _soundsToPlay = new LinkedList<Narration>();
        
        AddInitialSounds();
        FuseBox.Instance.OnPowerDown += AddBlackOutSounds;
    }

    public void PlaySound(float delay, EventReference eventReference, string subs, float duration = 0)
    {
        _soundsToPlay.AddLast(new Narration(eventReference, Time.time+delay, subs, duration));
    }

    private void Update()
    {
        try
        {
            foreach (var narration in _soundsToPlay.Where(narration => narration.delay <= Time.time))
            {
                _timeToStopSubs = float.MaxValue;
                subtitles.transform.parent.gameObject.SetActive(true);
                subtitles.text = narration.subtitles;
                _emitter.EventReference = narration.eventReference;
                _emitter.Stop();
                _emitter.Lookup();
                _emitter.Play();
                _soundsToPlay.Remove(narration);
                if (_soundsToPlay.Count <= 0) _timeToStopSubs = Time.time+narration.duration;
            }
            if(_timeToStopSubs <= Time.time) subtitles.transform.parent.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            // ignored
        }

        if (Time.timeScale == 0)
        {
            _emitter.EventInstance.setPaused(true);
            subtitles.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _emitter.EventInstance.getPaused(out var paused);
            if (paused)
            {
                _emitter.EventInstance.setPaused(false);
                subtitles.transform.parent.gameObject.SetActive(true);
            }
        }
    }
    
    private void AddInitialSounds()
    {
        _delay++;
        PlaySound(_delay, _events.voiceRebootingSystem, "Rebooting system...");
        _delay += 2f;
        PlaySound(_delay, _events.voiceLifeSignsDetected, "Life Signs Detected.");
        _delay += 2.2f;
        PlaySound(_delay, _events.voiceHelloGardener, "Hello, Gardener.");
        _delay += 2.5f;
        PlaySound(_delay, _events.voiceOxygenLow, "Oxygen Low...");
        _delay += 2.5f;
        PlaySound(_delay, _events.voiceCriticalSituation, "Critical situation detected. Initiating Emergency Protocol.");
        _delay += 5f;
        PlaySound(_delay, _events.voiceOxyPlantTutorial, "Please water the Bubbling Oxygen plants to avoid asphyxiation. They produce a catallist that renews the oxygen of a room.");
        _delay += 11f;
        PlaySound(_delay, _events.voiceGunTutorial, "Use your \"Gun\" to water the plants. Press the gun's left trigger to charge a water blob. Press the right trigger to release it. Ensure that the watering mode, blue, is active, use the gun's wheel to modify the mode.");
        _delay += 19f;
        PlaySound(_delay, _events.voiceWaterPlantTutorial, "Ensure to keep them alive. Extract the necessary water from the watering cacti, they collect moisture from the air and concentrate it.", 15);
        _delay += 15;
    }

    private void AddBlackOutSounds()
    {
        if (_delay < Time.time) _delay = 0;
        else _delay -= Time.time;

        _delay += 0.5f;
        PlaySound(_delay, _events.voiceEnergySystemsEmpty, "Energy systems empty.");
        _delay += 3f;
        PlaySound(_delay, _events.voiceCriticalSituation, "Critical situation detected. Initiating Emergency Protocol.");
        _delay += 4.5f;
        PlaySound(_delay, _events.voiceOpenMaintenance, "Open the door to the maintenance room. Manually reset the circuit breaker in the maintenance room. The door has to be energized.");
        _delay += 10f;
        PlaySound(_delay, _events.voiceGeneticManipulator, "Use the Genetic Manipulator to transform plants to other types. Create energy generating Star Plants. They create a highly conductive liquid that can replenish batteries.", 12);
        _delay += 12f;
    }
}

public struct Narration
{
    public EventReference eventReference;
    public float delay;
    public string subtitles;
    public float duration;

    public Narration(EventReference eventReference, float delay, string subtitles, float duration = 0)
    {
        this.eventReference = eventReference;
        this.delay = delay;
        this.subtitles = subtitles;
        this.duration = duration;
    }
}
