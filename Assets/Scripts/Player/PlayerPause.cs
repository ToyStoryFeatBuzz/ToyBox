using UnityEngine;
using ToyBox.InputSystem;
using ToyBox.Managers;

namespace ToyBox.Player {
    public class PlayerPause : MonoBehaviour {
        PauseManager _pauseManager => PauseManager.Instance;
        PlayerEnd _playerEnd;
        private void Start() {
            GetComponent<PlayerInputSystem>().OnPauseEvent.Started += StartPause;
            _playerEnd = GetComponent<PlayerEnd>();
        }
        
        void StartPause() {
            if (_playerEnd.IsDead) {
                return;
            }
            Debug.Log("gros zizi");
            _pauseManager.StartPause(gameObject);
        }
        
    }
}