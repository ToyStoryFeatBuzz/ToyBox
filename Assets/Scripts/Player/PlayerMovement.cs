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
        
        PlayerInputManager _inputManager;
        Rigidbody2D _rb;

        bool _performGroundCheck;
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputManager = GetComponent<PlayerInputManager>();
            _inputManager.JumpEvent.Started += OnJump;
            _inputManager.JumpEvent.Canceled += OnJumpCancel;
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
            
            

            //if (Physics2D.OverlapBox(new Vector2(transform.position.x, transform.position.y - 0.5f),
            //        new Vector2(1, 0.1f), 0f, LayerMask.GetMask("Ground")))
            //{
            //    _isGrounded = true;
            //    _remainJump = _maxJump;
            //}
            //else
            //{
            //    _isGrounded = false;
            //}
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
                _performGroundCheck = false;
            }
        }

        private void OnJumpCancel()
        {
            Debug.Log("JumpCanceled");
            _performGroundCheck = true;
            _rb.gravityScale = _gravity;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector2(transform.position.x, transform.position.y - 0.5f), new Vector2(1f, 0.1f));
        }
    }
}
