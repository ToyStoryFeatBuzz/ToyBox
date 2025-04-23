using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Obstacles {
    
    public class PlayerKiller : MonoBehaviour
    {
        [SerializeField] GameObject _playerExplosion;
        private void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.TryGetComponent(out PlayerEnd player)) {
                AudioManager.Instance.PlaySFX("PlayerDie",transform.position,1f,0.7f);
                if (_playerExplosion)
                {
                    GameObject playerExplosionVisual = Instantiate(_playerExplosion,transform.position, Quaternion.identity);
                    playerExplosionVisual.GetComponent<BombExplosionVisual>().Player = player;
                }
            }
        }
    }
}
