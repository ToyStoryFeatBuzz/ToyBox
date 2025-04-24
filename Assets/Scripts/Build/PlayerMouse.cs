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

        public Action<float> mouseInBorderXEvent;
        public Action<float> mouseInBorderYEvent;

        float maxX;
        float maxY;

        Camera cam;
        
        Sprite spriteIdle;
        Sprite spriteClicked;
        PlayerManager _playerManager => PlayerManager.Instance;

        private void Awake() {
            if (!_mouseBody) {
                _mouseBody = Instantiate(_mouseBodyPrefab);
            }
            ResetMousePos();
            _mouseBody.position = _mousePos;
            ActivateMouse(false);
            cam = Camera.main;
            _mouseBody.parent = cam.transform;
           
        }
        
        private void Start()
        {
            spriteIdle = _playerManager.AnimationClips[_playerManager.Players.Count -1 ].spriteIdlle;
            spriteClicked = _playerManager.AnimationClips[_playerManager.Players.Count -1 ].spriteClic;
        }


        private void SetMaxPos()
        {
            if (!cam) {
                cam = Camera.main;
            }

            maxX = cam.orthographicSize * cam.aspect;
            maxY = cam.orthographicSize;
        }


        private void ResetMousePos() {
            _mousePos = Camera.main.transform.position;
        }

        public void Move(Vector2 movement) {
            SetMaxPos();

            Vector2 camPos = cam.transform.position;
            _mousePos += movement * _mouseSensitivity;

            if (Mathf.Abs(_mousePos.x) > maxX * 0.95f)
            {
                mouseInBorderXEvent.Invoke(Mathf.Sign(_mousePos.x));
            }
            if (Mathf.Abs(_mousePos.y) > maxY * 0.95f)
            {
                mouseInBorderYEvent.Invoke(Mathf.Sign(_mousePos.y));
            }

            _mousePos.Set(Mathf.Clamp(_mousePos.x, -maxX, maxX), Mathf.Clamp(_mousePos.y, -maxY, maxY));
            _mouseBody.position = camPos + _mousePos;
        }

        public void TriggerClicked()
        {
            _mouseBody.GetComponentInChildren<Image>().sprite = spriteClicked;
        }

        public Vector2 Click() {
            return _mouseBody.position;
        }

        public void ActivateMouse(bool activation) {
            if (!_mouseBody) {
                _mouseBody = Instantiate(_mouseBodyPrefab);
                ResetMousePos();
                _mouseBody.position = _mousePos;
                _mouseBody.GetComponentInChildren<Image>().sprite = spriteIdle;
            }

            _mouseBody?.gameObject?.SetActive(activation);
            Cursor.visible = !activation;
        }
    }

}