using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;
    
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
    public void PlaySound(AudioClip clip)
    { 
        if(clip == null)
        {
            Debug.LogError("AudioClip is missing!");
            return;
        }
        
        effectSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        if(clip == null)
        {
            Debug.LogError("AudioClip is missing!");
            return;
        }
        
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
    
    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void ChangeEffectsVolume(float value)
    {
        effectSource.volume = value;
    }

    public void StopMusic()
    {
        musicSource.Pause();
    }

    public void StopEffect()
    {
        effectSource.Stop();
    }

    public void ResumeMusic()
    {
        musicSource.Play();
    }
    
    public void ToggleEffects()
    {
        effectSource.mute = !effectSource.mute;
    }
    
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
}