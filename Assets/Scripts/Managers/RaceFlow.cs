using System;
using System.Collections.Generic;
using ToyBox.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ToyBox.Managers {
    public class RaceFlow : MonoBehaviour
    {
        [SerializeField] Transform _startTransform;
        [SerializeField] Transform _winnersBox;

        GameModeManager _gameModeManager;
        PlayerManager _playerManager;
    
        int _finishedPlayers;
        bool _raceStarted;
        
        
        void Start() {
            _gameModeManager = GameModeManager.Instance;
            _playerManager = _gameModeManager.gameObject.GetComponent<PlayerManager>();
            _gameModeManager.OnRaceStart += RaceStart;
        }
    
        private void RaceStart()
        {
            foreach (StPlayer player in _playerManager.Players)
            {
                player.PlayerObject.transform.position = new Vector2(_startTransform.position.x+Random.Range(-2,2), _startTransform.position.y); //Randomizing the start position for now
                player.PlayerObject.GetComponent<PlayerMovement>().IsDead = false;
            }
            _raceStarted = true;
            _finishedPlayers = 0;
        }
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
            {
                _finishedPlayers++;
                
                playerMovement.gameObject.transform.position = _winnersBox.position;
            }
        }
    
        void Update() {
            if (!_raceStarted) return;
            if (GetAlivePlayers().Count == _finishedPlayers) {
                _gameModeManager.OnRaceEnd?.Invoke();
                Debug.Log("Race ends");
                _raceStarted = false;
            }
        }
    
        private List<StPlayer> GetAlivePlayers()
        {
            List<StPlayer> alivePlayers = new ();
            foreach (StPlayer player in _playerManager.Players)
            {
                if (player.PlayerObject.GetComponent<PlayerMovement>().IsDead == false)
                {
                    alivePlayers.Add(player);
                }
            }
            return alivePlayers;
        }
    }

}
