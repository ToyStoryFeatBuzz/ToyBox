using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers {
    public class PlayerManager : MonoBehaviour {
        public List<StPlayer> Players = new ();

        public void AddPlayer(PlayerInput player) {
            string name = "Player " + (Players.Count + 1);
            player.gameObject.name = name;
            Players.Add(new StPlayer { Name = name, PlayerInput = player, PlayerObject = player.gameObject });
        }
        
        
        public void RemovePlayer(PlayerInput player) {
            Debug.Log("AAAAAAAAAAAAA");
            StPlayer? playerToRemove = Players.Find(_player => _player.PlayerInput == player);
            Players.Remove(playerToRemove.Value);
        }
        
        
    }

    [Serializable]
    public struct StPlayer {
        public string Name;
        public PlayerInput PlayerInput;
        public GameObject PlayerObject;
    }
}