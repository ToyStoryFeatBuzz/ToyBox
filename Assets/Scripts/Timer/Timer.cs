using System.Linq;
using TMPro;
using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Timer
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timerText;
        GameModeManager _gameModeManager => GameModeManager.Instance; 
        PlayerManager _playerManager => PlayerManager.Instance;
        [SerializeField] private float _maxTime;

        [SerializeField] float _remainTime;
        bool _isRaceStarted;

        void Start() {
            _remainTime = _maxTime;
            _timerText.text = _remainTime.ToString("00:00<style=\"Smaller\">.00</style>");
        }

        public void StartTimer() {
            _remainTime = _maxTime;
            _isRaceStarted = true;
        }

        public void StopTimer() {
            _isRaceStarted = false;
        }

        void Update() {
            if (!_isRaceStarted) return;
            if (_remainTime > 0f) {
                _remainTime -= Time.deltaTime;
            }
            else {
                KillAllPlayer();
                _remainTime = 0f;
            }
            _timerText.text = _remainTime.ToString("00:00<style=\"Smaller\">.00</style>");
        }

        void KillAllPlayer() {
            
            foreach (Managers.Player player in _playerManager.Players.Where(player => player.PlayerState == Enums.EPlayerState.Alive)) {
                player.PlayerObject.GetComponent<PlayerEnd>().SetDeath();
            }
        }


    }
}