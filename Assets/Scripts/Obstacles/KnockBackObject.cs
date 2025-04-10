using System;
using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Obstacles {
    [RequireComponent(typeof(Collider2D))]
    public class KnockBackObject : MonoBehaviour {
        [SerializeField] Vector2 knockBackDirection;
        [SerializeField] float knockBackForce;

        private void OnCollisionEnter2D(Collision2D collision) {
            if ( collision.gameObject.TryGetComponent(out PlayerMovement playerMovement)) {
                playerMovement.ApplyKnockBack(knockBackDirection.normalized * knockBackForce);
            }
        }

        void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)knockBackDirection.normalized);
        }
    }
}
