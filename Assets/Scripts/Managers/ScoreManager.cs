using System;
using System.Collections.Generic;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace ToyBox.Managers {
    public class ScoreManager : MonoBehaviour
    {
        private PlayerManager _playerManager => PlayerManager.Instance;
        
       public Dictionary<string, int> PlayerScores;
       public static ScoreManager Instance;
       
       private void Awake() {
           if (Instance == null) {
               Instance = this;
               DontDestroyOnLoad(transform.root);
           } else 
           {
               Destroy(gameObject);
           }
       }

       private void Start()
       {
           PlayerScores = new();
       }

       public void AddScoreDic(string playerName, int score)
       {
              if (PlayerScores.ContainsKey(playerName))
              {
                PlayerScores[playerName] = score;
              }
              else
              {
                PlayerScores.Add(playerName, score);
              }
       }

       public void RemoveScoreDic(string playerName)
       {
           PlayerScores.Remove(playerName);
       }
       
       public void RestScoreDic()
       {
           PlayerScores.Clear();
       }

    }
}
