using ToyBox.InputSystem;
using ToyBox.Player;
using UnityEngine;

public class NewPlayerAnimator : MonoBehaviour
{
    GameObject _player;
    Animator _animator;

    #region Component References
    Rigidbody2D _rigidbody;
    PlayerMovement _playerMovement;
    PlayerInputSystem _playerInput;
    #endregion
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = gameObject.transform.parent.gameObject;
        
        _playerMovement = _player.GetComponent<PlayerMovement>();
        _rigidbody = _player.GetComponent<Rigidbody2D>();
        _playerInput = _player.GetComponent<PlayerInputSystem>();

        _playerMovement.OnJumpEvent += StartJumpAnim;
        _playerMovement.OnJumpCancelEvent += CancelJumpAnim;
        
        _animator.SetBool("IsGrounded", true);
    }

    void Update()
    {
        if (_playerInput.MoveValue != 0)
        {
            if (_playerInput.MoveValue > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }

        _animator.SetBool("IsGrounded", _playerMovement.IsGrounded);
        _animator.SetBool("WallSlideLeft", _playerMovement.CanWallJumpLeft);
        _animator.SetBool("WallSlideRight", _playerMovement.CanWallJumpRight);
        if (_playerMovement.CanWallJumpLeft)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_playerMovement.CanWallJumpRight)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void StartJumpAnim()
    {
        _animator.SetBool("IsJumping", true);
    }

    void CancelJumpAnim()
    {
        _animator.SetBool("IsJumping", false);
    }
}
