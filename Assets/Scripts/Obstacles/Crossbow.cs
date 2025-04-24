using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] float _range;
    [SerializeField] GameObject _boltPrefab;
    [SerializeField] bool _shot;
    [SerializeField] float _shotPower;
    Animator _animator;

    void Start()
    {
        GameModeManager.Instance.OnRaceStartExtern += () =>
        {
            _shot = false;
        };
        _animator = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        
        if (_shot)
        {
            return;
        }
        
        RaycastHit2D[] hitList = Physics2D.RaycastAll(transform.position, transform.up, _range);

        foreach (RaycastHit2D hit in hitList)
        {
            if (hit.collider.gameObject.TryGetComponent(out PlayerMovement player))
            {
                Debug.Log("Hit Player");
                Shoot();
            }
        }
        
    }

    void Shoot()
    {
        _shot = true;
        GameObject bolt = Instantiate(_boltPrefab, transform.position, Quaternion.identity);
        bolt.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        bolt.GetComponent<Rigidbody2D>().AddForce(transform.up * _shotPower, ForceMode2D.Impulse);
        _animator.SetTrigger("Shot");
        Destroy(bolt, 1f);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _range);
    }
}
