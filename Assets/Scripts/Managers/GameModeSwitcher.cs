using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {
    public class GameModeSwitcher : MonoBehaviour {
        public static GameModeSwitcher Instance { get; private set; }

        private PlayerManager _playerManager;
        
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
            foreach (PlayerInput player in _playerManager.Players) {
                player.currentActionMap = player.actions.FindActionMap("Race");
            }
        }

        public void StartConstructMode() {
            foreach (PlayerInput player in _playerManager.Players) {
                player.currentActionMap = player.actions.FindActionMap("Construct");
            }
        }
    }
}