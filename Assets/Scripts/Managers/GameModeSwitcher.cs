using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Managers {
    public class GameModeSwitcher : MonoBehaviour {
        public static GameModeSwitcher Instance { get; private set; }

        private PlayerManager _playerManager;
        
        // Race flow events
        public UnityEvent RaceStart;
        public UnityEvent RaceEnd;
        
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
            RaceStart.Invoke();
        }

        public void StartConstructMode() {
            foreach (StPlayer player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
            }
        }
    }
}