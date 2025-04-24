using System.Collections;
using ToyBox.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] float _timeToShow = 3f;
        [SerializeField] private float _maxScore;
        [SerializeField] private float _animationDuration = 2f;
        [SerializeField] private LeaderboardSlotAnimator _slotAnimator;

        private PlayerManager _playerManager => PlayerManager.Instance;
        private ScoreManager _scoreManager => ScoreManager.Instance;
        private GameModeManager _gameModeManager => GameModeManager.Instance;
        private LeaderboardData _leaderboardData => LeaderboardData.Instance;

        private Dictionary<string, (int score, Color color, Sprite sprite)> _playerScoreDict;

        private void Start()
        {
            _maxScore = _gameModeManager.GetPointToWin();
            _gameModeManager.OnLeaderboardStartIntern += ShowLeaderboard;
            HideLeaderboard();
            _playerScoreDict = _scoreManager.PlayerScores;
            CheckPlayers();
        }

        public void ShowLeaderboard()
        {
            _playerManager.SetNewPlayersEntries(false);
            _leaderboardData.PanelEndMatch.SetActive(true);
            StartCoroutine(DelayedUpdateAndShow());
        }

        private IEnumerator DelayedUpdateAndShow()
        {
            yield return null;

            UpdateLeaderboard();
            yield return StartCoroutine(ShowingLeaderboard());
        }

        private IEnumerator ShowingLeaderboard()
        {
            yield return new WaitForSeconds(_timeToShow);
            HideLeaderboard();
            
            _gameModeManager.OnLeaderboardFinishIntern?.Invoke();
            _gameModeManager.OnLeaderboardFinishExtern?.Invoke();
        }

        public void HideLeaderboard()
        {
            _leaderboardData.PanelEndMatch.SetActive(false);
            _scoreManager.ResetRound();
        }

        public void UpdateLeaderboard()
        {
            CheckPlayers();
            List<(string name, int score, Color c, Sprite sprite)> sortedPlayers = GetSortedPlayers();
            List<Transform> activeSlots = new();

            for (int i = 0; i < _leaderboardData.PlayerInfos.Count; i++)
            {
                Transform parent = _leaderboardData.PlayerInfos[i].FillBar.transform.parent;

                if (i < sortedPlayers.Count)
                {
                    (string playerName, int score, Color c, Sprite sprite) = sortedPlayers[i];
                    parent.gameObject.SetActive(true);
                    
                    _leaderboardData.PlayerInfos[i].FillBar.color = c;
                    Color colorAdd = c;
                    colorAdd.a = 0.75f;
                    _leaderboardData.PlayerInfos[i].FillBarAdd.color = colorAdd;

                    
                    _leaderboardData.PlayerInfos[i].TextPoint.text = score.ToString();
                    _leaderboardData.PlayerInfos[i].TextPoint.color = c;
                    _leaderboardData.PlayerInfos[i].TextSlot.text = playerName;
                    
                    _leaderboardData.PlayerInfos[i].FillBar.fillAmount = _leaderboardData.PlayerInfos[i].FillBarAdd.fillAmount;
                    
                    StartCoroutine(AnimateFillBar(_leaderboardData.PlayerInfos[i].FillBarAdd, score / _maxScore));

                    activeSlots.Add(parent);
                }
                else
                {
                    parent.gameObject.SetActive(false);
                }
            }
            
            _slotAnimator.AnimateReorder(activeSlots);
        }


        public void CheckPlayers()
        {
            _playerScoreDict = _scoreManager.PlayerScores;
        }

        private IEnumerator AnimateFillBar(Image fillBar, float targetFill)
        {
            float startFill = fillBar.fillAmount;
            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _animationDuration;
                fillBar.fillAmount = Mathf.Lerp(startFill, targetFill, t);
                yield return null;
            }

            fillBar.fillAmount = targetFill;
        }

        public void ResetLeaderBoard()
        {
            for (int i = 0; i < _leaderboardData.PlayerInfos.Count; i++)
            {
                _leaderboardData.PlayerInfos[i].FillBar.fillAmount = 0;
                _leaderboardData.PlayerInfos[i].FillBarAdd.fillAmount = 0;
                _leaderboardData.PlayerInfos[i].TextSlot.text = "";
                _leaderboardData.PlayerInfos[i].FillBar.color = Color.white;
            }
        }

        public List<(string name, int score, Color c, Sprite sprite)> GetSortedPlayers()
        {
            List<(string name, int score, Color c, Sprite sprite)> playerList = new();

            foreach (Managers.Player player in _playerManager.Players)
            {
                if (_playerScoreDict.ContainsKey(player.Name))
                {
                    playerList.Add((player.Name, _playerScoreDict[player.Name].score, _playerScoreDict[player.Name].color, _playerScoreDict[player.Name].sprite));
                }
            }

            return playerList.OrderByDescending(x => x.score).ToList();
        }
    }
}
