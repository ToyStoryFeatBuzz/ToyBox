using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.Managers;
using ToyBox.Player;
using UnityEngine;
using static ToyBox.Enums.EPlayerState;

namespace ToyBox.LevelDesign {
    public class DynamicCamera : MonoBehaviour {
        [SerializeField] private float _camSizeMultiplier = 1.1f;
        [SerializeField] private float _minCamSize = 2;
        [SerializeField] private float _maxCamSize = 50;
        [SerializeField] private float _camMovementSpeed = 1;
        [SerializeField] private float _camZoomSpeed = 1;
        PlayerManager _playerManager => PlayerManager.Instance;

        private Camera _mainCam;
        private Vector3 _centerPlayerPos = Vector3.zero;
        private float _camSize = 0f;
        private Vector3 _camTargetPos = Vector3.zero;

        public Transform EditorMapCenter;
        public float EditorCamZoom;
        public Action ActualModeFunction;
        
        List<Managers.Player> _alivePlayers = new();
        
        //Debug
        //[SerializeField] private List<Transform> _cameraObjects=new List<Transform>();
        void Start() {
            _mainCam = Camera.main;
            EditorMapCenter.position = new Vector3(EditorMapCenter.position.x, EditorMapCenter.position.y, -10);
            ActualModeFunction = RaceMode;
        }

        void Update() {
            ActualModeFunction.Invoke();
            _alivePlayers = _playerManager.GetAlivePlayers();
        }

        public void RaceMode() {
            //if (_cameraObjects.Count > 0) 
            if (_playerManager.Players.Count > 0) {
                _centerPlayerPos = Vector3.zero;
                _camSize = 0;
                //foreach (Transform player in _cameraObjects)
                foreach (Managers.Player player in _alivePlayers) {
                    //_centerPlayerPos += player.transform.position;
                    _centerPlayerPos += player.PlayerObject.transform.position;
                }

                if (_alivePlayers.Count > 0) {
                    _centerPlayerPos /= _alivePlayers.Count;
                }
                //_centerPlayerPos/=_cameraObjects.Count;

                //foreach (Transform player in _cameraObjects)
                foreach (Managers.Player player in _alivePlayers) {
                    //float distance = Vector3.Distance(player.transform.position, _centerPlayerPos);
                    float distance = Vector3.Distance(player.PlayerObject.transform.position, _centerPlayerPos);
                    if (_camSize < distance * _camSizeMultiplier) {
                        _camSize = distance * _camSizeMultiplier;
                    }
                }

                _centerPlayerPos = new Vector3(_centerPlayerPos.x, _centerPlayerPos.y, -10);
                _mainCam.transform.position = Vector3.Lerp(_mainCam.transform.position, _centerPlayerPos,
                    Time.deltaTime * _camMovementSpeed);
                _camSize = Mathf.Clamp(_camSize, _minCamSize, _maxCamSize);
                _mainCam.orthographicSize =
                    Mathf.Lerp(_mainCam.orthographicSize, _camSize, Time.deltaTime * _camZoomSpeed);
                ;
            }
        }

        public void EditorMode() {
            _mainCam.transform.position = Vector3.Lerp(_mainCam.transform.position, EditorMapCenter.position,
                Time.deltaTime * _camMovementSpeed);
            _mainCam.orthographicSize =
                Mathf.Lerp(_mainCam.orthographicSize, EditorCamZoom, Time.deltaTime * _camZoomSpeed);
            if (Vector3.Distance(_mainCam.transform.position, EditorMapCenter.position) <= 0.1f) {
                _mainCam.transform.position = EditorMapCenter.position;
            }

            if (Mathf.Abs(_mainCam.orthographicSize - _camSize) <= 0.1f) {
                _mainCam.orthographicSize = _camSize;
            }

        }


    }

}