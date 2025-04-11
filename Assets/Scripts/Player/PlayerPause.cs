using UnityEngine;
using ToyBox.InputSystem;
using ToyBox.Managers;

namespace ToyBox.Player {
    public class PlayerPause : MonoBehaviour {
        PauseManager _pauseManager => PauseManager.Instance;
        
        private void Start() {
            GetComponent<PlayerInputSystem>().OnPauseEvent.Started += StartPause;
        }
        
        void StartPause() {
            _pauseManager.StartPause(transform.root.gameObject);
        }
        
    }
}