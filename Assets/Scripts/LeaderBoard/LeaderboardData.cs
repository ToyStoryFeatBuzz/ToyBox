using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Leaderboard {
    public class LeaderboardData : MonoBehaviour {
        public static LeaderboardData Instance { get; private set; }

        private void Awake() {
            if (Instance ==null) {
                Instance = this;
            }
        }

        [Header("Leaderboard")] 
        public GameObject PanelEndMatch;
        
        [Header("Leaderboard Graph")] 
        public GameObject PanelEndGame;
        public GameObject Graph;
        public GameObject Background;
        
        [Header("Both")] 
        public List<PlayerInfo> PlayerInfos;
    }

    [Serializable]
    public class PlayerInfo {
        [Header("Leaderboard")] 
        public Image FillBar;
        public Image FillBarAdd;
        public TextMeshProUGUI TextSlot;

        [Header("Leaderboard Graph")] 
        public TextMeshProUGUI TextName;
        public TextMeshProUGUI TextPoints;
        public LineRenderer LineRenderer;
        public Image ImagePlayer;
    }
}