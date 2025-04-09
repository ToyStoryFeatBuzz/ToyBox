using System;
using Toybox.InputSystem;
using UnityEngine;

namespace ToyBox.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float _acceleration;
        [SerializeField] float _deceleration;
        [SerializeField] float _maxSpeed;
        [SerializeField] float _jumpForce;
        [SerializeField] int _maxJump;
        [SerializeField] int _remainJump;
        [SerializeField] float _gravity;

        bool _isGrounded;
        bool _performGroundCheck;
        PlayerInputManager _inputManager;
        Rigidbody2D _rb;

        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputManager = GetComponent<PlayerInputManager>();
            _inputManager.JumpEvent.Started += OnJump;
            _inputManager.JumpEvent.Canceled += OnJumpCancel;
            _inputManager.JumpEvent.Performed += OnJumpCancel; // If held too long, cancels the jump, simpler than making some timer
            _remainJump = _maxJump;
        }

        private void FixedUpdate()
        {
            _rb.AddForceX(_acceleration * _inputManager.MoveValue * Time.fixedDeltaTime, ForceMode2D.Impulse);
            _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -_maxSpeed, _maxSpeed);
            

            if (_inputManager.MoveValue == 0)
            {
                _rb.AddForceX(-_rb.linearVelocityX * _deceleration * Time.fixedDeltaTime, ForceMode2D.Impulse);
            }
            
            if (_rb.IsTouchingLayers(LayerMask.GetMask("Ground")) && _performGroundCheck)
            {
                _isGrounded = true;
                _remainJump = _maxJump;
            }
            else
            {
                _isGrounded = false;
            }
        }

        private void OnJump()
        {
            if (_remainJump != 0 || _isGrounded)
            {
                _rb.gravityScale = 1;
                _rb.linearVelocityY = 0;
                _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
                _remainJump--;
                _isGrounded = false;
                _performGroundCheck = false; //Little hack to make sure the ground check does not trigger on the first frame after jumping, to prevent triple jumps
            }
        }

        private void OnJumpCancel()
        {
            _performGroundCheck = true;
            _rb.gravityScale = _gravity;
        }
    }
}
