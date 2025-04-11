using System;
using ToyBox.Player;
using UnityEngine;

namespace Toybox.Obstacles {
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
                    if (colliderInRange.gameObject.TryGetComponent(out PlayerMovement playerInRange))
                    {
                        playerInRange.IsDead = true;
                        playerInRange.ApplyKnockBack((playerInRange.transform.position-transform.position).normalized*30); // Applies a knockback depending on the direction the hit players have from the center of the mine, mimics an explosion
                        //playerMovement.gameObject.transform.position = new Vector2(-999, -999); // Uncomment if you prefer making all the players disappear first
                    }
                }
                Destroy(this.gameObject); //Destroy self after explosion
            }
        }
    }
}