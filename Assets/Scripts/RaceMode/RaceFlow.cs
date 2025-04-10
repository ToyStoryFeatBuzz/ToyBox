using System;
using Managers;
using ToyBox.Player;
using UnityEngine;

public class RaceFlow : MonoBehaviour
{
    [SerializeField] Transform _startTransform;
    [SerializeField] Transform _endTransform;

    GameModeSwitcher _gameModeSwitcher;
    PlayerManager _playerManager;

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
            player.PlayerObject.transform.position = _startTransform.position;
            player.PlayerObject.GetComponent<PlayerMovement>().IsDead = false;
        }
    }
}
