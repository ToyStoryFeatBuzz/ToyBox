using System.Collections.Generic;
using ToyBox.Managers;
using UnityEngine;

namespace ToyBox.Player {
    public class PlayerStats : MonoBehaviour
    {
        [field:SerializeField] public int Score {get;  private set; }
        
        public List<int> MatchScores = new();
        public string Name;
        public Color color;
        public Sprite sprite;
        
        private void Start() {
            Name = gameObject.name;
        }
        
        public void AddScore(int amount) {
            Score += amount;
            AddScoreToList(Score);
        }
        
        public void RemoveScore(int amount) {
            Score -= amount;
            AddScoreToList(Score);
        }
        
        public void ResetScore() {
            Score = 0;
            MatchScores.Clear();
        }
        
        public void AddScoreToList(int amount) {
            MatchScores.Add(amount);
        }
        
        public void SetScore(int amount) {
            int diff = Score - amount;
            Score = amount;
            MatchScores[MatchScores.Count - 1] -= diff;
        }
    }
}

