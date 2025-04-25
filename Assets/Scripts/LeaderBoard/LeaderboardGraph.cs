using ToyBox.Managers;
using System.Collections.Generic;
using System.Linq;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Leaderboard
{
    public class LeaderboardGraph : MonoBehaviour
    {
        #region References
        [Header("References")]
        Leaderboard _leaderBoard;
        
        [SerializeField] private LineRenderer _lineBorderX;
        [SerializeField] private LineRenderer _lineBorderY;
        [SerializeField] private GameObject _graphLine;
        PlayerManager _playerManager => PlayerManager.Instance;
        private LeaderboardData _leaderboardData => LeaderboardData.Instance;
        private GameModeManager _gameModeManager => GameModeManager.Instance;
        #endregion

        #region Positioning
        [Header("Positioning")]
        [SerializeField] private Transform _graphOrigin;
        [SerializeField] private Transform _graphEnd;
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

            _maxGraphHeight = _graphEnd.position.y - _graphOrigin.position.y;
            _maxGraphWidth = _graphEnd.position.x - _graphOrigin.position.x;

            _gameModeManager.OnLeaderboardGraphStartIntern += UpdateLeaderBoard;
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

        public void UpdateLeaderBoard()
        {
            _leaderBoard.ResetLeaderBoard();
            _leaderBoard.CheckPlayers();
            _leaderboardData.PanelEndGame.SetActive(true);
            _leaderboardData.Graph.SetActive(true);
            _leaderboardData.Background.SetActive(true);
            
            List<(string name, int score, Color c, Sprite sprite)> sortedPlayers = _leaderBoard.GetSortedPlayers();

            for (int i = 0; i < _leaderboardData.PlayerInfos.Count; i++)
            {
                Transform parent = _leaderboardData.PlayerInfos[i].TextPoints.transform.parent;
                parent.gameObject.SetActive(i < sortedPlayers.Count);

                if (i >= sortedPlayers.Count) {
                    continue;
                }
                (string name, int score, Color c, Sprite sprite) = sortedPlayers[i];
                _leaderboardData.PlayerInfos[i].ImagePlayer.sprite = sprite;
                _leaderboardData.PlayerInfos[i].TextPoints.text = score.ToString();
                _leaderboardData.PlayerInfos[i].TextName.text = name;
            }

            int maxScore = Mathf.Max(1, sortedPlayers.Max(p => p.score));
            
            maxScore = Mathf.Clamp(maxScore, 0, _gameModeManager.GetPointToWin());
            
            int maxMatches = _playerManager.Players.Max(p => p.PlayerStats?.MatchScores.Count ?? 0);

            for (int i = 0; i < sortedPlayers.Count && i < _leaderboardData.PlayerInfos.Count; i++)
            {
                DrawPlayerLine(i, sortedPlayers[i].name, maxScore, maxMatches, sortedPlayers[i].c);
            }
            
            DrawnBorderLines(maxScore, maxMatches);
        }

        #endregion

        #region Drawing

        private void DrawPlayerLine(int playerIndex, string playerName, int maxScore, int maxMatches, Color c)
        {
            Managers.Player player = _playerManager.Players.FirstOrDefault(p => p.Name == playerName);
            if (player == null || string.IsNullOrEmpty(player.Name)) return;

            PlayerStats stats = player.PlayerStats;
            if (stats == null || stats.MatchScores == null || stats.MatchScores.Count == 0) return;

            List<int> matchScores = stats.MatchScores;
            
            matchScores[matchScores.Count - 1] = Mathf.Min(matchScores[matchScores.Count - 1], maxScore);

            LineRenderer lr = _leaderboardData.PlayerInfos[playerIndex].LineRenderer;
            lr.positionCount = matchScores.Count + 1;

            Color playerColor = c;
            lr.startColor = lr.endColor = playerColor;

            Vector3[] positions = new Vector3[matchScores.Count + 1];

            float availableWidth = _maxGraphWidth - 2 * _xMargin;
            float availableHeight = _maxGraphHeight - 2 * _yMargin;

            float xStep = maxMatches > 0 ? availableWidth / maxMatches : 0;
            float yScaleFactor = maxScore > 0 ? availableHeight / maxScore : 0;

            positions[0] = _graphOrigin.position + new Vector3(_xMargin, _yMargin, 0);
            
            for (int i = 0; i < matchScores.Count; i++)
            {
                float xPos = _xMargin + ((i + 1) * xStep);
                float yPos = _yMargin + (matchScores[i] * yScaleFactor);

                xPos = Mathf.Clamp(xPos, _xMargin, _maxGraphWidth - _xMargin);
                yPos = Mathf.Clamp(yPos, _yMargin, _maxGraphHeight - _yMargin);

                positions[i + 1] = _graphOrigin.position + new Vector3(xPos, yPos, 0);
            }

            lr.SetPositions(positions);
        }

        private void DrawnBorderLines(int maxScore, int maxMatches)
        {
            _lineBorderX.startWidth = _lineWidth;
            _lineBorderY.startWidth = _lineWidth;
            
            Vector3[] borderX = new Vector3[2];
            Vector3[] borderY = new Vector3[2];

            float availableWidth = _maxGraphWidth - 2 * _xMargin;
            float availableHeight = _maxGraphHeight - 2 * _yMargin;

            float xStep = maxMatches > 0 ? availableWidth / maxMatches : 0;
            float yScaleFactor = maxScore > 0 ? availableHeight / maxScore : 0;

            borderX[0] = _graphOrigin.position + new Vector3(_xMargin, _yMargin, 0);
            borderX[1] = _graphOrigin.position + new Vector3(_xMargin + availableWidth, _yMargin, 0);

            borderY[0] = _graphOrigin.position + new Vector3(_xMargin, _yMargin, 0);
            borderY[1] = _graphOrigin.position + new Vector3(_xMargin, _yMargin + availableHeight, 0);
            
            Vector3 xLongGraph = borderX[1] - borderX[0];
            Vector3 yLongGraph = borderY[1] - borderY[0];
            
            Vector3 xStepGraph = xLongGraph / maxMatches;
            Vector3 yStepGraph = yLongGraph / maxScore;


            for (int i = 0; i < maxMatches + 1; i++)
            {
                GameObject graphElement = Instantiate(_graphLine, (Vector3)(_graphOrigin.position + new Vector3(_xMargin, _yMargin, 0) + i * xStepGraph), Quaternion.identity);
                graphElement.GetComponent<GraphElement>().Init(i, new Vector2(14f, -20f), new(0, 1));
            }
            
            for (int i = 0; i < (int)(maxScore /10) + 1; i++)
            {
                GameObject graphElement = Instantiate(_graphLine, (Vector3)(_graphOrigin.position + new Vector3(_xMargin, _yMargin, 0) + i * 10 * yStepGraph), Quaternion.identity);
                graphElement.GetComponent<GraphElement>().Init(i * 10, new Vector2(-28f, 14f), new(1, 0));
            }
            

            
            _lineBorderX.SetPositions(borderX);
            _lineBorderY.SetPositions(borderY);
        }

        #endregion

        public void HideLeaderboard()
        {
            _leaderboardData.PanelEndGame.SetActive(false);
            _leaderboardData.Graph.SetActive(false);
            _leaderboardData.Background.SetActive(false);
            foreach (PlayerInfo playerInfo in _leaderboardData.PlayerInfos)
            {
                playerInfo.LineRenderer.positionCount = 0;
                playerInfo.TextPoints.text = "";
                playerInfo.TextName.text = "";
            }
        }
    }
}
