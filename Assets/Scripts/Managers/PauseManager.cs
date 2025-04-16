using ToyBox.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace ToyBox.Managers {
    public class PauseManager : MonoBehaviour {
        PlayerInput _menuInput;
        
        PlayerManager _playerManager => PlayerManager.Instance;
        
        public static PauseManager Instance { get; private set; }
        
        private MenuManager _menuManager => MenuManager.Instance;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            _menuInput = GetComponent<PlayerInput>();
            Debug.Log(_menuInput);
        }

        void StartPause(Player pausedPlayer) {
            if (!_menuManager) {
                return;
            }
            Time.timeScale = 0f;
            _playerManager.PlayerInputManager.enabled = false;
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.enabled = false;
            }
            _menuInput.enabled = true;
            _menuInput.user.UnpairDevices();
            if (pausedPlayer.Device == null) return;
            InputUser.PerformPairingWithDevice(pausedPlayer.Device, _menuInput.user);
            _menuManager.gameObject.SetActive(true);
        }
        
        public void StartPause(GameObject pausedPlayer) {
            StartPause(_playerManager.GetPlayer(pausedPlayer));
        }

        public void EndPause() {
            _menuManager.gameObject.SetActive(false);
            _menuInput.enabled = false;
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.enabled = true;
            }
            _playerManager.PlayerInputManager.enabled = true;
            Time.timeScale = 1f;
        }

    }
}