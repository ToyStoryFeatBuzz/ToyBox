using Toybox.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {
    public class GameModeManager : MonoBehaviour {
        public static GameModeManager Instance { get; private set; }

        private PlayerManager _playerManager;
        MenuInputManager _menuInputManager => MenuInputManager.Instance;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            _playerManager = GetComponent<PlayerManager>();
        }
        
        public void StartRaceMode() {
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Race");
            }
        }

        public void StartConstructMode() {
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
            }
        }

        public void StartPause() {
            Time.timeScale = 0f;
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.enabled = false;
            }

            _playerManager.enabled = false;
            _menuInputManager.MenuInput.enabled = true;
        }

        public void EndPause() {
            _menuInputManager.MenuInput.enabled = false;
                        foreach (StPlayer player in _playerManager.Players) {
                            player.PlayerInput.enabled = false;
                        }
        }
    }
}