using ToyBox.Managers;
using ToyBox.Player;
using Unity.VisualScripting;
using UnityEngine;

public class BombExplosionVisual : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public PlayerEnd Player;

    public void DestroyAfterAnimation()
    {
        if (Player && !Player.IsDead && PlayerManager.Instance.GetPlayer(Player.gameObject).PlayerState == ToyBox.Enums.EPlayerState.Alive) Player.SetDeath();
        Destroy(gameObject);
    }
    
    
    
}
