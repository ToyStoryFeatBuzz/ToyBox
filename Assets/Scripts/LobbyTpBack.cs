using ToyBox.Player;
using UnityEngine;

public class LobbyTpBack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            player.transform.position = Vector3.zero;
        }
    }
}
