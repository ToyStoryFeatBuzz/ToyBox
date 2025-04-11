using System.Collections.Generic;
using UnityEngine;

namespace ToyBox.Player {
    public class PlayerStats : MonoBehaviour
    {
        public int Score {get;  private set; }
        
        public List<int> MatchScores = new();

        public void AddScore(int amount) {
            Score += amount;
        }
        
        public void RemoveScore(int amount) {
            Score -= amount;
        }
        
        public void ResetScore() {
            Score = 0;
        }
        
        public void AddScoreToList(int amount) {
            MatchScores.Add(amount);
        }
    }
}

