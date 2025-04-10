using ToyBox.Player;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            playerMovement.IsDead = true;
            playerMovement.enabled = false;
        }
    }
}
