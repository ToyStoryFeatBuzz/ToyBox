using System.Collections.Generic;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Managers {
    public class ScoreManager : MonoBehaviour
    { 
        [SerializeField] private int _pointsPerDeath = 1;
        [SerializeField] private int[] _scoreList = { 10, 8, 6, 4, 2, 1 };
        private PlayerManager _playerManager => PlayerManager.Instance;
        private List<Player> _players => _playerManager.Players; // List of all players
        public static ScoreManager Instance;
        
        public Dictionary<string, (int score, Color color)> PlayerScores = new();

        private List<string> _arrivalOrder = new(); // Liste d'arrivée des joueurs

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }

        public void AddScoreDic(string playerName, int score, Color c)
        {
            if (PlayerScores.ContainsKey(playerName))
            {
                PlayerScores[playerName] = (score, c);
                Debug.Log("Mise à jour du score dans le dictionnaire : " + playerName + " => " + score);
            }
            else
            {
                PlayerScores.Add(playerName, (score, c));
                Debug.Log("Ajout score dans dictionnaire : " + playerName + " => " + score);
            }
            Debug.Log("Ajout score dans dictionnaire : " + playerName + " => " + score);
        }

        public void AddScore(string playerName, PlayerStats playerStats)
        {
            if (_arrivalOrder.Contains(playerName))
                return;

            _arrivalOrder.Add(playerName);

            int position = _arrivalOrder.Count - 1;
            int scoreToAdd = (position < _scoreList.Length) ? _scoreList[position] : 0;

            int diff = playerStats.Score + scoreToAdd - GameModeManager.PointToWin;
            
            if(diff >0) scoreToAdd -= diff;

            playerStats.AddScore(scoreToAdd); // Mise à jour locale
            AddScoreDic(playerName, playerStats.Score, playerStats.color); // Mise à jour dans le dico
        }

        public void AddDeathScore(string playerName, PlayerStats playerStats)
        {
            playerStats.AddScore(_pointsPerDeath);
            AddScoreDic(playerName, playerStats.Score, playerStats.color);
        }

        
        public void RemoveScoreDic(string playerName)
        {
            PlayerScores.Remove(playerName);
        }

        public void RestScoreDic()
        {
            PlayerScores.Clear();
        }

        public void ResetRound()
        {
            _arrivalOrder.Clear();
        }
    }
}
