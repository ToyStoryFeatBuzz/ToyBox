using ToyBox.Managers;
using UnityEngine;
using static ToyBox.Enums;
using UnityEngine.InputSystem;

namespace ToyBox.Player {
    public class PlayerEnd : MonoBehaviour {
        public bool IsDead;
        private ScoreManager _scoreManager;
        private PlayerStats _playerStats;
        PlayerManager _playerManager => PlayerManager.Instance;

        private void Start()
        {
            _playerStats = GetComponent<PlayerStats>();
            _scoreManager = ScoreManager.Instance;
        }

        public void SetDeath() {
            Debug.Log(gameObject.name + " is dead (SetDeath called)");
            _playerManager.SetPlayerState(gameObject, EPlayerState.Dead);
            IsDead = true;
            gameObject.transform.position = new Vector2(-999, -999);
            _scoreManager.AddDeathScore(_playerStats.Name, _playerStats);
        }

        public void SetWin() {
            _playerManager.SetPlayerState(gameObject, EPlayerState.Finished);
            print(_scoreManager==null);
            _scoreManager.AddScore(_playerStats.Name, _playerStats);
        }
    }
}