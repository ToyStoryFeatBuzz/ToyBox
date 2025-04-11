using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using ToyBox.Player;
using static ToyBox.Enums;

namespace ToyBox.Managers {
    public class PlayerManager : MonoBehaviour {
        public List<StPlayer> Players = new();

        public static PlayerManager Instance;
        
        public PlayerInputManager PlayerInputManager;
        
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
        }

        public void AddPlayer(PlayerInput player) {
            string name = "Player " + (Players.Count + 1);
            player.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = name;
            InputDevice device = player.user.pairedDevices.First();
            player.gameObject.name = name;
            PlayerStats playerStats = player.gameObject.GetComponent<PlayerStats>();
            
            Players.Add(new StPlayer
                { Name = name, PlayerInput = player, PlayerObject = player.gameObject, Device = device, PlayerStats = playerStats });
        }

        void OnDeviceChange(InputDevice device, InputDeviceChange change) {
            if (change != InputDeviceChange.Disconnected) return;
            foreach (StPlayer player in Players.Where(_player => _player.Device == device)) {
                RemovePlayer(player);
                break;
            }
        }

        void RemovePlayer(StPlayer player) {
            Players.Remove(player);
            RefreshPlayerName();
            if (player.PlayerObject != null) {
                Destroy(player.PlayerObject);
            }
        }

        public StPlayer GetPlayer(GameObject playerGameObject) {
            StPlayer stPlayer = new();
            
            foreach (StPlayer player  in Players.Where(_player => _player.PlayerObject == playerGameObject)) {
                stPlayer = player;
            }
            return stPlayer;
        }

        private void RefreshPlayerName() {
            for (int i = 0; i < Players.Count; i++) {
                StPlayer player = Players[i];
                player.Name = $"Player {i + 1}";
                Players[i].PlayerObject.name = player.Name;
                Players[i] = player;
            }
        }
        
        public void SetPlayerState(GameObject player, EPlayerState state) {
            StPlayer stPlayer = GetPlayer(player);
            stPlayer.PlayerState = state;
        }

        public void SetPlayerState(StPlayer player, EPlayerState state) => player.PlayerState = state;
        
    }

    [Serializable]
    public struct StPlayer {
        public string Name;
        public PlayerInput PlayerInput;
        public GameObject PlayerObject;
        public InputDevice Device;
        public PlayerStats PlayerStats;
        public EPlayerState PlayerState;
    }
}