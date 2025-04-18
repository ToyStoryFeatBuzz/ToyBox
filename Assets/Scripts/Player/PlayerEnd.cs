using System;
using ToyBox.Managers;
using UnityEngine;
using static ToyBox.Enums;

namespace ToyBox.Player {
    public class PlayerEnd : MonoBehaviour {
        public bool IsDead;
        private PlayerStats _playerStats;
        PlayerManager _playerManager => PlayerManager.Instance;

        private void Start()
        {
            _playerStats = GetComponent<PlayerStats>();
        }

        public void SetDeath() {
            _playerManager.SetPlayerState(gameObject, EPlayerState.Dead);
            IsDead = true;
            gameObject.transform.position = new Vector2(-999, -999); //Send the dead out of the map to avoid clutter on the race
            _playerStats.AddScore(0);
        }

        public void SetWin() {
            _playerManager.SetPlayerState(gameObject, EPlayerState.Finished);
            _playerStats.AddScore(5);
        }
    }
}