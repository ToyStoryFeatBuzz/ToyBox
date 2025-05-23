using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.Managers;
using UnityEngine;

namespace ToyBox.LevelDesign
{
    public class PlayerCamera : MonoBehaviour
    {
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

        [SerializeField] private RaceFlow _raceFlow;


        List<Managers.Player> _alivePlayers = new();

        [Range(0f, 1f)]
        [HideInInspector]
        public List<float> playersImpact = new List<float> {};


        void Start()
        {
            _mainCam = Camera.main;
            EditorMapCenter.position = new Vector3(EditorMapCenter.position.x, EditorMapCenter.position.y, -10);
            ActualModeFunction = RaceMode;

            GameModeManager.Instance.OnRaceStart += () => { ActualModeFunction = RaceMode; };
            GameModeManager.Instance.OnBuildStart += () => { ActualModeFunction = EditorMode; };
        }

        void Update()
        {
            ActualModeFunction.Invoke();
            _alivePlayers = _playerManager.GetAlivePlayers();
        }


        public void RaceMode()
        {
            if (_playerManager.Players.Count > 0)
            {
                _centerPlayerPos = Vector3.zero;
                _camSize = 0;

                List<Transform> players = _raceFlow ? GetPlayerListOrder() : _alivePlayers.Select(player => player.PlayerObject.transform).ToList();

                foreach (var player in players)
                {
                    _centerPlayerPos += player.position;
                }

                if (players.Count > 0)
                {
                    _centerPlayerPos /= players.Count;
                }

                Vector3 newCenter = _centerPlayerPos;

                for (int i = 0; i < players.Count; i++)
                {
                    var player = players[i];
                    newCenter += (_centerPlayerPos - player.position) * (playersImpact.Count < i ? 0f : playersImpact[i]);

                    float distance = Vector3.Distance(player.position, _centerPlayerPos);
                    if (_camSize < distance * _camSizeMultiplier)
                    {
                        _camSize = distance * _camSizeMultiplier;
                    }
                }

                newCenter = new Vector3(newCenter.x, newCenter.y, -10); ;
                _centerPlayerPos = new Vector3(_centerPlayerPos.x, _centerPlayerPos.y, -10);

                _mainCam.transform.position = Vector3.Lerp(_mainCam.transform.position, newCenter, Time.deltaTime * _camMovementSpeed);

                _camSize = Mathf.Clamp(_camSize, _minCamSize, _maxCamSize);

                _mainCam.orthographicSize = Mathf.Lerp(_mainCam.orthographicSize, _camSize, Time.deltaTime * _camZoomSpeed);
                
            }
        }

        private List<Transform> GetPlayerListOrder()
        {
            var list = _raceFlow.GetPlayersInOrder();

            for (int i = 0; i < list.Count; i++)
            {
                if (!_alivePlayers.Any(_alivePlayer => _alivePlayer.Name == list[i].player))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }

            return list.Select(player => player.t).ToList();
        }

        public void EditorMode()
        {
            _mainCam.transform.position = Vector3.Lerp(_mainCam.transform.position, EditorMapCenter.position,
                Time.deltaTime * _camMovementSpeed);
            _mainCam.orthographicSize = Mathf.Lerp(_mainCam.orthographicSize, EditorCamZoom, Time.deltaTime * _camZoomSpeed);
            if (Vector3.Distance(_mainCam.transform.position, EditorMapCenter.position) <= 0.1f)
            {
                _mainCam.transform.position = EditorMapCenter.position;
            }

            if (Mathf.Abs(_mainCam.orthographicSize - _camSize) <= 0.1f)
            {
                _mainCam.orthographicSize = _camSize;
            }

        }
    }
}
