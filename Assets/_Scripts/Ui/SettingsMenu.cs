using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Bindings")] 
    [SerializeField] private GameObject settingsMenu;

    [Header("Buttons")] 
    [SerializeField] private Button exitButton;

    [Header("Sound")]
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider ambienceVolume;
    [SerializeField] private Slider sfxVolume;
    [SerializeField] private Slider musicVolume;
    
    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolution;
    private Vector2 _currentResolution;
    [SerializeField] private TMP_Dropdown screenMode;
    private FullScreenMode _currentScreenMode;

    [Header("FPS")] 
    [SerializeField] private Toggle vSync;

    private void Start()
    {
        AudioManager audioManager = AudioManager.instance;

        if (masterVolume != null && audioManager != null)
        {
            masterVolume.SetValueWithoutNotify(audioManager.masterVolume);
            masterVolume.onValueChanged.AddListener(audioManager.SetMasterVolume);
        }

        if (ambienceVolume != null && audioManager != null)
        {
            ambienceVolume.SetValueWithoutNotify(audioManager.ambienceVolume);
            ambienceVolume.onValueChanged.AddListener(audioManager.SetAmbienceVolume);
        }

        if (sfxVolume != null && audioManager != null)
        {
            sfxVolume.SetValueWithoutNotify(audioManager.sfxVolume);
            sfxVolume.onValueChanged.AddListener(audioManager.SetSFXVolume);
        }

        if (musicVolume != null && audioManager != null)
        {
            musicVolume.SetValueWithoutNotify(audioManager.musicVolume);
            musicVolume.onValueChanged.AddListener(audioManager.SetMusicVolume);
        }

        if (resolution != null)
        {
            resolution.options = new List<TMP_Dropdown.OptionData>()
            {
                new("1280 x 720"), new("1920 x 1080"), new("2560 x 1440"), new("3840 x 2160")
            };
            resolution.onValueChanged.AddListener(ChangeResolution);
        }

        if (screenMode != null)
        {
            screenMode.options = new List<TMP_Dropdown.OptionData>()
            {
                new("Fullscreen"), new("Fullscreen Window"), new("Window")
            };
            screenMode.onValueChanged.AddListener(ChangeScreenMode);
        }

        if (vSync != null)
        {
            vSync.onValueChanged.AddListener(ToggleVsync);
        }

        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitButton);
        }
    }

    public void EnableSettings()
    {
        if (resolution != null)
        {
            _currentResolution = new Vector2(Screen.width, Screen.height);
            
            int index = resolution.options.FindIndex(x => x.text == $"{(int)_currentResolution.x} x {(int)_currentResolution.y}");
            if (index == -1)
            {
                resolution.options.Add(new TMP_Dropdown.OptionData($"{(int)_currentResolution.x} x {(int)_currentResolution.y}"));
                index = resolution.options.Count - 1;
            }
            
            resolution.SetValueWithoutNotify(index);
        }

        if (screenMode != null)
        {
            _currentScreenMode = Screen.fullScreenMode;
            
            int mode = _currentScreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => 0,
                FullScreenMode.FullScreenWindow => 1,
                FullScreenMode.Windowed => 2,
            };
            screenMode.SetValueWithoutNotify(mode);
        }

        if (vSync != null)
        {
            vSync.SetIsOnWithoutNotify(QualitySettings.vSyncCount == 1);
        }
        
        settingsMenu.SetActive(true);
    }
    
    private void ChangeResolution(int res)
    {
        string[] n = resolution.options[res].text.Split(" x ");
        _currentResolution = new Vector2(int.Parse(n[0]), int.Parse(n[1]));
        
        Screen.SetResolution((int) _currentResolution.x, (int) _currentResolution.y, _currentScreenMode);
    }
    
    private void ChangeScreenMode(int res)
    {
        FullScreenMode mode = res switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
        };

        _currentScreenMode = mode;
        Screen.fullScreenMode = mode;
    }

    private void ToggleVsync(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
    }

    public void DisableSettings()
    {
        ExitButton();
    }
    
    private void ExitButton()
    {
        settingsMenu.SetActive(false);
    }
}
