using UnityEngine;

public class BombExplosionVisual : MonoBehaviour
{
    [SerializeField] Animator _animator;

    public void DestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
    
    
    
}
