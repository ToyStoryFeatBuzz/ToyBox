using ToyBox.Player;
using UnityEngine;

namespace ToyBox.Obstacles {
    public class Mine : MonoBehaviour {
        [SerializeField] float _explosionRange;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,_explosionRange);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement)) // Checks if it was a player that hit the mine
            {
                foreach (Collider2D colliderInRange in Physics2D.OverlapCircleAll(transform.position, _explosionRange)) // For each player in the explosion range
                {
                    if (!colliderInRange.gameObject.TryGetComponent(out PlayerMovement playerInRange)) {
                        continue;
                    }
                    playerInRange.gameObject.GetComponent<PlayerEnd>().IsDead = true;
                    playerInRange.ApplyKnockBack((playerInRange.transform.position-transform.position).normalized*30); // Applies a knockback depending on the direction the hit players have from the center of the mine, mimics an explosion
                }
                Destroy(gameObject); //Destroy self after explosion
            }
        }
    }
}