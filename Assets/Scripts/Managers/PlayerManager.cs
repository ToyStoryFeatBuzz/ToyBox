using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using ToyBox.Player;
using static ToyBox.Enums;
using ToyBox.Build;
using UnityEditor.Animations;

namespace ToyBox.Managers {
    public class PlayerManager : MonoBehaviour {
        public List<Player> Players = new();

        public static PlayerManager Instance;
        
        public PlayerInputManager PlayerInputManager;
        public List<AnimatorController> AnimationClips;
        private List<AnimatorController> Animators = new();
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(transform.root);
            } else {
                Destroy(gameObject);
            }
        }
        
        private void Start() {
            UnityEngine.InputSystem.InputSystem.onDeviceChange += OnDeviceChange;
            PlayerInputManager = GetComponent<PlayerInputManager>();
            SetNewPlayersEntries(true);
        }

        public void SetNewPlayersEntries(bool possibility)
        {
            PlayerInputManager.enabled = possibility;
        }

        public void SetPlayersMovements(bool activation) {
            foreach (Player player in Players) {
                player.PlayerObject.GetComponent<PlayerEnd>().IsDead = !activation;
            }
        }

        public bool DoesAllPlayersFinishedBuilding()
        {
            return Players.All(player => !player.PlayerObject.GetComponent<PlayerEdition>().IsPlacing());
        }


        public void AddPlayer(PlayerInput player) {
            if(Players.FirstOrDefault(_player => _player.PlayerInput == player) != null){
                return;
            }
            Debug.Log("Player added");
            string name = "Player " + (Players.Count + 1);
            player.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
            InputDevice device = player.user.pairedDevices.First();
            player.gameObject.name = name;
            PlayerStats playerStats = player.gameObject.GetComponent<PlayerStats>();
            
            Animator animator = player.gameObject.GetComponentInChildren<Animator>();
            if (animator != null && AnimationClips.Count >= Players.Count + 1) {
                animator.runtimeAnimatorController = AnimationClips[Players.Count];
            }
            
            Players.Add(new Player
                { Name = name, PlayerInput = player, PlayerObject = player.gameObject, Device = device, PlayerStats = playerStats, PlayerState = EPlayerState.Alive });

            player.transform.parent = transform;
            player.transform.localPosition = Vector3.zero;
        }
        

        void OnDeviceChange(InputDevice device, InputDeviceChange change) {
            if (change != InputDeviceChange.Disconnected) return;
            foreach (Player player in Players.Where(_player => _player.Device == device)) {
                RemovePlayer(player);
                break;
            }
        }

        void RemovePlayer(Player player) {
            Players.Remove(player);
            RefreshPlayerName();
            if (player.PlayerObject != null) {
                Destroy(player.PlayerObject);
            }
        }

        public Player GetPlayer(GameObject playerGameObject) {
            return Players.FirstOrDefault(_player => _player.PlayerObject == playerGameObject);
        }

        private void RefreshPlayerName() {
            for (int i = 0; i < Players.Count; i++) {
                Player player = Players[i];
                player.Name = $"Player {i + 1}"; 
                Players[i].PlayerObject.name = player.Name;
                Players[i] = player;
            }
        }
        
        public void SetPlayerState(GameObject player, EPlayerState state) {
            SetPlayerState(GetPlayer(player), state);
        }

        public void SetPlayerState(Player player, EPlayerState state) {
            player.PlayerState = state;
        }


        public List<Player> GetAlivePlayers() {
            return Players.Where(_player => _player.PlayerState == EPlayerState.Alive).ToList();
        }

        public int GetBestScore() {
            return Players.Max(player => player.PlayerStats.Score);
        }
        
    }

    [Serializable]
    public class Player {
        public string Name;
        public PlayerInput PlayerInput;
        public GameObject PlayerObject;
        public InputDevice Device;
        public PlayerStats PlayerStats;
        public EPlayerState PlayerState;
    }
}