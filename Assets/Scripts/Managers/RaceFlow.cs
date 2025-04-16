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
        
        
        void Start() {
            _playerManager = _gameModeManager.gameObject.GetComponent<PlayerManager>();
            _gameModeManager.OnRaceStart += RaceStart;
        }
    
        private void RaceStart() {
            foreach (Player player in _playerManager.Players) {
                player.PlayerObject.transform.position = new Vector2(_startTransform.position.x+Random.Range(-2,2), _startTransform.position.y); //Randomizing the start position for now
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

            List<(string player, Transform t, float score)> pls = new();

            foreach (Player player in _playerManager.Players)
            {
                pls.Add((player.Name, player.PlayerObject.transform, MapPath.Instance.GetPlayerAdvancement(player.PlayerObject.transform)));
            }

            string a = "RANK  ===  ";

            foreach (var p in pls)
            {
                a += p.player + " : " + p.score + " ||| ";
            }

            print(a);
        }
    }

}
