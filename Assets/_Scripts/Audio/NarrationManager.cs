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

    void Start()
    {
        _audioManager = AudioManager.instance;
        _events = FMODEvents.instance;
        _soundsToPlay = new LinkedList<Narration>();

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
        PlaySound(_delay, _events.voiceGunTutorial, "Use your \"Gun\" to water the plants. Press the gun's left trigger to charge a water blob. Press the right trigger to release it. Ensure that the watering mode, blue, is active, use the gun's wheel to modify the mode.", 19);
        _delay += 19f;
    }

    public void PlaySound(float delay, EventReference eventReference, string subs, float duration = 0)
    {
        _soundsToPlay.AddLast(new Narration(eventReference, Time.time+delay, subs, duration));
    }

    private void Update()
    {
        foreach (var narration in _soundsToPlay.Where(narration => narration.delay <= Time.time))
        {
            _timeToStopSubs = float.MaxValue;
            subtitles.transform.parent.gameObject.SetActive(true);
            subtitles.text = narration.subtitles;
            _audioManager.PlayOneShot(narration.eventReference, transform.position);
            _soundsToPlay.Remove(narration);
            if (_soundsToPlay.Count <= 0) _timeToStopSubs = Time.time+narration.duration;
        }
        if(_timeToStopSubs <= Time.time) subtitles.transform.parent.gameObject.SetActive(false);
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
