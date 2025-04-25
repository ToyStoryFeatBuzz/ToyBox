using System;
using ToyBox.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ToyBox.Build {
    public class PlayerMouse : MonoBehaviour {
        Vector2 _mousePos = Vector2.zero;

        [FormerlySerializedAs("mouseBody")] [SerializeField]
        Transform _mouseBody;

        [FormerlySerializedAs("mouseBodyPrefab")] [SerializeField]
        Transform _mouseBodyPrefab;

        [FormerlySerializedAs("mouseSensivity")] [SerializeField]
        float _mouseSensitivity;

        public Action<float> OnMouseInBorderXEvent;
        public Action<float> OnMouseInBorderYEvent;

        float _maxX;
        float _maxY;

        Camera _cam;
        
        Sprite _spriteIdle;
        Sprite _spriteClicked;
        PlayerManager _playerManager => PlayerManager.Instance;

        private void Awake() {
            if (!_mouseBody) {
                _mouseBody = Instantiate(_mouseBodyPrefab);
            }
            ResetMousePos();
            _mouseBody.position = _mousePos;
            ActivateMouse(false);
            _cam = Camera.main;
            _mouseBody.parent = _cam.transform;
           
        }
        
        private void Start()
        {
            _spriteIdle = _playerManager.AnimationClips[_playerManager.Players.Count -1 ].spriteIdlle;
            _spriteClicked = _playerManager.AnimationClips[_playerManager.Players.Count -1 ].spriteClic;
        }


        private void SetMaxPos()
        {
            if (!_cam) {
                _cam = Camera.main;
            }

            _maxX = _cam.orthographicSize * _cam.aspect;
            _maxY = _cam.orthographicSize;
        }


        private void ResetMousePos() {
            _mousePos = Camera.main.transform.position;
        }

        public void Move(Vector2 movement) {
            SetMaxPos();

            Vector2 camPos = _cam.transform.position;
            _mousePos += movement * _mouseSensitivity;

            if (Mathf.Abs(_mousePos.x) > _maxX * 0.95f)
            {
                OnMouseInBorderXEvent.Invoke(Mathf.Sign(_mousePos.x));
            }
            if (Mathf.Abs(_mousePos.y) > _maxY * 0.95f)
            {
                OnMouseInBorderYEvent.Invoke(Mathf.Sign(_mousePos.y));
            }

            _mousePos.Set(Mathf.Clamp(_mousePos.x, -_maxX, _maxX), Mathf.Clamp(_mousePos.y, -_maxY, _maxY));
            _mouseBody.position = camPos + _mousePos;
        }

        public void TriggerClicked()
        {
            _mouseBody.GetChild(0).GetComponent<Image>().sprite = _spriteClicked;
        }

        public void SetPlacingPossibility(bool possibility)
        {
            _mouseBody.GetChild(1).gameObject.SetActive(!possibility);
        }

        public Vector2 Click() {
            return _mouseBody.position;
        }

        public void ActivateMouse(bool activation) {
            if (!_mouseBody) {
                _mouseBody = Instantiate(_mouseBodyPrefab);
                ResetMousePos();
                _mouseBody.position = _mousePos;
                _mouseBody.GetChild(0).GetComponent<Image>().sprite = _spriteIdle;
            }

            _mouseBody?.gameObject?.SetActive(activation);
            Cursor.visible = !activation;
        }
    }

}