using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.InputSystem;
using ToyBox.Player;
using UnityEngine;
using static ToyBox.Enums;
using Random = UnityEngine.Random;

namespace ToyBox.Managers {
    public class RaceFlow : MonoBehaviour
    {
        [SerializeField] Transform _startTransform;
        [SerializeField] Transform _winnersBox;

        GameModeManager _gameModeManager => GameModeManager.Instance;
        PlayerManager _playerManager;
    
        int _finishedPlayers;
        bool _raceStarted;

        public static RaceFlow Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }


        void Start() {
            _playerManager = _gameModeManager.gameObject.GetComponent<PlayerManager>();
            print("&&&&" + (_gameModeManager != null));
            _gameModeManager.OnRaceStart += RaceStart;

            foreach (Player player in _playerManager.Players)
            {
                player.PlayerObject.transform.position = new Vector2(_startTransform.position.x + Random.Range(-2, 2), _startTransform.position.y); //Randomizing the start position for now
            }
        }
    
        private void RaceStart() {
            foreach (Player player in _playerManager.Players) {
                player.PlayerObject.transform.position = new Vector2(_startTransform.position.x+Random.Range(-2,2), _startTransform.position.y); //Randomizing the start position for now
                player.PlayerState = EPlayerState.Alive;
            }
            _raceStarted = true;
            _finishedPlayers = 0;
        }
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerInputSystem player)) {
                _finishedPlayers++;
                player.gameObject.transform.position = _winnersBox.position;
                player.SetWin();
            }
        }
    
        void Update() {
            print(_raceStarted);
            if (!_raceStarted) return;
            if (_playerManager.GetAlivePlayers().Count == 0) {
                print("a");
                _gameModeManager.OnRaceEnd?.Invoke();
                _raceStarted = false;
            }
        }
    }

}
