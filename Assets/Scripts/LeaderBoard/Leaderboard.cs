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
        [SerializeField] private float _maxScore;
        private PlayerManager _playerManager => PlayerManager.Instance;
        private ScoreManager _scoreManager => ScoreManager.Instance;
        GameModeManager _gameModeManager => GameModeManager.Instance;
        private LeaderboardData _leaderboardData => LeaderboardData.Instance;
        
        private Dictionary<string, (int score, Color color, Sprite sprite)> _playerScoreDict;

        private void Start()
        {
            _maxScore = _gameModeManager.GetPointToWin();
            _gameModeManager.OnLeaderboardStart += ShowLeaderboard;
            HideLeaderboard();
            _playerScoreDict = _scoreManager.PlayerScores;
            CheckPlayers();
        }

        public void ShowLeaderboard()
        {
            print("DDDDDDDDDDDDDDDDDDDD");
            _playerManager.SetNewPlayersEntries(false);
            UpdateLeaderboard();
            _leaderboardData.PanelEndMatch.SetActive(true);
            StartCoroutine(ShowingLeaderboard());
        }
        
        IEnumerator ShowingLeaderboard()
        {
            print("EEEEEEEEEEEEEEEEEEEEEEEEE");
            yield return new WaitForSeconds(_timeToShow);
            HideLeaderboard();
            _gameModeManager.OnLeaderboardFinish.Invoke();
        }

        public void HideLeaderboard()
        {
            _leaderboardData.PanelEndMatch.SetActive(false);
            _scoreManager.ResetRound();
        }

        public void UpdateLeaderboard()
        {
            CheckPlayers();

            List<(string name, int score, Color c, Sprite sprite)> sortedPlayers = GetSortedPlayers();

            for (int i = 0; i < _leaderboardData.PlayerInfos.Count; i++)
            {
                Transform parent = _leaderboardData.PlayerInfos[i].FillBar.transform.parent;

                if (i < sortedPlayers.Count)
                {
                    (string playerName, int score, Color c, Sprite sprite) = sortedPlayers[i];

                    parent.gameObject.SetActive(true);
                    _leaderboardData.PlayerInfos[i].FillBar.color = c;
                    _leaderboardData.PlayerInfos[i].FillBar.fillAmount = score / _maxScore;
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
            _playerScoreDict = _scoreManager.PlayerScores;
        }

        public List<(string name, int score, Color c, Sprite sprite)> GetSortedPlayers()
        {
            List<(string name, int score, Color c, Sprite sprite)> playerList = new();

            foreach (Managers.Player player in _playerManager.Players)
            {
                if (_playerScoreDict.ContainsKey(player.Name))
                {
                    playerList.Add((player.Name, _playerScoreDict[player.Name].score, _playerScoreDict[player.Name].color, _playerScoreDict[player.Name].sprite));
                }
            }

            return playerList.OrderByDescending(x => x.score).ToList();
        }
    }
}
