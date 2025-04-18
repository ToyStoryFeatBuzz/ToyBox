using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToyBox.Managers;
using UnityEngine.SceneManagement;


public class VoteManager : MonoBehaviour
{
    public VoteZone[] VoteZones;
    
    private PlayerManager _playerManager => PlayerManager.Instance;
    private List<string> _votedPlayers = new List<string>();
    private bool _hasTallied = false;
    private Coroutine _countdownCoroutine;

    public void OnPlayerVoted(string playerId)
    {
        if (_votedPlayers.Contains(playerId)) return;

        _votedPlayers.Add(playerId);
        Debug.Log($"{playerId} a voté. Total votes : {_votedPlayers.Count}/{_playerManager.Players.Count}");

        float majority = _playerManager.Players.Count / 2f;

        if (!_hasTallied && _playerManager.Players.Count >= 2)
        {
            if (_votedPlayers.Count >= _playerManager.Players.Count)
            {
                TallyVotes();
            }
            else if (_votedPlayers.Count > majority)
            {
                if (_countdownCoroutine == null)
                {
                    Debug.Log("⏳ Majorité atteinte. Début du compte à rebours de 15 secondes...");
                    _countdownCoroutine = StartCoroutine(VoteCountdown());
                }
            }
        }
    }

    public void OnPlayerUnvoted(string playerId)
    {
        if (!_votedPlayers.Contains(playerId)) return;

        _votedPlayers.Remove(playerId);
        Debug.Log($"{playerId} a retiré son vote. Total votes : {_votedPlayers.Count}/{_playerManager.Players.Count}");

        float majority = _playerManager.Players.Count / 2f;
        
        if (_votedPlayers.Count <= majority && _countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
            Debug.Log("❌ Majorité perdue. Compte à rebours annulé.");
        }
        
        else if (_votedPlayers.Count > majority && _countdownCoroutine == null)
        {
            Debug.Log("⏳ Majorité rétablie. Redémarrage du compte à rebours de 15 secondes...");
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
            Debug.Log($"⏳ Fin du vote dans {countdown} secondes...");
        }

        if (!_hasTallied)
        {
            Debug.Log("🕒 Temps écoulé. Vote terminé.");
            TallyVotes();
        }
    }

    public void TallyVotes()
    {
        _hasTallied = true;

        List<VoteZone> topZones = new List<VoteZone>();
        int highestVote = 0;
        
        foreach (var zone in VoteZones)
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

            if (topZones.Count == 1)
            {
                winner = topZones[0];
                Debug.Log($"🏆 Map gagnante : {winner.MapName} avec {highestVote} votes !");
            }
            else
            {
                winner = topZones[Random.Range(0, topZones.Count)];
                Debug.Log($"🎲 Égalité entre plusieurs maps. Map choisie au hasard : {winner.MapName} !");
            }

            LoadMap(winner.MapName);
        }
        else
        {
            Debug.LogWarning("⚠️ Aucun vote reçu. Impossible de choisir une map.");
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