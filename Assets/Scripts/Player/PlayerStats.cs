using System;
using System.Collections.Generic;
using ToyBox.Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace ToyBox.Player {
    public class PlayerStats : MonoBehaviour
    {
        [field:SerializeField] public int Score {get;  private set; }
        
        public List<int> MatchScores = new();
        public string Name;
        
        private void Start() {
            Name = gameObject.name;
        }
        
        public void AddScore(int amount) {
            Score += amount;
            AddScoreToList(Score);
            Debug.Log(Name + " added score: " + Score);
            AddToScoreManager();
        }
        
        public void RemoveScore(int amount) {
            Score -= amount;
            AddScoreToList(Score);
        }
        
        public void ResetScore() {
            Score = 0;
            MatchScores.Clear();
            ResetScoreManager();
        }
        
        public void AddScoreToList(int amount) {
            MatchScores.Add(amount);
        }
        
        public void AddToScoreManager() {
            ScoreManager.Instance.AddScoreDic(Name, Score);
        }

        public void ResetScoreManager()
        {
            ScoreManager.Instance.RestScoreDic();
        }
    }
}

