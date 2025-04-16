using System.Collections.Generic;
using UnityEngine;

public class VoteZone : MonoBehaviour
{
    public string MapName;
    private HashSet<string> _voters = new HashSet<string>(); 

    public void AddVoter(string playerId)
    {
        _voters.Add(playerId);
    }

    public void RemoveVoter(string playerId)
    {
        _voters.Remove(playerId);
    }

    public int GetVoteCount()
    {
        return _voters.Count;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerVote pv = other.GetComponent<PlayerVote>();
        if (pv != null)
        {
            pv.EnterZone(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerVote pv = other.GetComponent<PlayerVote>();
        if (pv != null)
        {
            pv.ExitZone(this);
        }
    }
}