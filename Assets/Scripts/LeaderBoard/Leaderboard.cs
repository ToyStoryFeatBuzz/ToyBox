using ToyBox.Managers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ToyBox.Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        private PlayerManager _playerManager => PlayerManager.Instance;

        [Header("UI Match")]
        public GameObject PanelMatchUI;
        public List<GameObject> FillBars;
        public List<TextMeshProUGUI> TextSlots;

        private Dictionary<string, int> _playerScoreDict;

        private void Start()
        {
            HideLeaderboard();
            _playerScoreDict = new();
            CheckPlayers();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                ShowLeaderboard();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                CheckPlayers();
            }
        }

        public void ShowLeaderboard()
        {
            UpdateLeaderboard();
            PanelMatchUI.SetActive(true);
        }

        public void HideLeaderboard()
        {
            PanelMatchUI.SetActive(false);
        }

        public void UpdateLeaderboard()
        {
            CheckPlayers();

            var sortedPlayers = GetSortedPlayers();

            for (int i = 0; i < FillBars.Count && i < TextSlots.Count; i++)
            {
                Transform parent = FillBars[i].transform.parent;

                if (i < sortedPlayers.Count)
                {
                    var (playerName, score) = sortedPlayers[i];
                    Debug.Log($"AFTER SORTING Name: {playerName}, Score: {score}");

                    parent.gameObject.SetActive(true);
                    FillBars[i].GetComponent<Image>().fillAmount = score / 100f;
                    TextSlots[i].text = playerName;
                }
                else
                {
                    parent.gameObject.SetActive(false);
                }
            }
        }

        public void CheckPlayers()
        {
            if (_playerManager.Players.Count < 1)
            {
                Debug.Log("No players found");
                return;
            }

            foreach (Managers.Player player in _playerManager.Players)
            {
                if (!_playerScoreDict.ContainsKey(player.Name))
                {
                    _playerScoreDict.Add(player.Name, Random.Range(1, 100));
                    SimulateMatchScores(player);  // Simule des scores de match
                    Debug.Log($"Player {player.Name} has {_playerScoreDict[player.Name]} points.");
                }
            }
        }

        public void SimulateMatchScores(Managers.Player player)
        {
            List<int> matchScores = new();
            int matchCount = Random.Range(1, 10);

            for (int i = 0; i < matchCount; i++)
            {
                matchScores.Add(Random.Range(1, 10));
            }

            player.PlayerStats.MatchScores = matchScores;
        }

        public List<(string name, int score)> GetSortedPlayers()
        {
            var playerList = new List<(string name, int score)>();

            foreach (var player in _playerManager.Players)
            {
                if (_playerScoreDict.ContainsKey(player.Name))
                {
                    playerList.Add((player.Name, _playerScoreDict[player.Name]));
                }
            }

            return playerList.OrderByDescending(x => x.score).ToList();
        }
    }
}
