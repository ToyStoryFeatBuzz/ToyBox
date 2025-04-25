using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using ToyBox.Managers;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class VoteManager : MonoBehaviour
{
    public VoteZone[] VoteZones;
    
    private PlayerManager _playerManager => PlayerManager.Instance;
    private List<string> _votedPlayers = new ();
    private bool _hasTallied;
    private Coroutine _countdownCoroutine;
    [FormerlySerializedAs("oneplayer")] public bool HasOneplayer;
    
    public void OnPlayerVoted(string playerId)
    {
        if (_votedPlayers.Contains(playerId)) return;

        _votedPlayers.Add(playerId);
        
        float majority = _playerManager.Players.Count / 2f;

        if (!_hasTallied && _playerManager.Players.Count >= 2 || !_hasTallied && HasOneplayer == true)
        {
            if (_votedPlayers.Count >= _playerManager.Players.Count)
            {
                TallyVotes();
            }
            else if (_votedPlayers.Count > majority)
            {
                if (_countdownCoroutine == null)
                {
                    _countdownCoroutine = StartCoroutine(VoteCountdown());
                }
            }
        }
    }

    public void OnPlayerUnvoted(string playerId)
    {
        if (!_votedPlayers.Contains(playerId)) return;

        _votedPlayers.Remove(playerId);
        
        float majority = _playerManager.Players.Count / 2f;
        
        if (_votedPlayers.Count <= majority && _countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
        
        else if (_votedPlayers.Count > majority && _countdownCoroutine == null)
        {
            _countdownCoroutine = StartCoroutine(VoteCountdown());
        }
    }
    
    private IEnumerator VoteCountdown()
    {
        float countdown = 15f;

        while (countdown > 0f)
        {
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }

        if (!_hasTallied)
        {
            TallyVotes();
        }
    }

    public void TallyVotes()
    {
        _hasTallied = true;

        List<VoteZone> topZones = new List<VoteZone>();
        int highestVote = 0;
        
        foreach (VoteZone zone in VoteZones)
        {
            int count = zone.GetVoteCount();

            if (count > highestVote)
            {
                highestVote = count;
                topZones.Clear();
                topZones.Add(zone);
            }
            else if (count == highestVote)
            {
                topZones.Add(zone);
            }
        }
        
        if (topZones.Count > 0)
        {
            VoteZone winner;

            winner = topZones.Count == 1 ? topZones[0] : topZones[Random.Range(0, topZones.Count)];
            AudioManager.Instance.StopMusic();
            LoadMap(winner.MapName);
        }
    }

    private void LoadMap(string map) {
        AsyncOperation op =SceneManager.LoadSceneAsync(map);
        if (op != null) {
            op.completed += StartRace;
        }
    }

    void StartRace(AsyncOperation operation) {
        GameModeManager.Instance.StartCountDown(3.5f);
    }

}