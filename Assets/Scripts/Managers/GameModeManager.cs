using System;
using ToyBox.Build;
using System.Collections;
using TMPro;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToyBox.Managers {
    public class GameModeManager : MonoBehaviour {
        
        [SerializeField] int _pointToWin = 100;
        public static int PointToWin = 100;
        public static GameModeManager Instance { get; private set; }

        private BuildsManager _buildsManager => BuildsManager.Instance;

        private PlayerManager _playerManager => PlayerManager.Instance;
        
        public int nbRounds=0;
        
        public Action OnRaceStartIntern;
        public Action OnRaceStartExtern;
        
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
        
        public TextMeshProUGUI roundsText;
        public TextMeshProUGUI cdText;

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
            if (_playerManager.GetBestScore() < PointToWin)
            {
                OnLeaderboardStartIntern?.Invoke();
                OnLeaderboardStartExtern?.Invoke();
                if (roundsText != null)
                {
                    nbRounds++;
                    roundsText.text = nbRounds.ToString();
                }
                else
                {
                    Debug.LogWarning("Rounds Text is null");
                }

            }
            else {
                //_playerManager.ClampScoreToMax(_pointToWin);
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
                    Debug.Log($"Lobby retour activé pour {player.PlayerObject.name}");
                }
                else
                {
                    Debug.LogWarning($"Aucun ReadyUpHandler trouvé pour {player.PlayerObject.name}");
                }
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
            Debug.Log("Countdown finished");
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
            _buildsManager.Shuffle(0);
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