using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GetSetAudioMixer : MonoBehaviour
{
    public AudioMixer _audioMixer;
    
    
    [Header("~~~~~~ General ~~~~~~")] 
    public StVolumeSlider _mainSlider;
        
    [Header("~~~~~~ Music ~~~~~~")]
    public StVolumeSlider _musicSlider;
        
    [Header("~~~~~~ SFX ~~~~~~")]
    public StVolumeSlider _sfxSlider;

    public static GetSetAudioMixer Instance;

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }
    }

    public void SetMusicSound(float volume)
    {
        SetVolume("Music", volume);
        SetVolumePercentage(_musicSlider);
    }

    public void SetSFXSound(float volume)
    {
        SetVolume("SFX", volume);
        SetVolumePercentage(_sfxSlider);
    }

    public void SetMainSound(float volume)
    {
        SetVolume("Master", volume);
        SetVolumePercentage(_mainSlider);
    }

    public void SetVolume(string volumeName, float volume)
    {
        if (!_audioMixer.SetFloat(volumeName, volume))
        {
            Debug.LogWarning("Audio parameter not found : " + volumeName);
        }
    }

    public float GetVolume(string volumeName)
    {
        float value;
        if (_audioMixer.GetFloat(volumeName, out value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("Audio parameter not found : " + volumeName);
            return 0f;
        }
    }

    void SetVolumePercentage(StVolumeSlider _soundSlider)
    {
        
        float rslt=(5*_soundSlider.Slider.value)/4+100;
        _soundSlider.TextPercent.text = $"{(int)rslt} %";
    }
    [Serializable]
    public struct StVolumeSlider {
        public string Name;
        public Slider Slider;
        public TextMeshProUGUI TextPercent;
    }
}
