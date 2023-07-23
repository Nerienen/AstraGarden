using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenDoor : MonoBehaviour
{
    private Animator _animator;
    private ParticleSystem[] _particle;
    private void Start()
    {
        _particle = GetComponentsInChildren<ParticleSystem>();
        _animator = GetComponent<Animator>();

        FuseBox.Instance.OnPowerDown += FixDoor;
    }

    public void PlaySounds()
    {
        //AudioManager.instance.PlayOneShot(FMODEvents.instance.doorBroken, transform.position);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.doorBang, transform.position);


    }

    private void OnDestroy()
    {
        FuseBox.Instance.OnPowerDown -= FixDoor;
    }

    private void FixDoor()
    {
        _animator.SetBool("Opened", true);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.doorOpen, transform.position);

        foreach (var particle in _particle)
        {
            particle.gameObject.SetActive(false);
        }
    }
}
