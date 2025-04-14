using ToyBox.InputSystem;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Obstacles {
    public class PlayerKiller : MonoBehaviour {
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent(out PlayerEnd player)) {
                player.SetDeath(); 
            }
        }
    }
}
