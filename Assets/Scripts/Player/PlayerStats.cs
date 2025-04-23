using System.Collections.Generic;
using ToyBox.Managers;
using UnityEngine;

namespace ToyBox.Player {
    public class PlayerStats : MonoBehaviour
    {
        [field:SerializeField] public int Score {get;  private set; }
        
        public List<int> MatchScores = new();
        public string Name;
        
        private void Start() {
            Name = gameObject.name;
            Debug.Log("Initialisé PlayerStats avec nom : " + Name);
        }
        
        public void AddScore(int amount) {
            Score += amount;
            AddScoreToList(Score);
            Debug.Log(Name + " added score: " + Score);
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
    }
}

