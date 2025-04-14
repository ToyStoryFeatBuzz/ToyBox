using UnityEngine;
using ToyBox.InputSystem;
using ToyBox.Managers;

namespace ToyBox.Player {
    public class PlayerPause : MonoBehaviour {
        PauseManager _pauseManager => PauseManager.Instance;
        private PlayerEnd _playerEnd;
        private void Start() {
            GetComponent<PlayerInputSystem>().OnPauseEvent.Started += StartPause;
            _playerEnd = GetComponent<PlayerEnd>();
        }
        
        void StartPause() {
            if (_playerEnd.IsDead) {
                return;
            }
            _pauseManager.StartPause(gameObject);
        }
        
    }
}