using ToyBox.Player;
using Unity.VisualScripting;
using UnityEngine;

public class BombExplosionVisual : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public PlayerEnd Player;

    public void DestroyAfterAnimation()
    {
        if (Player) Player.SetDeath();
        Destroy(gameObject);
    }
    
    
    
}
