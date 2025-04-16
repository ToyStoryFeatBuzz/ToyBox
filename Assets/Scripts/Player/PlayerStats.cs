using System;
using System.Collections.Generic;
using ToyBox.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox.Player {
    public class PlayerStats : MonoBehaviour
    {
        public int Score {get;  private set; }
        
        public List<int> MatchScores = new();
        public string Name;
        private ScoreManager _scoreManager => ScoreManager.Instance;



        public void AddScore(int amount) {
            Score += amount;
            AddScoreToList(Score);
            _scoreManager.AddScoreDic(Name,Score);
        }
        
        public void RemoveScore(int amount) {
            Score -= amount;
            AddScoreToList(Score);
            _scoreManager.AddScoreDic(Name,Score);
        }
        
        public void ResetScore() {
            Score = 0;
            MatchScores.Clear();
            _scoreManager.RestScoreDic();
        }
        
        public void AddScoreToList(int amount) {
            MatchScores.Add(amount);
        }
    }
}

