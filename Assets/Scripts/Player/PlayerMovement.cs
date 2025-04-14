using System;
using ToyBox.InputSystem;
using UnityEngine;
using UnityEngine.Serialization;
using static ToyBox.Enums;

namespace ToyBox.Player
{
    public class PlayerMovement : MonoBehaviour
    { 
        #region SERIALIZED VARIABLES
        [Header("Movement variables")]
        [SerializeField] float _acceleration;
        [SerializeField] float _deceleration;
        [SerializeField] float _maxSpeed;
        
        [Header("Jump variables")]
        [SerializeField] float _jumpForce;
        [SerializeField] int _maxJump;
        [SerializeField] int _remainJump;
        [SerializeField] Vector2 _wallJumpVector;
        [SerializeField] bool _canWallJumpOnSameWall;
        [SerializeField] float _gravity;
        [SerializeField] LayerMask _platformLayer;
        
        [Space(10)]
        [Header("OverlapBox offsets")]
        [SerializeField] Vector2 _groundOffset;
        [SerializeField] Vector2 _groundCheckSize;
        [Space(5)]
        [SerializeField] Vector2 _leftWallOffset;
        [SerializeField] Vector2 _leftWallCheckSize;
        [Space(5)]
        [SerializeField] Vector2 _rightWallOffset;
        [SerializeField] Vector2 _rightWallCheckSize;
        #endregion
        
        EWallJumpDirection _wallJumpDirection = EWallJumpDirection.None;
        
        bool _isGrounded;
        bool _performGroundCheck;
        PlayerInputSystem _inputSystem;
        Rigidbody2D _rb;
        
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputSystem = GetComponent<PlayerInputSystem>();
            _inputSystem.OnJumpEvent.Started += OnJump;
            _inputSystem.OnJumpEvent.Canceled += OnJumpCancel;
            _inputSystem.OnJumpEvent.Performed += OnJumpCancel; // If held too long, cancels the jump, simpler than making some timer
            _remainJump = _maxJump;
        }

        private void FixedUpdate()
        {
            if (!_inputSystem.IsDead) {
                _rb.AddForceX(_acceleration * _inputSystem.MoveValue * Time.fixedDeltaTime, ForceMode2D.Impulse); //Makes the player move in the direction of the input
                _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -_maxSpeed, _maxSpeed); //Clamps the speed to the max speed


                if (_inputSystem.MoveValue == 0) {
                    _rb.AddForceX(-_rb.linearVelocityX * _deceleration * Time.fixedDeltaTime, ForceMode2D.Impulse); // If there is no input, quickly slow down the player
                }

                if (Physics2D.OverlapBox(transform.position+(Vector3)_groundOffset,_groundCheckSize,0,_platformLayer) && _performGroundCheck) // Ground check
                {
                    _isGrounded = true;
                    _remainJump = _maxJump;
                    _wallJumpDirection = EWallJumpDirection.None;
                }
                else
                {
                    _isGrounded = false;
                }
            }
        }

        private void OnJump()
        {
            if (Physics2D.OverlapBox(transform.position + (Vector3)_leftWallOffset, _leftWallCheckSize, 0,
                    _platformLayer) && !_isGrounded && (_wallJumpDirection != EWallJumpDirection.Left || _canWallJumpOnSameWall)) //Wall jump checks
            {
                _wallJumpDirection = EWallJumpDirection.Left;
                Jump(_wallJumpVector*_jumpForce);
            }
            else if (Physics2D.OverlapBox(transform.position + (Vector3)_rightWallOffset, _rightWallCheckSize, 0, _platformLayer) && !_isGrounded && (_wallJumpDirection != EWallJumpDirection.Right || _canWallJumpOnSameWall))
            {
                _wallJumpDirection = EWallJumpDirection.Right;
                Jump(new Vector2(-_wallJumpVector.x,_wallJumpVector.y) * _jumpForce);
            }
            else if (_remainJump >  0) {
                // If you jump after being in the air without jumping (i.e a jump pad)
                Jump(_jumpForce);
            }
        }

        void Jump(float jumpValue) {
            _rb.gravityScale = 1;
            _rb.linearVelocityY = 0;
            _rb.AddForceY(jumpValue, ForceMode2D.Impulse);
            _remainJump--;
            _isGrounded = false;
            _performGroundCheck = false;
        }
        
        void Jump(Vector2 jumpValue) {
            _rb.gravityScale = 1;
            _rb.linearVelocityY = 0;
            _rb.AddForce(jumpValue, ForceMode2D.Impulse);
            _remainJump -- ;
            _isGrounded = false;
            _performGroundCheck = false;
        }

        private void OnJumpCancel() //Gets automatically called if the player releases the jump input or holds it too long
        {
            _performGroundCheck = true;
            _rb.gravityScale = _gravity;
        }

        public void ApplyKnockBack(Vector2 knockBackVector) // Used for obstacles that knockbacks you
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(knockBackVector, ForceMode2D.Impulse);
        }

        void OnDrawGizmosSelected() //To show the overlap boxes in the editor
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position+(Vector3)_groundOffset, _groundCheckSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(new Vector2(transform.position.x+_leftWallOffset.x,transform.position.y+_leftWallOffset.y), _leftWallCheckSize);
            Gizmos.DrawWireCube(new Vector2(transform.position.x+_rightWallOffset.x,transform.position.y+_rightWallOffset.y), _rightWallCheckSize);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position+(Vector3)_wallJumpVector);
        }
    }


}
