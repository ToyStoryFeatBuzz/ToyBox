using System;
using ToyBox.Player;
using UnityEngine;

public class PlayerAnimationSwitcher : MonoBehaviour
{
    private Animator _animator;
    private PlayerMovement _playerMovement;
    private Rigidbody2D _rigidbody2D;
    
    [SerializeField] private float stillMovingThreshold; //minimal speed for the player to be considered moving
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerMovement = GetComponent<PlayerMovement>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (MathF.Abs(_rigidbody2D.velocity.x) < stillMovingThreshold)
        {
            ChangeAnimatorState(animatorStatesEnum.idle);
        }
        else
        {
            if (_rigidbody2D.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            ChangeAnimatorState(animatorStatesEnum.IsWalking); //not to stay here
        }
    }

    private void ChangeAnimatorState(animatorStatesEnum newState)
    {
        for (int i = 0; i < (int)animatorStatesEnum.GetTheCount; i++)
        {
            animatorStatesEnum _currentBool = (animatorStatesEnum)i;
            string _currentBoolName = _currentBool.ToString();
            
            if (_currentBool == newState) //turns on newState
            {
                _animator.SetBool(_currentBoolName, true);
            }
            else //turns off all the rest
            {
                _animator.SetBool(_currentBoolName, false);
            }
        }
    }

    enum animatorStatesEnum
    {
        IsJumping,
        IsFalling,
        IsRising,
        IsLanding,
        IsWalking,
        ReachedApex,
        OnWall,
        GetTheCount,
        idle,
    }
}