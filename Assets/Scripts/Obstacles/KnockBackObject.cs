using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Obstacles {
    [RequireComponent(typeof(Collider2D))]
    public class KnockBackObject : MonoBehaviour {
        [SerializeField] Vector2 _knockBackDirection;
        [SerializeField] float _knockBackForce;
        [SerializeField] Animator _animator;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerMovement playerMovement)) {
                
                playerMovement.ApplyKnockBack(_knockBackDirection.normalized * _knockBackForce); 
                _animator?.SetTrigger("Bounce");
                AudioManager.Instance.PlaySFX("Trampoline_Bounce",transform.position,volume:4f);
            }
        }
        
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_knockBackDirection.normalized);
        }
    }
}
