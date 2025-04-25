using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ToyBox.Build;
using ToyBox.LevelDesign;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using static ToyBox.Enums;

namespace ToyBox.Managers {
    public class RaceFlow : MonoBehaviour
    {
        [SerializeField] Transform _startTransform;
        [SerializeField] Transform _winnersBox;
        ColliderDetector _endCollider;
        [SerializeField] MapSpawnPos _spawnPos;
        
        [SerializeField] TextMeshProUGUI _roundsText;

        GameModeManager _gameModeManager => GameModeManager.Instance;
        MapPath _mapPath => MapPath.Instance;
        PlayerManager _playerManager;
    
        int _finishedPlayers;
        bool _raceStarted;

        public List<(string player, Transform t, float score)> playersOrder = new();
        
        void Start() {
            _playerManager = _gameModeManager.gameObject.GetComponent<PlayerManager>();
           // _leaderboard = _gameModeManager.transform.parent.GetComponentInChildren<Leaderboard.Leaderboard>();
            _gameModeManager.OnRaceStartExtern += RaceStart;
            _gameModeManager.OnPreStart += RaceStart;
            _endCollider=transform.GetComponentInChildren<ColliderDetector>();
            _endCollider._onTriggerEnterFunction = OnTriggerEndFunction;
            foreach (Player player in _playerManager.Players)
            {
                
                playersOrder.Add((player.Name, player.PlayerObject.transform, 0f));
                player.PlayerObject.GetComponent<PlayerInput>().DeactivateInput();
            }
        }
        

        public void SetCamCenterMovements(Action<float> movementX, Action<float> movementY)
        {
            foreach (Player player in _playerManager.Players)
            {
                var mouse = player.PlayerObject.GetComponent<PlayerMouse>();
                mouse.OnMouseInBorderXEvent += movementX;
                mouse.OnMouseInBorderYEvent += movementY;
            }
        }

        public List<(string player, Transform t, float score)> GetPlayersInOrder()
        {
            return playersOrder.OrderBy(p => p.score).ToList();
        }

        private void RaceStart() {
            _roundsText.text ="Round "+ _gameModeManager.NbRounds;
            foreach (Player player in _playerManager.Players) 
            { 
                _spawnPos.SetPlayersPos();
                player.PlayerObject.GetComponent<PlayerInput>().ActivateInput();
                player.PlayerState = EPlayerState.Alive;
            }
            _raceStarted = true;
            _finishedPlayers = 0;
        }

        private void OnTriggerEndFunction(Collider2D collision)
        {
            if (!collision.gameObject.TryGetComponent(out PlayerEnd player)) {
                return;
            }
            _finishedPlayers++;
            player.gameObject.transform.position = _winnersBox.position;
            player.SetWin();
            AudioManager.Instance.PlaySFX("RaceEnd_Crowd",volume:0.5f);
        }
    
        void Update() {
            if (!_raceStarted) return;
            if (_playerManager.GetAlivePlayers().Count == 0) {
                _gameModeManager.OnRaceEndIntern?.Invoke();
                _gameModeManager.OnRaceEndExtern?.Invoke();
                AudioManager.Instance.StopMusic();
                AudioManager.Instance.PlaySFX("RaceEnd_Horn", volume:0.7f);
                _raceStarted = false;
            }

            for (int i = 0; i < playersOrder.Count; i++)
            {
                (string player, Transform t, float score) p = playersOrder[i];

                p.score = _mapPath.GetPlayerAdvancement(p.t);

                playersOrder[i] = p;
            }

        }
    }

}
