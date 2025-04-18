using System.Collections.Generic;
using UnityEngine;
using ToyBox.Managers;

public class VoteZone : MonoBehaviour
{
    public string MapName;
    private List<string> _voters = new List<string>(); 
    private PlayerManager _playerManager => PlayerManager.Instance;
    [SerializeField] VoteManager _voteManager;
    
    public void AddVoter(string playerId)
    {
        _voters.Add(playerId);
        _voteManager?.OnPlayerVoted(playerId);
    }

    public void RemoveVoter(string playerId)
    {
        _voters.Remove(playerId);
        _voteManager.OnPlayerUnvoted(playerId);
    }

    public int GetVoteCount()
    {
        return _voters.Count;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerVote pv = other.GetComponent<PlayerVote>();
        if (pv != null)
        {
            pv.EnterZone(this);
            Debug.Log(pv.name + " has entered vote zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerVote pv = other.GetComponent<PlayerVote>();
        if (pv != null)
        {
            pv.ExitZone(this);
            Debug.Log(_voters.Count);
        }
    }
}