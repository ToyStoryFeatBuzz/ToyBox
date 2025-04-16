using System.Collections;
using ToyBox.Managers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ToyBox.Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] float _timeToShow = 3f;
        private PlayerManager _playerManager => PlayerManager.Instance;

        [Header("UI Match")]
        public GameObject PanelMatchUI;
        public List<GameObject> FillBars;
        public List<TextMeshProUGUI> TextSlots;

        private Dictionary<string, int> _playerScoreDict;
        private ScoreManager _scoreManager => ScoreManager.Instance;
        GameModeManager _gameModeManager => GameModeManager.Instance;

        private void Start()
        {
            _gameModeManager.OnRaceEnd += ShowLeaderboard;
            HideLeaderboard();
            _playerScoreDict = _scoreManager.PlayerScores;
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
            _playerManager.SetNewPlayersEntries(false);
            UpdateLeaderboard();
            PanelMatchUI.SetActive(true);
            StartCoroutine(ShowingLeaderboard());
        }
        
        IEnumerator ShowingLeaderboard()
        {
            yield return new WaitForSeconds(_timeToShow);
            HideLeaderboard();
            _gameModeManager.OnLeaderboardFinish.Invoke();
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
                    FillBars[i].GetComponent<Image>().fillAmount = score / 10f;
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
            _scoreManager.PlayerScores = _playerScoreDict;
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
