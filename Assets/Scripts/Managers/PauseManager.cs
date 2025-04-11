using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace ToyBox.Managers {
    public class PauseManager : MonoBehaviour {
        PlayerInput _menuInput;
        
        PlayerManager _playerManager => PlayerManager.Instance;
        
        public static PauseManager Instance { get; private set; }
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }
        
        void StartPause(Player pausedPlayer) {
            Time.timeScale = 0f;
            _playerManager.PlayerInputManager.enabled = false;
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.enabled = false;
            }
            _menuInput.enabled = true;
            _menuInput.user.UnpairDevices();
            InputUser.PerformPairingWithDevice(pausedPlayer.Device, _menuInput.user);
        }
        
        public void StartPause(GameObject pausedPlayer) {
            StartPause(_playerManager.GetPlayer(pausedPlayer));
        }

        public void EndPause() {
            _menuInput.enabled = false;
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.enabled = true;
            }
            _playerManager.PlayerInputManager.enabled = true;
            Time.timeScale = 1f;
        }

    }
}