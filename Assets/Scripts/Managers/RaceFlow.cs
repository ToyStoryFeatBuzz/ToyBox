using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.InputSystem;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using static ToyBox.Enums;
using Random = UnityEngine.Random;

namespace ToyBox.Managers {
    public class RaceFlow : MonoBehaviour
    {
        [SerializeField] Transform _startTransform;
        [SerializeField] Transform _winnersBox;

        GameModeManager _gameModeManager => GameModeManager.Instance;
        MapPath _mapPath => MapPath.Instance;
        PlayerManager _playerManager;
    
        int _finishedPlayers;
        bool _raceStarted;

        public List<(string player, Transform t, float score)> playersOrder = new();
        
        void Start() {
            _playerManager = _gameModeManager.gameObject.GetComponent<PlayerManager>();
           // _leaderboard = _gameModeManager.transform.parent.GetComponentInChildren<Leaderboard.Leaderboard>();
            _gameModeManager.OnRaceStart += RaceStart;

            foreach (Player player in _playerManager.Players)
            {
                playersOrder.Add((player.Name, player.PlayerObject.transform, 0f));
                player.PlayerObject.GetComponent<PlayerInput>().DeactivateInput();
            }
        }

        public List<(string player, Transform t, float score)> GetPlayersInOrder()
        {
            return playersOrder.OrderBy(p => p.score).ToList();
        }

        private void RaceStart() {
            foreach (Player player in _playerManager.Players) {
                player.PlayerObject.transform.position = new Vector2(_startTransform.position.x+Random.Range(-2,2), _startTransform.position.y); //Randomizing the start position for now
                player.PlayerObject.GetComponent<PlayerInput>().ActivateInput();
                player.PlayerState = EPlayerState.Alive;
            }
            _raceStarted = true;
            _finishedPlayers = 0;
        }
    
        private void OnTriggerEnter2D(Collider2D collision) {
            if (!collision.gameObject.TryGetComponent(out PlayerEnd player)) {
                return;
            }
            _finishedPlayers++;
            player.gameObject.transform.position = _winnersBox.position;
            player.SetWin();
        }
    
        void Update() {
            if (!_raceStarted) return;
            if (_playerManager.GetAlivePlayers().Count == 0) {
                _gameModeManager.OnRaceEnd?.Invoke();
                _raceStarted = false;
            }

            for (int i = 0; i < playersOrder.Count; i++)
            {
                var p = playersOrder[i];

                p.score = _mapPath.GetPlayerAdvancement(p.t);

                playersOrder[i] = p;
            }

        }
    }

}
