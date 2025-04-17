using System;
using ToyBox.Managers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToyBox.Leaderboard
{
    public class LeaderboardGraph : MonoBehaviour
    {
        #region References
        [Header("References")]
        Leaderboard _leaderBoard;
        PlayerManager _playerManager => PlayerManager.Instance;
        private LeaderboardData _leaderboardData => LeaderboardData.Instance;
        private GameModeManager _gameModeManager => GameModeManager.Instance;
        #endregion

        #region Graph Settings
        [Header("Graph Settings")]
        [SerializeField] private List<Color> _playerColors = new List<Color>
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            new Color(1f, 0f, 1f),        // rose
            new Color(0.5f, 0f, 0.5f),    // violet
            new Color(1f, 0.5f, 0f)       // orange
        };
        #endregion

        #region Positioning
        [Header("Positioning")]
        [SerializeField] private Vector3 _graphOrigin = new Vector3(-5f, -2f, 0f);
        [SerializeField] private float _lineWidth = 0.15f;
        #endregion

        #region Graph Limits
        [Header("Graph Limits")]
        [SerializeField] private float _maxGraphWidth = 10f;
        [SerializeField] private float _maxGraphHeight = 5f;
        [SerializeField] private float _xMargin = 1f;
        [SerializeField] private float _yMargin = 0.5f;
        #endregion

        #region Unity Methods

        private void Start() {
            _gameModeManager.OnLeaderboardGraphStart += UpdateLeaderBoard;
            _leaderBoard = GetComponent<Leaderboard>();
            _leaderboardData.PanelEndGame.SetActive(false);
            _leaderboardData.Graph.SetActive(false);
            _leaderboardData.Background.SetActive(false);
            InitializeLineRenderers();
        }

        #endregion

        #region Initialization

        private void InitializeLineRenderers()
        {
            foreach (PlayerInfo playerInfo in _leaderboardData.PlayerInfos)
            {
                playerInfo.LineRenderer.startWidth = _lineWidth;
                playerInfo.LineRenderer.endWidth = _lineWidth;
                playerInfo.LineRenderer.positionCount = 0;
            }
        }

        #endregion

        #region LeaderBoard Display
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                UpdateLeaderBoard();
            }
        }

        public void UpdateLeaderBoard()
        {
            _leaderBoard.CheckPlayers();
            _leaderboardData.PanelEndGame.SetActive(true);
            _leaderboardData.Graph.SetActive(true);
            _leaderboardData.Background.SetActive(true);
            List<(string name, int score)> sortedPlayers = _leaderBoard.GetSortedPlayers();

            for (int i = 0; i < _leaderboardData.PlayerInfos.Count; i++)
            {
                Transform parent = _leaderboardData.PlayerInfos[i].TextPoints.transform.parent;
                parent.gameObject.SetActive(i < sortedPlayers.Count);

                if (i >= sortedPlayers.Count) {
                    continue;
                }
                (string name, int score) = sortedPlayers[i];
                _leaderboardData.PlayerInfos[i].TextPoints.text = score.ToString();
                _leaderboardData.PlayerInfos[i].TextName.text = name;
            }

            int maxScore = Mathf.Max(1, sortedPlayers.Max(p => p.Item2));
            int maxMatches = _playerManager.Players.Max(p => p.PlayerStats?.MatchScores.Count ?? 0);

            for (int i = 0; i < sortedPlayers.Count && i < _leaderboardData.PlayerInfos.Count; i++)
            {
                DrawPlayerLine(i, sortedPlayers[i].Item1, maxScore, maxMatches);
            }
        }

        #endregion

        #region Drawing

        private void DrawPlayerLine(int playerIndex, string playerName, int maxScore, int maxMatches)
        {
            Managers.Player player = _playerManager.Players.FirstOrDefault(p => p.Name == playerName);
            if (player == null || string.IsNullOrEmpty(player.Name)) return;

            PlayerStats stats = player.PlayerStats;
            if (stats == null || stats.MatchScores == null || stats.MatchScores.Count == 0) return;

            List<int> matchScores = GetPaddedScores(stats.MatchScores, maxMatches);

            LineRenderer lr = _leaderboardData.PlayerInfos[playerIndex].LineRenderer;
            lr.positionCount = matchScores.Count + 1;

            Color playerColor = _playerColors[playerIndex % _playerColors.Count];
            lr.startColor = lr.endColor = playerColor;

            Vector3[] positions = new Vector3[matchScores.Count + 1];

            float availableWidth = _maxGraphWidth - 2 * _xMargin;
            float availableHeight = _maxGraphHeight - 2 * _yMargin;

            float xStep = matchScores.Count > 0 ? availableWidth / matchScores.Count : 0;
            float yScaleFactor = maxScore > 0 ? availableHeight / maxScore : 0;

            positions[0] = _graphOrigin + new Vector3(_xMargin, _yMargin, 0);

            int cumulativeScore = 0;
            for (int i = 0; i < matchScores.Count; i++)
            {
                cumulativeScore += matchScores[i];
                float xPos = _xMargin + ((i + 1) * xStep);
                float yPos = _yMargin + (cumulativeScore * yScaleFactor);

                xPos = Mathf.Clamp(xPos, _xMargin, _maxGraphWidth - _xMargin);
                yPos = Mathf.Clamp(yPos, _yMargin, _maxGraphHeight - _yMargin);

                positions[i + 1] = _graphOrigin + new Vector3(xPos, yPos, 0);
            }

            lr.SetPositions(positions);
        }

        private List<int> GetPaddedScores(List<int> original, int targetLength)
        {
            List<int> padded = new List<int>(original);
            int lastScore = original.Count > 0 ? original.Last() : 0;
            while (padded.Count < targetLength)
                padded.Add(lastScore);
            return padded;
        }

        #endregion
    }
}
