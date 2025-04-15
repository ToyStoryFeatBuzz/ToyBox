using System;
using ToyBox.Build;
using System.Collections;
using TMPro;
using ToyBox.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToyBox.Managers {
    public class GameModeManager : MonoBehaviour {
        public static GameModeManager Instance { get; private set; }

        [FormerlySerializedAs("buildsManager")] public BuildsManager _buildsManager;

        private PlayerManager _playerManager => PlayerManager.Instance;

        public Action OnRaceStart;
        public Action OnRaceEnd;

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
            //StartRaceMode();
            OnRaceEnd += StartConstructMode;
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
            _playerManager.SetPlayersMovements(true);
            StartRaceMode();
        }
        
        public void StartRaceMode() {
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Race");
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = false;
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = false;
            }
            OnRaceStart?.Invoke();
        }

        public void StartConstructMode() {
            _buildsManager.Shuffle(0);
            foreach (Player player in _playerManager.Players) {
                player.PlayerInput.currentActionMap = player.PlayerInput.actions.FindActionMap("Construct");
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = true;
                player.PlayerObject.GetComponent<PlayerEdition>().enabled = true;
            }
        }
    }
}