using System;
using System.Linq;
using Toybox.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace ToyBox.Managers {
    public class GameModeManager : MonoBehaviour {
        public static GameModeManager Instance { get; private set; }

        public BuildsManager buildsManager;

        private PlayerManager _playerManager;
        MenuInputManager _menuInputManager => MenuInputManager.Instance;

        private PlayerInputManager _playerInputManager;

        public Action OnRaceStart;
        public Action OnRaceEnd;
        
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
            _playerInputManager = GetComponent<PlayerInputManager>();
        }
        
        public void StartRaceMode() {
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Race");
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = false;
            }
            OnRaceStart?.Invoke();
        }

        public void StartConstructMode() {
            buildsManager.Shuffle(0);
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = true;
            }
        }

        public void StartPause(StPlayer pausedPlayer) {
            Time.timeScale = 0f;
            _playerInputManager.enabled = false;
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.enabled = false;
            }
            _menuInputManager.MenuInput.enabled = true;
            _menuInputManager.MenuInput.user.UnpairDevices();
            InputUser.PerformPairingWithDevice(pausedPlayer.Device, _menuInputManager.MenuInput.user);
        }
        
        public void StartPause(GameObject pausedPlayer) {
            foreach (StPlayer player in _playerManager.Players.Where(_player => _player.PlayerObject == pausedPlayer)) {
                StartPause(player);
                break;
            }
        }

        public void EndPause() {
            _menuInputManager.MenuInput.enabled = false;
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.enabled = true;
            }
            _playerInputManager.enabled = true;
            Time.timeScale = 1f;
        }
    }
}