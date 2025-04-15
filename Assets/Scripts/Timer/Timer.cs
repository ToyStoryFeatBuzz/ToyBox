using System.Collections.Generic;
using TMPro;
using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Timer {
    public class Timer : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _timerText;
        GameModeManager _gameModeManager;
        
        [Header("Player Manager")]
        [SerializeField] PlayerManager _playerManager;
        private List<Managers.Player> _listPlayers;
        
        float _time;
        bool _running;

        void Start()
        {
            _gameModeManager = GameModeManager.Instance;
            if (!PlayerPrefs.HasKey("BestTime"))
            {
                PlayerPrefs.SetFloat("BestTime", float.MaxValue);
            }

            _gameModeManager.OnRaceStart += StartTimer;
            _gameModeManager.OnRaceEnd += StopTimer;
        }

        void StartTimer()
        {
            _time = 0.0f;
            _running = true;
        }

        void StopTimer()
        {
            _running = false;
            
            if (PlayerPrefs.GetFloat("BestTime") > _time)
            {
                PlayerPrefs.SetFloat("BestTime", _time);
            }
            _time = 0.0f;
        }

        void Update()
        {
            if (_running)
            {
                _time += Time.deltaTime;
                _timerText.text = _time.ToString("00:00<style=\"Smaller\">.00</style>");
            } else
            {
                KillPlayers();
            }
        }
        
        void KillPlayers()
        {
            _listPlayers = _playerManager.GetAlivePlayers();
            
            foreach (Managers.Player player in _listPlayers)
            {
                player.PlayerObject.GetComponent<PlayerEnd>().SetDeath();
            }
        }
    }
}