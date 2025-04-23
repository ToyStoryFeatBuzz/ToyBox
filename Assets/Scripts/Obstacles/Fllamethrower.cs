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
    {   //Currently broken and causes the game to softlock
        //GameModeManager.Instance.OnRaceStart += () =>
        //{
        //    Animate(true);
        //};
        //
        //GameModeManager.Instance.OnRaceEnd += () =>
        //{
        //    Animate(false);
        //};
    }

    public void Shoot()
    {
        GameObject bolt = Instantiate(_projectile, _nozzle.position, Quaternion.identity);
        bolt.GetComponent<Rigidbody2D>().AddForce(-transform.right * _projectileSpeed, ForceMode2D.Impulse);
        AudioManager.Instance.PlaySFX("BallThrowerLaunch",transform.position,1f);
        Destroy(bolt, 1f);
    }

    private void Animate(bool state)
    {
        _animator?.SetBool("Active", state);
    }
}