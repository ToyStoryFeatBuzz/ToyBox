using ToyBox.Managers;
using UnityEngine;
using static ToyBox.Enums;

namespace ToyBox.Player {
    public class PlayerEnd : MonoBehaviour {
        public bool IsDead;
        
        PlayerManager _playerManager => PlayerManager.Instance;
        
        public void SetDeath() {
            _playerManager.SetPlayerState(gameObject, EPlayerState.Dead);
            IsDead = true;
            gameObject.transform.position = new Vector2(-999, -999); //Send the dead out of the map to avoid clutter on the race
        }

        public void SetWin() {
            _playerManager.SetPlayerState(gameObject, EPlayerState.Finished);
        }
    }
}