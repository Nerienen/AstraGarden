using System;
using UnityEngine;

public class EmergencyButton : Interactable
{
    Animator _animator;

    bool _hasBeenInteracted;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponent<Animator>();
    }

    void ProcessInteraction()
    {
        _animator.SetTrigger("Press");

        if (CutsceneController.Instance != null && FMODEvents.instance != null)
        {
            MusicManager.Instance.music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.openBlindEnding, transform.position);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.ending, transform.position);
            CutsceneController.Instance.PreparePlayer();
        }
    }

    public override bool Interact()
    {
        if (_hasBeenInteracted)
            return false;

        _hasBeenInteracted = true;
        ProcessInteraction();

        return _hasBeenInteracted;
    }

    public override bool Interact(Transform grabPoint)
    {
        return false;
    }
}
