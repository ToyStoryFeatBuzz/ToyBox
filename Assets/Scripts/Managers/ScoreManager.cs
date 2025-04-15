using System;
using System.Collections.Generic;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Managers {
    public class ScoreManager : MonoBehaviour
    {
       public Dictionary<string, int> PlayerScores;
       public static ScoreManager Instance;
       
       private void Awake() {
           if (Instance == null) {
               Instance = this;
               DontDestroyOnLoad(transform.root);
           } else {
               Destroy(gameObject);
           }
       }

       private void Start()
       {
           PlayerScores = new();
       }

       public void AddScoreDic(string playerName, int score)
       {
           PlayerScores.Add(playerName,score);
       }
       
       public void RestScoreDic()
       {
           PlayerScores.Clear();
       }

    }
}
