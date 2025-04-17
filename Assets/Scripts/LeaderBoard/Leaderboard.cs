using System;
using System.Collections;
using ToyBox.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ToyBox.Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] float _timeToShow = 3f;
        private PlayerManager _playerManager => PlayerManager.Instance;
        private ScoreManager _scoreManager => ScoreManager.Instance;
        GameModeManager _gameModeManager => GameModeManager.Instance;
        private LeaderboardData _leaderboardData => LeaderboardData.Instance;
        
        
        
        private Dictionary<string, int> _playerScoreDict;

        private void Start()
        {
            _gameModeManager.OnRaceEnd += ShowLeaderboard;
            HideLeaderboard();
            _playerScoreDict = _scoreManager.PlayerScores;
            CheckPlayers();
        }

        public void ShowLeaderboard()
        {
            _playerManager.SetNewPlayersEntries(false);
            UpdateLeaderboard();
            _leaderboardData.PanelEndMatch.SetActive(true);
            StartCoroutine(ShowingLeaderboard());
        }
        
        IEnumerator ShowingLeaderboard()
        {
            yield return new WaitForSeconds(_timeToShow);
            HideLeaderboard();
            _gameModeManager.OnLeaderboardFinish.Invoke();
        }

        public void HideLeaderboard()
        {
            _leaderboardData.PanelEndMatch.SetActive(false);
        }

        public void UpdateLeaderboard()
        {
            CheckPlayers();

            List<(string name, int score)> sortedPlayers = GetSortedPlayers();

            for (int i = 0; i < _leaderboardData.PlayerInfos.Count; i++)
            {
                Transform parent = _leaderboardData.PlayerInfos[i].FillBar.transform.parent;

                if (i < sortedPlayers.Count)
                {
                    (string playerName, int score) = sortedPlayers[i];
                    Debug.Log($"AFTER SORTING Name: {playerName}, Score: {score}");

                    parent.gameObject.SetActive(true);
                    _leaderboardData.PlayerInfos[i].FillBar.fillAmount = score / 10f;
                    _leaderboardData.PlayerInfos[i].TextSlot.text = playerName;
                }
                else
                {
                    parent.gameObject.SetActive(false);
                }
            }
        }

        public void CheckPlayers()
        {
            _scoreManager.PlayerScores = _playerScoreDict;
        }

        public List<(string name, int score)> GetSortedPlayers()
        {
            List<(string name, int score)> playerList = new List<(string name, int score)>();

            foreach (Managers.Player player in _playerManager.Players)
            {
                if (_playerScoreDict.ContainsKey(player.Name))
                {
                    playerList.Add((player.Name, _playerScoreDict[player.Name]));
                }
            }

            return playerList.OrderByDescending(x => x.score).ToList();
        }
    }
}
