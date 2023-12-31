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
    [SerializeField] private TMP_Text subtitles;
    private StudioEventEmitter _emitter;

    private Coroutine _currentNarration;
    private bool _narrationIsRunning;

    public static NarrationManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        _emitter = GetComponent<StudioEventEmitter>();

        UiController.instance.onSetPaused += paused =>
        {
            if (!_narrationIsRunning) return;

            if (paused)
            {
                _emitter.EventInstance.setPaused(true);
                subtitles.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                _emitter.EventInstance.setPaused(false);
                subtitles.transform.parent.gameObject.SetActive(true);
            }
        };
    }

    public void StartNarration(Narration[] narrations, int index = 0)
    {
        if (_currentNarration != null && _narrationIsRunning) StopCoroutine(_currentNarration);

        subtitles.transform.parent.gameObject.SetActive(true);
        _currentNarration = StartCoroutine(Narrate(narrations, index));
    }
    
    private IEnumerator Narrate(Narration[] narrations, int index)
    {
        _narrationIsRunning = true;
        
        while (index < narrations.Length)
        {
            Narration narration = narrations[index++];
            
            yield return new WaitForSeconds(narration.startDelay);
            ChangeEmitterAudio(narration.voiceLine);
            subtitles.text = narration.text;
            yield return new WaitForSeconds(narration.voiceLineDuration);
        }

        yield return new WaitForSeconds(1f);
        subtitles.transform.parent.gameObject.SetActive(false);

        _narrationIsRunning = false;
    }

    private void ChangeEmitterAudio(EventReference audio)
    {
        _emitter.EventReference = audio;
        _emitter.Stop();
        _emitter.Lookup();
        _emitter.Play();
    }

    // private void AddInitialSounds()
    // {
    //     _delay++;
    //     PlaySound(_delay, _events.voiceRebootingSystem, "Rebooting system...");
    //     _delay += 2f;
    //     PlaySound(_delay, _events.voiceLifeSignsDetected, "Life Signs Detected.");
    //     _delay += 2.2f;
    //     PlaySound(_delay, _events.voiceHelloGardener, "Hello, Gardener.");
    //     _delay += 2.5f;
    //     PlaySound(_delay, _events.voiceOxygenLow, "Oxygen Low...");
    //     _delay += 2.5f;
    //     PlaySound(_delay, _events.voiceCriticalSituation, "Critical situation detected. Initiating Emergency Protocol.");
    //     _delay += 5f;
    //     PlaySound(_delay, _events.voiceOxyPlantTutorial, "Please water the Bubbling Oxygen plants to avoid asphyxiation. They produce a catallist that renews the oxygen of a room.");
    //     _delay += 11f;
    //     PlaySound(_delay, _events.voiceGunTutorial, "Use your \"Gun\" to water the plants. Press the gun's left trigger to charge a water blob. Press the right trigger to release it. Ensure that the watering mode, blue, is active, use the gun's wheel to modify the mode.");
    //     _delay += 19f;
    //     PlaySound(_delay, _events.voiceWaterPlantTutorial, "Ensure to keep them alive. Extract the necessary water from the watering cacti, they collect moisture from the air and concentrate it.", 15);
    //     _delay += 15;
    // }

    // private void AddBlackOutSounds()
    // {
    //     if (_delay < Time.time) _delay = 0;
    //     else _delay -= Time.time;
    //
    //     _delay += 0.5f;
    //     PlaySound(_delay, _events.voiceEnergySystemsEmpty, "Energy systems empty.");
    //     _delay += 3f;
    //     PlaySound(_delay, _events.voiceCriticalSituation, "Critical situation detected. Initiating Emergency Protocol.");
    //     _delay += 4.5f;
    //     PlaySound(_delay, _events.voiceOpenMaintenance, "Open the door to the maintenance room. Manually reset the circuit breaker in the maintenance room. The door has to be energized.");
    //     _delay += 10f;
    //     PlaySound(_delay, _events.voiceGeneticManipulator, "Use the Genetic Manipulator to transform plants to other types. Create energy generating Star Plants. They create a highly conductive liquid that can replenish batteries.", 12);
    //     _delay += 12f;
    // }
}