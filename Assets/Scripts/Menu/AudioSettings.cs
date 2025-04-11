using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class AudioSettings : MonoBehaviour {
        [Header("~~~~~~ General ~~~~~~")] 
        [SerializeField] private StVolumeSlider _generalVolume;
        
        [Header("~~~~~~ Music ~~~~~~")]
        [SerializeField] private StVolumeSlider _musicVolume;
        
        [Header("~~~~~~ SFX ~~~~~~")]
        [SerializeField] private StVolumeSlider _sfxVolume;


        [Header("~~~~~~ Audio Mixer ~~~~~~")] 
        [SerializeField] private AudioMixer _audioMixer;
        
        private void Start() {
            _generalVolume.Slider.onValueChanged.AddListener(delegate{SetVolume(_generalVolume);});
            _musicVolume.Slider.onValueChanged.AddListener(delegate{SetVolume(_musicVolume);});
            _sfxVolume.Slider.onValueChanged.AddListener(delegate{SetVolume(_sfxVolume);});

            GetSavedVolume();
        }

        void SetVolume(StVolumeSlider volumeSlider) {
            _audioMixer.SetFloat(volumeSlider.Name, Mathf.Log10(volumeSlider.Slider.value) * 20);
            volumeSlider.TextPercent.text = $"{Mathf.RoundToInt(volumeSlider.Slider.value * 100)} %";
            PlayerPrefs.SetFloat(volumeSlider.Name, volumeSlider.Slider.value);
            PlayerPrefs.Save();
        }
        
        void SetVolume(StVolumeSlider volumeSlider, float value) {
            _audioMixer.SetFloat(volumeSlider.Name, Mathf.Log10(value) * 20);
            PlayerPrefs.SetFloat(volumeSlider.Name, value);
            PlayerPrefs.Save();
        }


        void GetSavedVolume() {
            _generalVolume.Slider.value = !PlayerPrefs.HasKey(_generalVolume.Name) ? 1f : PlayerPrefs.GetFloat(_generalVolume.Name);
            _musicVolume.Slider.value = !PlayerPrefs.HasKey(_musicVolume.Name) ? 1f : PlayerPrefs.GetFloat(_musicVolume.Name);
            _sfxVolume.Slider.value = !PlayerPrefs.HasKey(_sfxVolume.Name) ? 1f : PlayerPrefs.GetFloat(_sfxVolume.Name);
        }

        void Default() {
            SetVolume(_generalVolume, 1f);
            SetVolume(_musicVolume, 1f);
            SetVolume(_sfxVolume, 1f);
        }
    }

    [Serializable]
    public struct StVolumeSlider {
        public string Name;
        public Slider Slider;
        public TextMeshProUGUI TextPercent;
    }
    
}