using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Obstacles {
    public class PlayerKiller : MonoBehaviour {
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement)) {
                playerMovement.IsDead = true;
                playerMovement.gameObject.transform.position = new Vector2(-999, -999); //Send the dead out of the map to avoid clutter on the race
            }
        }
    }
}
