using UnityEngine;
using UnityEngine.Serialization;

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