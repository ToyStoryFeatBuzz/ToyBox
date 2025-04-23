using System.Collections.Generic;
using ToyBox.Managers;
using ToyBox.InputSystem;
using UnityEngine;

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
            if (!_playerMouse) {
                _playerMouse = GetComponent<PlayerMouse>();
            }
            _playerMouse.ActivateMouse(true);
        }

        private void OnDisable() {
            if (_draggedObject != null) Destroy(_draggedObject);
            _playerMouse.ActivateMouse(false);
        }

        public void SetRandomObject() // Deprecated
        {
            SelectObject(_objectsPrefabs[Random.Range(0, _objectsPrefabs.Count)]);
        }

        public void SelectObject(GameObject go) // Select new object and drag it
        {
            if (_draggedObject != null) Destroy(_draggedObject);

            _draggedObject = Instantiate(go);
        }

        public void Place() // When LMB is pressed
        {
            if (_draggedObject) // Place object if exist
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
            else // Try to get nearest object in the choosing box
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

        public void Rotate(float angle) // Rotate object and spread to offsets
        {
            if (!_draggedObject) return;
            _draggedObject.transform.eulerAngles = new(0, 0, _draggedObject.transform.eulerAngles.z + angle);
            _draggedObject.GetComponent<BuildObject>().RotateOffsets(angle);
            _placeable = _buildsManager.CanPlace(_draggedObject.GetComponent<BuildObject>());
            if (_draggedObject.GetComponent<BuildObject>().IsMovingPlatform)
            {
                _draggedObject.GetComponent<BuildObject>().MovingPart.transform.eulerAngles = new(0, 0, _draggedObject.transform.eulerAngles.z + angle);
                _draggedObject.GetComponent<BuildObject>().MovingPartPlaceholder.transform.eulerAngles = new(0, 0, _draggedObject.transform.eulerAngles.z + angle);
            }
        }

        private void Update()
        {
            if (_playerInputManager.GridMoveDir.magnitude > 0) _playerMouse.Move(_playerInputManager.GridMoveDir); // Apply Mouse/Joystick movements to cursor

            _mousePos = _playerMouse.Click();

            if (_draggedObject == null) return;

            Vector2 targetPos = _mousePos + (Vector2.one * _snapInterval / 2f);

            if (_doesSnap) // Snap cursor to grid
            {
                int x = Mathf.FloorToInt(targetPos.x / _snapInterval);
                int y = Mathf.FloorToInt(targetPos.y / _snapInterval);
                targetPos.Set(x * _snapInterval, y * _snapInterval);
            }

            _draggedObject.transform.position = targetPos;

            if (_lastDifferentPos != targetPos) // If a movement is detected, refresh ability to place
            {
                _placeable = _buildsManager.CanPlace(_draggedObject.GetComponent<BuildObject>());
                _lastDifferentPos = targetPos;
            }
            _placeable = _buildsManager.CanPlace(_draggedObject.GetComponent<BuildObject>());
            _lastDifferentPos = targetPos;
        }
    }
}
