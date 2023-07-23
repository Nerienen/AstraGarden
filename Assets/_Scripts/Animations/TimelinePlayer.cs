using System;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class TimelinePlayer : MonoBehaviour
{
    public event Action OnFinished;

    PlayableDirector _playableDirector;
    bool _isPlaying;

    void Awake()
    {
        _playableDirector = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (!_isPlaying)
            return;

        if (_playableDirector.state != PlayState.Playing)
        {
            _isPlaying = false;
            OnFinished?.Invoke();
        }
    }

    public void Play()
    {
        _playableDirector.Play();
        _isPlaying = true;
    }
}
