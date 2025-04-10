using System;
using Toybox.InputSystem;
using UnityEngine;

namespace ToyBox.Player
{
    public class PlayerMovement : MonoBehaviour
    { 
        #region SERIALIZED VARIABLES
        public bool IsDead;
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
        [SerializeField] LayerMask _groundLayer;
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
        PlayerInputManager _inputManager;
        Rigidbody2D _rb;
        
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _inputManager = GetComponent<PlayerInputManager>();
            _inputManager.OnJumpEvent.Started += OnOnJump;
            _inputManager.OnJumpEvent.Canceled += OnOnJumpCancel;
            _inputManager.OnJumpEvent.Performed += OnOnJumpCancel; // If held too long, cancels the jump, simpler than making some timer
            _remainJump = _maxJump;
        }

        private void FixedUpdate()
        {
            if (!IsDead)
            {


                _rb.AddForceX(_acceleration * _inputManager.MoveValue * Time.fixedDeltaTime, ForceMode2D.Impulse);
                _rb.linearVelocityX = Mathf.Clamp(_rb.linearVelocityX, -_maxSpeed, _maxSpeed);


                if (_inputManager.MoveValue == 0)
                {
                    _rb.AddForceX(-_rb.linearVelocityX * _deceleration * Time.fixedDeltaTime, ForceMode2D.Impulse);
                }

                if (Physics2D.OverlapBox(transform.position+(Vector3)_groundOffset,_groundCheckSize,0,_groundLayer) && _performGroundCheck)
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

        private void OnOnJump()
        {
            if (Physics2D.OverlapBox(transform.position + (Vector3)_leftWallOffset, _leftWallCheckSize, 0,
                    _groundLayer) && !_isGrounded && (_wallJumpDirection != EWallJumpDirection.Left || _canWallJumpOnSameWall))
            {
                _wallJumpDirection = EWallJumpDirection.Left;
                _rb.gravityScale = 1;
                _rb.linearVelocity = Vector2.zero;
                _rb.AddForce(_wallJumpVector*_jumpForce, ForceMode2D.Impulse);
                _remainJump = 1;
                _isGrounded = false;
                _performGroundCheck = false;
            }
            else if (Physics2D.OverlapBox(transform.position + (Vector3)_rightWallOffset, _rightWallCheckSize, 0,
                         _groundLayer) && !_isGrounded && (_wallJumpDirection != EWallJumpDirection.Right || _canWallJumpOnSameWall))
            {
                _wallJumpDirection = EWallJumpDirection.Right;
                _rb.gravityScale = 1;
                _rb.linearVelocity = Vector2.zero;
                _rb.AddForce(new Vector2(-_wallJumpVector.x,_wallJumpVector.y) * _jumpForce, ForceMode2D.Impulse);
                _remainJump = 1;
                _isGrounded = false;
                _performGroundCheck = false;
            }
            else if ((_remainJump != 0 && _isGrounded) && !IsDead)
            {
                _rb.gravityScale = 1;
                _rb.linearVelocityY = 0;
                _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
                _remainJump--;
                _isGrounded = false;
                _performGroundCheck = false; //Little hack to make sure the ground check does not trigger on the first frame after jumping, to prevent triple jumps
            }
            else if (_remainJump != 0)
            {
                _rb.gravityScale = 1;
                _rb.linearVelocityY = 0;
                _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);
                _remainJump = 0;
                _isGrounded = false;
                _performGroundCheck = false;
            }
        }

        private void OnOnJumpCancel()
        {
            _performGroundCheck = true;
            _rb.gravityScale = _gravity;
        }

        //Not used see PlayerKiller.cs
        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if (collision.gameObject.CompareTag("KillObject"))
        //    {
        //        IsDead = true;
        //    }
        //}

        public void ApplyKnockBack(Vector2 knockBackVector)
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

    enum EWallJumpDirection
    {
        Left,
        Right,
        None
    }
}
