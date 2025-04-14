using System;
using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] float _range;
    [SerializeField] GameObject _boltPrefab;
    bool _shot;

    void Update()
    {
        
        if (_shot)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, _range);
        if (!hit) return; 
        if (hit.collider.gameObject.TryGetComponent(out PlayerMovement player))
        {
            Shoot();
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
