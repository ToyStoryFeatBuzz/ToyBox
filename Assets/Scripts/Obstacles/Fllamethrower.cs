using ToyBox.Managers;
using UnityEngine;

public class Fllamethrower : MonoBehaviour
{
    [SerializeField] float _range;
    [SerializeField] GameObject _projectile;
    [SerializeField] Transform _nozzle;
    [SerializeField] float _projectileSpeed;
    [SerializeField] Animator _animator;

    void Start()
    {
        GameModeManager.Instance.OnRaceStart += () =>
        {
            _animator.SetBool("Active", true);
        };
        
        GameModeManager.Instance.OnRaceEnd += () =>
        {
            _animator.SetBool("Active", false);
        };
    }

    public void Shoot()
    {
        GameObject bolt = Instantiate(_projectile, _nozzle.position, Quaternion.identity);
        bolt.GetComponent<Rigidbody2D>().AddForce(-transform.right * _projectileSpeed, ForceMode2D.Impulse);
        
        Destroy(bolt, 1f);
    }
}