using ToyBox.Managers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToyBox.Leaderboard
{
    public class LeaderBoardGraph : MonoBehaviour
    {
        #region References

        
        [Header("References")]
        [SerializeField] private Leaderboard _leaderBoard;
        [SerializeField] private PlayerManager _playerManager;
        
        #endregion

        #region UI Elements

        [FormerlySerializedAs("panelEndGameUI")]
        [Header("UI Elements")]
        [SerializeField] private GameObject _panelEndGameUI;
        [SerializeField] private GameObject _panelGraphUI;
        [SerializeField] private List<TextMeshProUGUI> _textName;
        [SerializeField] private List<TextMeshProUGUI> _textPoints;
        [SerializeField] private List<LineRenderer> _lineRenderers;

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

        private void Start()
        {
            _panelEndGameUI.SetActive(false);
            _panelGraphUI.SetActive(false);
            InitializeLineRenderers();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UpdateLeaderBoard();
            }
        }

        #endregion

        #region Initialization

        private void InitializeLineRenderers()
        {
            foreach (var lr in _lineRenderers)
            {
                lr.startWidth = _lineWidth;
                lr.endWidth = _lineWidth;
                lr.positionCount = 0;
            }
        }

        #endregion

        #region LeaderBoard Display

        public void UpdateLeaderBoard()
        {
            _leaderBoard.CheckPlayers();
            _panelEndGameUI.SetActive(true);
            _panelGraphUI.SetActive(true);
            var sortedPlayers = _leaderBoard.GetSortedPlayers();

            // Met à jour les textes (nom + score)
            for (int i = 0; i < _textPoints.Count && i < _textName.Count; i++)
            {
                Transform parent = _textPoints[i].transform.parent;
                parent.gameObject.SetActive(i < sortedPlayers.Count);

                if (i < sortedPlayers.Count)
                {
                    var (name, score) = sortedPlayers[i];
                    _textPoints[i].text = score.ToString();
                    _textName[i].text = name;
                }
            }

            // Détermine les valeurs max
            int maxScore = Mathf.Max(1, sortedPlayers.Max(p => p.Item2));
            int maxMatches = _playerManager.Players.Max(p => p.PlayerStats?.MatchScores.Count ?? 0);

            // Trace les courbes
            for (int i = 0; i < sortedPlayers.Count && i < _lineRenderers.Count; i++)
            {
                DrawPlayerLine(i, sortedPlayers[i].Item1, maxScore, maxMatches);
            }
        }

        #endregion

        #region Drawing

        private void DrawPlayerLine(int playerIndex, string playerName, int maxScore, int maxMatches)
        {
            var player = _playerManager.Players.FirstOrDefault(p => p.Name == playerName);
            if (player == null || string.IsNullOrEmpty(player.Name)) return;

            var stats = player.PlayerStats;
            if (stats == null || stats.MatchScores == null || stats.MatchScores.Count == 0) return;

            List<int> matchScores = new List<int>(stats.MatchScores);

            // Complète avec le dernier score si besoin
            while (matchScores.Count < maxMatches)
            {
                matchScores.Add(matchScores.Count > 0 ? matchScores.Last() : 0);
            }

            LineRenderer lr = _lineRenderers[playerIndex];
            lr.positionCount = matchScores.Count + 1; // +1 pour le point de départ

            // Couleur de la ligne
            Color playerColor = _playerColors[playerIndex % _playerColors.Count];
            lr.startColor = lr.endColor = playerColor;

            Vector3[] positions = new Vector3[matchScores.Count + 1];

            float availableWidth = _maxGraphWidth - 2 * _xMargin;
            float availableHeight = _maxGraphHeight - 2 * _yMargin;

            float xStep = matchScores.Count > 0 ? availableWidth / matchScores.Count : 0;
            float yScaleFactor = availableHeight / maxScore;

            // Point de départ
            positions[0] = _graphOrigin + new Vector3(_xMargin, _yMargin, 0);

            // Points des scores
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

        #endregion
    }
}
