using Managers;
using UnityEngine;
using Toybox.InputSystem;

namespace ToyBox.Player {
    public class PlayerPause : MonoBehaviour {
        GameModeManager _gameModeManager => GameModeManager.Instance;
        
        private void Start() {
            GetComponent<PlayerInputManager>().OnPauseEvent.Started += StartPause;
        }
        
        void StartPause() {
            _gameModeManager.StartPause(transform.root.gameObject);
        }
        
    }
}