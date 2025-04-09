using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using ToyBox.Player;
using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    [SerializeField] private float _camSizeMultiplier = 1.1f;
    [SerializeField] private float _minCamSize = 2;
    [SerializeField] private float _maxCamSize = 50;
    [SerializeField] private float _camMovementSpeed = 1;
    [SerializeField] private float _camZoomSpeed = 1;
    [SerializeField] private PlayerManager _playerManager;
    
    private Camera _mainCam;
    private Vector3 _centerPlayerPos = Vector3.zero;
    private float _camSize = 0f;
    private Vector3 _camTargetPos = Vector3.zero;
    
    public Transform EditorMapCenter;
    public float EditorCamZoom;
    public Action ActualModeFunction;
    
    //Debug
    //[SerializeField] private List<Transform> _cameraObjects=new List<Transform>();
    void Start()
    {
        _mainCam = Camera.main;
        EditorMapCenter.position=new Vector3(EditorMapCenter.position.x,EditorMapCenter.position.y,-10);
        ActualModeFunction = RaceMode;
    }
    
    void Update()
    {
        ActualModeFunction.Invoke();
    }

    public void RaceMode()
    {
        //if (_cameraObjects.Count > 0) 
        if(_playerManager.Players.Count>0)
        {
            List<StPlayer> alivePlayers = GetAlivePlayers();
            _centerPlayerPos=Vector3.zero;
            _camSize = 0;
            //foreach (Transform player in _cameraObjects)
            foreach (StPlayer player in alivePlayers)
            {
                //_centerPlayerPos += player.transform.position;
                _centerPlayerPos += player.PlayerObject.transform.position;
            }
            //_centerPlayerPos/=_cameraObjects.Count;
            _centerPlayerPos/=alivePlayers.Count;
            
            //foreach (Transform player in _cameraObjects)
            foreach (StPlayer player in alivePlayers)
            {
                //float distance = Vector3.Distance(player.transform.position, _centerPlayerPos);
                float distance = Vector3.Distance(player.PlayerObject.transform.position,_centerPlayerPos);
                if (_camSize < distance*_camSizeMultiplier)
                {
                    _camSize = distance*_camSizeMultiplier;
                }
            }
            _centerPlayerPos=new Vector3(_centerPlayerPos.x,_centerPlayerPos.y,-10);
            _mainCam.transform.position = Vector3.Lerp(_mainCam.transform.position,_centerPlayerPos,Time.deltaTime*_camMovementSpeed);
            _camSize = Mathf.Clamp(_camSize, _minCamSize, _maxCamSize);
            _mainCam.orthographicSize = Mathf.Lerp(_mainCam.orthographicSize,_camSize,Time.deltaTime*_camZoomSpeed); ;
        }
    }

    public void EditorMode()
    {
        _mainCam.transform.position=Vector3.Lerp(_mainCam.transform.position,EditorMapCenter.position,Time.deltaTime*_camMovementSpeed);
        _mainCam.orthographicSize=Mathf.Lerp(_mainCam.orthographicSize,EditorCamZoom,Time.deltaTime*_camZoomSpeed);
        if (Vector3.Distance(_mainCam.transform.position, EditorMapCenter.position) <= 0.1f)
        {
            _mainCam.transform.position=EditorMapCenter.position;
        }

        if (Mathf.Abs(_mainCam.orthographicSize - _camSize) <= 0.1f)
        {
            _mainCam.orthographicSize = _camSize;
        }

    }

    private List<StPlayer> GetAlivePlayers()
    {
        List<StPlayer> alivePlayers = new List<StPlayer>();
        
        foreach (StPlayer player in _playerManager.Players)
        {
            if (player.PlayerObject.GetComponent<PlayerMovement>().IsDead != true)
            {
                alivePlayers.Add(player);
            }
        }
        
        return alivePlayers;
    }
}
