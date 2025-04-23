using System.Collections.Generic;
using ToyBox.Managers;
using UnityEngine;
using UnityEngine.Serialization;

using UnityEngine.UI;
using Cursor = UnityEngine.Cursor;


namespace ToyBox.Build {
    public class PlayerMouse : MonoBehaviour {
        Vector2 _mousePos = Vector2.zero;

        [FormerlySerializedAs("mouseBody")] [SerializeField]
         Transform _mouseBody;

        [FormerlySerializedAs("mouseBodyPrefab")] [SerializeField]
         Transform _mouseBodyPrefab;

        [FormerlySerializedAs("mouseSensivity")] [SerializeField]
        float _mouseSensitivity;

        float maxX;
        float maxY;

        Camera cam;
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
            _mouseBody.GetComponentInChildren<Image>().sprite = 
                _playerManager.AnimationClips[_playerManager.Players.Count + 1].CursorSpritesIddle;
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
            _mousePos.Set(Mathf.Clamp(_mousePos.x, -maxX, maxX), Mathf.Clamp(_mousePos.y, -maxY, maxY));
            _mouseBody.position = camPos + _mousePos;
        }

        public Vector2 Click() {
            _mouseBody.GetComponentInChildren<Image>().sprite = 
                _playerManager.AnimationClips[_playerManager.Players.Count + 1].CursorSpritesClic;
            return _mouseBody.position;
        }

        public void ActivateMouse(bool activation) {
            if (!_mouseBody) {
                _mouseBody = Instantiate(_mouseBodyPrefab);
                ResetMousePos();
                _mouseBody.position = _mousePos;
            }

            _mouseBody?.gameObject?.SetActive(activation);
            Cursor.visible = !activation;
        }
    }
    
}