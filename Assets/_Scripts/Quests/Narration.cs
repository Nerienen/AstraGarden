using System;
using FMODUnity;
using UnityEngine;

[Serializable]
public struct Narration 
{
    [Header("Narration options")] 
    public float startDelay;
    
    [Header("Subtitles")]
    public string text;
    
    [Header("Audio")]
    public EventReference voiceLine;
    public float voiceLineDuration;
}
