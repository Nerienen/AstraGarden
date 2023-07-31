using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public struct Narration 
{
    public float startDelay;
    
    public string text;
    
    public EventReference voiceLine;
    public float voiceLineDuration;

    public Narration(float startDelay, string text, float voiceLineDuration) : this()
    {
        this.startDelay = startDelay;
        this.text = text;
        this.voiceLineDuration = voiceLineDuration;
    }
}
