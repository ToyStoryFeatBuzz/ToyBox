using System;
using ToyBox.InputSystem;
using UnityEngine;

public class SoPlayerAnim : MonoBehaviour
{
    [SerializeField] private float _isMovingThreshold; //Minimum speed at which the player is considered as moving
    
    Animator _animator;
    Rigidbody2D _rigidbody;
    PlayerInputSystem _playerInputSystem;
    void Start()
    {
        _playerInputSystem = GetComponent<PlayerInputSystem>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(_rigidbody.linearVelocity.x) < _isMovingThreshold && _rigidbody.linearVelocity.y < _isMovingThreshold)
        {
            _animator.SetBool("IsWalking", false);
        }
        else
        {
            _animator.SetBool("IsWalking", true);
            
            if (_rigidbody.linearVelocity.x > 0)
            {
                _animator.SetFloat("WalkSpeed", 1);
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                _animator.SetFloat("WalkSpeed", -1);
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    void Update()
    {
        
    }
}
