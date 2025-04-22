using System;
using ToyBox.Build;
using System.Collections;
using TMPro;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Managers {
    public class GameModeManager : MonoBehaviour {
        
        [SerializeField] int _pointToWin = 100;
        public static GameModeManager Instance { get; private set; }

        private BuildsManager _buildsManager => BuildsManager.Instance;

        private PlayerManager _playerManager => PlayerManager.Instance;

        public Action OnRaceStart;
        public Action OnRaceEnd;
        public Action OnLeaderboardStart;
        public Action OnLeaderboardGraphStart;
        public Action OnLeaderboardFinish;
        public Action OnBuildStart;

        public TextMeshProUGUI cdText;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            OnRaceEnd += OpenLeaderBoard;
            OnLeaderboardFinish += StartConstructMode;
        }

        private void OpenLeaderBoard() {
            if (_playerManager.GetBestScore() < _pointToWin) {
                OnLeaderboardStart?.Invoke();
            }
            else {
                OnLeaderboardGraphStart?.Invoke();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                OnCountdownFinished();
            }
        }

        IEnumerator Countdown(float newTime)
        {
            float currentTime = newTime;

            while (currentTime > 0)
            {
                 cdText.text = (Mathf.RoundToInt(currentTime)).ToString();

                yield return new WaitForSeconds(0.1f);
                currentTime -= 0.1f;
            }

            cdText.text = "";
            OnCountdownFinished();
        }

        public void StartCountDown(float newTime)
        {
            _playerManager.SetPlayersMovements(false);
            StartCoroutine(Countdown(newTime));

            foreach (Player player in _playerManager.Players)
            {
                player.PlayerObject.GetComponent<Rigidbody2D>().linearVelocity.Set(0, 0);
            }
        }

        private void OnCountdownFinished()
        {
            Debug.Log("Countdown finished");
            _playerManager.SetPlayersMovements(true);
            StartRaceMode();
        }
        
        public void StartRaceMode()
        {
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Race");
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = false;
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = false;
                player.PlayerObject.GetComponent<SpeedUltimate>().enabled = true;
            }
            _playerManager.SetNewPlayersEntries(true);
            OnRaceStart?.Invoke();
        }

        public void StartConstructMode() {
            _buildsManager.Shuffle(0);
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = true;
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = true;
            }
            OnBuildStart?.Invoke();
        }
        
        public int GetPointToWin()
        {
            return _pointToWin;
        }
        
    }
}