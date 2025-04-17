using System;
using UnityEngine;
using ToyBox.Managers;

public class PlayerVote : MonoBehaviour
{
    public string PlayerId;
    private VoteZone _currentZone;


    private void Awake()
    {
        PlayerId = gameObject.name;
    }

    public void EnterZone(VoteZone zone)
    {
        if (_currentZone != null && _currentZone != zone)
        {
            _currentZone.RemoveVoter(PlayerId);
        }
        //PlayerId = _playerManager.
        _currentZone = zone;
        _currentZone.AddVoter(PlayerId);
    }

    public void ExitZone(VoteZone zone)
    {
        if (_currentZone == zone)
        {
            _currentZone.RemoveVoter(PlayerId);
            _currentZone = null;
        }
    }
}