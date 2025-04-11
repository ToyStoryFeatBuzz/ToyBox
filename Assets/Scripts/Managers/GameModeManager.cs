using System;
using UnityEngine;

namespace ToyBox.Managers {
    public class GameModeManager : MonoBehaviour {
        public static GameModeManager Instance { get; private set; }

        public BuildsManager buildsManager;

        private PlayerManager _playerManager => PlayerManager.Instance;

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
            StartRaceMode();
            OnRaceEnd += StartConstructMode;
        }
        
        public void StartRaceMode() {
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Race");
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = false;
            }
            OnRaceStart?.Invoke();
        }

        public void StartConstructMode() {
            buildsManager.Shuffle(0);
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = true;
            }
        }
    }
}