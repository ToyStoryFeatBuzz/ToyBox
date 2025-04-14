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

        private void Awake() {
            if (!_mouseBody)
                _mouseBody = Instantiate(_mouseBodyPrefab);
            ResetMousePos();
            _mouseBody.position = _mousePos;
            ActivateMouse(false);
        }

        private void ResetMousePos() {
            _mousePos = Camera.main.transform.position;
        }

        public void Move(Vector2 movement) {
            _mousePos += movement * _mouseSensitivity;
            _mouseBody.position = _mousePos;
        }

        public Vector2 Click() {
            return _mousePos;
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