using System;
using ToyBox.Build;
using System.Collections;
using TMPro;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace ToyBox.Managers {
    public class GameModeManager : MonoBehaviour {
        
        [SerializeField] int _pointToWin = 100;
        [SerializeField] CountDown _countDown;
        public static int PointToWin = 100;
        public static GameModeManager Instance { get; private set; }

        private BuildsManager _buildsManager => BuildsManager.Instance;

        private PlayerManager _playerManager => PlayerManager.Instance;
        private LevelKit _levelKit => LevelKit.Instance;
        
        public int NbRounds = 1;
        
        public Action OnRaceStartIntern;
        public Action OnRaceStartExtern;
        
        public Action OnCountDownFinishExtern;
        
        public Action OnRaceEndIntern;
        public Action OnRaceEndExtern;
        
        public Action OnLeaderboardStartIntern;
        public Action OnLeaderboardStartExtern;
        
        public Action OnLeaderboardGraphStartIntern;
        public Action OnLeaderboardGraphStartExtern;
        
        public Action OnLeaderboardFinishIntern;
        public Action OnLeaderboardFinishExtern;
        
        public Action OnBuildStartIntern;
        public Action OnBuildStartExtern;

        public Action OnPreStart;
        
        

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                PointToWin = _pointToWin;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            OnRaceEndIntern += OpenLeaderBoard;
            OnLeaderboardFinishIntern += StartConstructMode;  
            OnLeaderboardGraphStartIntern += EnableLobbyReturnForAllPlayers;
        }


        private void OpenLeaderBoard() {
            _levelKit.StopTimer();
            _levelKit.ToggleUI(false);
            if (_playerManager.GetBestScore() < PointToWin)
            {
                OnLeaderboardStartIntern?.Invoke();
                OnLeaderboardStartExtern?.Invoke();
                NbRounds++;
            }
            else {
                foreach (Player player in _playerManager.Players) {
                    player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
                }
                OnLeaderboardGraphStartIntern?.Invoke();
                OnLeaderboardGraphStartExtern?.Invoke();
                
            }
        }
        
        private void EnableLobbyReturnForAllPlayers()
        {
            foreach (Player player in _playerManager.Players)
            {
                var handler = player.PlayerObject.GetComponent<ReadyUpHandler>();
                if (handler != null)
                {
                    handler.EnableLobbyReturn();
                }
                else
                {
                    Debug.LogWarning($"Aucun ReadyUpHandler trouvé pour {player.PlayerObject.name}");
                }
            }
        }

        

        IEnumerator Countdown(float newTime)
        {
            _countDown.ToggleImage(true);
            float currentTime = newTime;

            while (currentTime > 0)
            {
                _countDown.SetSprites(Mathf.RoundToInt(currentTime));

                yield return new WaitForSeconds(0.1f);
                currentTime -= 0.1f;
            }

            _countDown.ToggleImage(false);
            OnCountdownFinished();
        }

        public void StartCountDown(float newTime)
        {
            StartRaceMode();    
            _playerManager.SetAnimInIddle(true);
            _playerManager.SetPlayersMovements(false);
            
            StartCoroutine(Countdown(newTime));
            foreach (Player player in _playerManager.Players)
            {
                player.PlayerObject.GetComponent<Rigidbody2D>().linearVelocity.Set(0, 0);
            }
            AudioManager.Instance.PlaySFX("RaceStart");
        }

        private void OnCountdownFinished()
        {
            _levelKit.ToggleUI(true);
            _levelKit.StartTimer();
            _playerManager.SetAnimInIddle(false);
            _playerManager.SetPlayersMovements(true);
            StartRaceMode();
            AudioManager.Instance.PlayMusic("RaceMode",0.5f);
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
            OnRaceStartIntern?.Invoke();
            OnRaceStartExtern?.Invoke();
        }

        public void StartConstructMode() {
            _buildsManager.Shuffle();
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = true;
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = true;
            }
            OnBuildStartIntern?.Invoke();
            OnBuildStartExtern?.Invoke();
            AudioManager.Instance.PlayMusic("EditMode");
        }

        public void ReturnToLobby() {
            _playerManager.ResetAllPlayerPositions();
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Race");
                player.PlayerState = Enums.EPlayerState.Alive;
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = false;
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = false;
                player.PlayerObject.GetComponent<SpeedUltimate>().enabled = true;
                player.PlayerStats.ResetScore();
            }
            
            _buildsManager.ClearObjects();

            OnRaceEndExtern = () => {};
            OnBuildStartExtern = () => {};
            OnLeaderboardFinishExtern = () => {};
            OnLeaderboardGraphStartExtern = () => {};
            OnLeaderboardStartExtern = () => {};
            OnRaceStartExtern = () => {};
            
            
            ScoreManager.Instance.ResetRound();
            SceneManager.LoadScene("Lobby");
        }
        
        public int GetPointToWin()
        {
            return PointToWin;
        }
        
    }
}