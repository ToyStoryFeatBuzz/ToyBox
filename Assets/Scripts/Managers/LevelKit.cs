using System;
using UnityEngine;

namespace ToyBox.Managers {
    public class LevelKit : MonoBehaviour {
        [SerializeField] private GameObject _UI;
        [SerializeField] Timer.Timer _timer;
        public static LevelKit Instance { get; set; }

        public void ToggleUI(bool toggle) => _UI.SetActive(toggle);
        
        public void StartTimer() => _timer.StartTimer();
        public void StopTimer() => _timer.StopTimer();
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }
    }
}
