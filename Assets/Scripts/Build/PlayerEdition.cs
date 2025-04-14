using System.Collections.Generic;
using ToyBox.Managers;
using ToyBox.InputSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace ToyBox.Build {
    public class PlayerEdition : MonoBehaviour
    {
        GameObject _draggedObject;

        [SerializeField] bool _doesSnap;
        [SerializeField] float _snapInterval;

        Vector2 _mousePos;

        [SerializeField] List<GameObject> _objectsPrefabs = new();

        Vector2 _lastDifferentPos = Vector2.zero;
        bool _placeable = false;

        BuildsManager _buildsManager => BuildsManager.Instance;

        PlayerMouse _playerMouse;

        PlayerInputSystem _playerInputManager;

        private void Start()
        {
            _playerMouse = GetComponent<PlayerMouse>();
            _playerInputManager = GetComponent<PlayerInputSystem>();

            _playerInputManager.OnPlaceEvent.Canceled += Place;
            _playerInputManager.OnRotateRightEvent.Performed += () => { Rotate(90); };
            _playerInputManager.OnRotateLeftEvent.Performed += () => { Rotate(-90); };
        }

        public bool IsPlacing() {
            return _draggedObject != null;
        }

        private void OnEnable() {
            //SetRandomObject();
            if (!_playerMouse) {
                _playerMouse = GetComponent<PlayerMouse>();
            }
            _playerMouse.ActivateMouse(true);
        }

        private void OnDisable() {
            if (_draggedObject != null) Destroy(_draggedObject);
            _playerMouse.ActivateMouse(false);
        }

        public void SetRandomObject() {
            SelectObject(_objectsPrefabs[Random.Range(0, _objectsPrefabs.Count)]);
        }

        public void SelectObject(GameObject go) {
            if (_draggedObject != null) Destroy(_draggedObject);

            _draggedObject = Instantiate(go);
        }

        public void Place()
        {
            if (_draggedObject)
            {
                if (_buildsManager.IsSelecting) return;

                BuildObject buildObject = _draggedObject.GetComponent<BuildObject>();

                _placeable = _buildsManager.CanPlace(buildObject);

                if (!_placeable && !buildObject.DoErase) return;

                _placeable = false;

                _draggedObject = null;

                _buildsManager.AddObject(buildObject);


                enabled = false;
                //SetRandomObject();
            }
            else
            {
                Collider2D hit = Physics2D.OverlapCircle(_mousePos, .1f);
                if (hit && hit.transform && hit.transform.GetComponentInParent<BuildObject>())
                {
                    BuildObject obj = hit.transform.GetComponentInParent<BuildObject>();

                    if (obj.IsChosen) return;

                    obj.Pick();
                    _draggedObject = obj.gameObject;
                    _placeable = _buildsManager.CanPlace(_draggedObject.GetComponent<BuildObject>());
                }
            }
        }

        public void Rotate(float angle)
        {
            if (!_draggedObject) return;
            _draggedObject.transform.eulerAngles = new(0, 0, _draggedObject.transform.eulerAngles.z + angle);
            _draggedObject.GetComponent<BuildObject>().RotateOffsets(angle);
            _placeable = _buildsManager.CanPlace(_draggedObject.GetComponent<BuildObject>());
        }

        private void Update()
        {
            if (_playerInputManager.GridMoveDir.magnitude > 0) _playerMouse.Move(_playerInputManager.GridMoveDir);

            _mousePos = _playerMouse.Click();

            if (_draggedObject == null) return;

            Vector2 targetPos = _mousePos + (Vector2.one * _snapInterval / 2f);

            if (_doesSnap)
            {
                int x = Mathf.FloorToInt(targetPos.x / _snapInterval);
                int y = Mathf.FloorToInt(targetPos.y / _snapInterval);
                targetPos.Set(x * _snapInterval, y * _snapInterval);
            }

            _draggedObject.transform.position = targetPos;

            if (_lastDifferentPos == targetPos) {
                return;
            }
            _placeable = _buildsManager.CanPlace(_draggedObject.GetComponent<BuildObject>());
            _lastDifferentPos = targetPos;
        }
    }
}
