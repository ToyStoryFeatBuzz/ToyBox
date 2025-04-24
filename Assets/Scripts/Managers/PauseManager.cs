using System.Linq;
using ToyBox.InputSystem;
using ToyBox.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace ToyBox.Managers {
    public class PauseManager : MonoBehaviour {
       
        PlayerManager _playerManager => PlayerManager.Instance;
        
        public static PauseManager Instance { get; private set; }
        
        private PauseMenu _pauseMenu => PauseMenu.Instance;
        private MenuInputDelegate _menuInput => MenuInputDelegate.Instance;
        
        private Player _pausedBy;
        private InputActionMap _oldActionMap;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }

        void StartPause(Player pausedPlayer) {
            if (!_pauseMenu) return;
            Time.timeScale = 0f;
            if (pausedPlayer.Device == null) return;
            _pausedBy = pausedPlayer;

            _oldActionMap = _pausedBy.PlayerInput.currentActionMap;
            _pausedBy.PlayerInput.currentActionMap = _pausedBy.PlayerInput.actions.FindActionMap("Menu");
            _menuInput.PlayerInControl = _pausedBy.PlayerObject.GetComponent<MenuPlayerInputManager>();
            
            Cursor.lockState = CursorLockMode.Confined;
            _pauseMenu.TogglePause(true);
        }
        
        public void StartPause(GameObject pausedPlayer) {
            StartPause(_playerManager.GetPlayer(pausedPlayer));
        }

        public void EndPause() {
            _pauseMenu.TogglePause(false);
            Cursor.lockState = CursorLockMode.Locked;
            _menuInput.enabled = false;
            _pausedBy.PlayerInput.currentActionMap = _oldActionMap;
            Time.timeScale = 1f;
            _oldActionMap = null;
            _pausedBy = null;
            _menuInput.PlayerInControl = null;
        }

    }
}