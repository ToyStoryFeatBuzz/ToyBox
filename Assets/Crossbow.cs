using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] float _range;
    [SerializeField] GameObject _boltPrefab;
    [SerializeField] bool _shot;

    void Start()
    {
        GameModeManager.Instance.OnRaceStart += () =>
        {
            _shot = false;
        };
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
        bolt.GetComponent<Rigidbody2D>().AddForce(transform.up * 80f, ForceMode2D.Impulse);
        
        Destroy(bolt, 2f);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * _range);
    }
}
