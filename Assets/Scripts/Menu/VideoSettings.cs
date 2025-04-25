using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ToyBox.Menu  {
    public class VideoSettings : MonoBehaviour {
       [SerializeField] TMP_Dropdown _resolutionDropdown;
       [SerializeField] TMP_Dropdown _displayDropdown;

        List<Resolution> _filteredResolutions = new ();
        RefreshRate _currentRefreshRate;
        
        int _oldResolutionID;
        int _currentResolutionID;
        int _currentDisplayID;

        private void Start() {
            GetAllResolutions();
            
            GetSavedResolution();
            ApplyResolution();
            
            GetSavedDisplay();
            ApplyDisplay();
        }

        private void GetAllResolutions() {
            _resolutionDropdown.ClearOptions();
            _currentRefreshRate = Screen.currentResolution.refreshRateRatio;

            foreach (Resolution resolution in Screen.resolutions) {
                if (Math.Abs(resolution.refreshRateRatio.value - _currentRefreshRate.value) < 0.0001f) {
                    _filteredResolutions.Add(resolution);
                }
            }
            _filteredResolutions.Reverse();
            
            List<string> options = new ();
            for (int u = 0; u < _filteredResolutions.Count; u++) {
                Resolution resolution = _filteredResolutions[u];
                options.Add($"{resolution.width} x {resolution.height}");
                if (resolution.width == Screen.width && resolution.height == Screen.height) {
                    _oldResolutionID = u;
                }
            }

            _resolutionDropdown.AddOptions(options);
            _resolutionDropdown.RefreshShownValue();
        }

        private void GetSavedResolution() {
            _currentResolutionID = !PlayerPrefs.HasKey("Resolution") ? _oldResolutionID : PlayerPrefs.GetInt("Resolution");
            _resolutionDropdown.value = _currentResolutionID;
        }

        public void ApplyResolution() {
            Resolution resolution = _filteredResolutions[_currentResolutionID];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode, _currentRefreshRate);
            PlayerPrefs.SetInt("Resolution", _currentResolutionID);
            PlayerPrefs.Save();
        }
        
        public void SetResolution(int resolutionID) {
            _currentResolutionID = resolutionID;
        }   


        void GetSavedDisplay() {
            _currentDisplayID = PlayerPrefs.GetInt("Display");
            _displayDropdown.value = _currentDisplayID;
        }

        public void ApplyDisplay() {
            switch (_currentDisplayID) {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
                case 3:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
            }
            
            PlayerPrefs.SetInt("Display", _currentDisplayID);
            PlayerPrefs.Save();
        }
        
        public void SetDisplay(int displayID) {
            _currentDisplayID = displayID;
        }
        
        public void Default() {
            _currentDisplayID = 0;
            _displayDropdown.value = _currentDisplayID;
            ApplyDisplay();
            
            _currentResolutionID = _oldResolutionID;
            _resolutionDropdown.value = _currentResolutionID;
            ApplyResolution();
        }
            
    }
}

