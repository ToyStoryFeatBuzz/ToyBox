using System;
using System.Collections.Generic;
using Managers;
using ToyBox.Player;
using UnityEngine;
using Random = UnityEngine.Random;


public class RaceFlow : MonoBehaviour
{
    [SerializeField] Transform _startTransform;
    
    GameModeSwitcher _gameModeSwitcher;
    PlayerManager _playerManager;

    int _finishedPlayers;
    bool _raceStarted;
    
    
    void Start()
    {
        _gameModeSwitcher = GameModeSwitcher.Instance;
        _playerManager = _gameModeSwitcher.gameObject.GetComponent<PlayerManager>();
        _gameModeSwitcher.RaceStart.AddListener(RaceStart);
    }

    private void RaceStart()
    {
        foreach (StPlayer player in _playerManager.Players)
        {
            player.PlayerObject.transform.position = new Vector2(_startTransform.position.x+Random.Range(-2,2), _startTransform.position.y); //Randomizing the start position for now
            player.PlayerObject.GetComponent<PlayerMovement>().IsDead = false;
        }
        _raceStarted = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement playerMovement))
        {
            _finishedPlayers++;
            playerMovement.enabled = false;
        }
    }

    void Update()
    {
        if (_raceStarted)
        {
          if (GetAlivePlayers().Count == _finishedPlayers)
          {
              _gameModeSwitcher.RaceEnd.Invoke();
              Debug.Log("Race ends");
              _raceStarted = false;
          }  
        }
    }

    private List<StPlayer> GetAlivePlayers()
    {
        List<StPlayer> alivePlayers = new ();
        foreach (StPlayer player in _playerManager.Players)
        {
            if (player.PlayerObject.GetComponent<PlayerMovement>().IsDead == false)
            {
                alivePlayers.Add(player);
            }
        }
        return alivePlayers;
    }
}
