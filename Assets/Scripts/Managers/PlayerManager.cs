using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using ToyBox.Player;

namespace ToyBox.Managers {
    public class PlayerManager : MonoBehaviour {
        public List<StPlayer> Players = new();

        private void Start() {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        public void AddPlayer(PlayerInput player) {
            string name = "Player " + (Players.Count + 1);
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

        public void RemovePlayer(StPlayer player) {
            Players.Remove(player);
            RefreshPlayers();
            if (player.PlayerObject != null) {
                Destroy(player.PlayerObject);
            }
        }

        private void RefreshPlayers() {
            for (int i = 0; i < Players.Count; i++) {
                StPlayer player = Players[i];
                player.Name = $"Player {i + 1}";
                Players[i].PlayerObject.name = player.Name;
                Players[i] = player;
            }
        }
    }

    [Serializable]
    public struct StPlayer {
        public string Name;
        public PlayerInput PlayerInput;
        public GameObject PlayerObject;
        public InputDevice Device;
        public PlayerStats PlayerStats;
    }
}