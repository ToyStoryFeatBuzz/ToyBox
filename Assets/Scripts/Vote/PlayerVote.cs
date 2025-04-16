using System;
using UnityEngine;
using ToyBox.Managers;

public class PlayerVote : MonoBehaviour
{
    public string PlayerId;
    private VoteZone _currentZone;
    

    public void EnterZone(VoteZone zone)
    {
        if (_currentZone != null && _currentZone != zone)
        {
            _currentZone.RemoveVoter(PlayerId);
        }

        _currentZone = zone;
        _currentZone.AddVoter(PlayerId);
        Debug.Log(_currentZone);
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