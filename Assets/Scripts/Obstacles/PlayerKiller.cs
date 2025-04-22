using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Obstacles {
    
    public class PlayerKiller : MonoBehaviour
    {
        [SerializeField] GameObject _playerExplosion;
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent(out PlayerEnd player)) {
                AudioManager.Instance.PlaySFX("PlayerDie");
                if (_playerExplosion) Instantiate(_playerExplosion,transform.position, Quaternion.identity);
                player.SetDeath(); 
            }
        }
    }
}
