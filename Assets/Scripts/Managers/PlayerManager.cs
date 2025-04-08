using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Managers {
    public class PlayerManager : MonoBehaviour {
        public List<PlayerInput> Players = new ();
        
        public void AddPlayer(PlayerInput player) => Players.Add(player);
        
        public void RemovePlayer(PlayerInput player) => Players.Remove(player);
        
        
    }
}