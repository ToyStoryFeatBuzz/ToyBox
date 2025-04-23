using System.Collections.Generic;
using UnityEngine;
using ToyBox.Managers;
using System.Collections;
using System.Linq;

public class VoteZone : MonoBehaviour
{
    public string MapName;
    private List<string> _voters = new List<string>(); 
    private PlayerManager _playerManager => PlayerManager.Instance;
    [SerializeField] VoteManager _voteManager;

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] List<Sprite> boxSprites = new();

    int opened = 0;
    
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

            opened ++;

            if (opened > 1) return;
            
            StopAllCoroutines();
            StartCoroutine(Open());

            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX("Cardboard_Open",pos:transform.position, volume:2f);
            
        }            
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerVote pv = other.GetComponent<PlayerVote>();
        if (pv != null)
        {
            pv.ExitZone(this);
            Debug.Log(_voters.Count);

            opened--;

            if(opened > 0) return;
            
            StartCoroutine(Close());
            StopAllCoroutines();

            AudioManager.Instance.StopSFX();
            AudioManager.Instance.PlaySFX("Cardboard_Close",pos:transform.position, volume:2f);
        }
    }

    private void Start()
    {
        _spriteRenderer.sprite = boxSprites[boxSprites.Count - 1];
    }

    IEnumerator Open()
    {
        for (int i = 0; i < boxSprites.Count; i++)
        {
            _spriteRenderer.sprite = boxSprites[boxSprites.Count - i - 1];
            yield return new WaitForSeconds(.04f);
        }
    }
    IEnumerator Close()
    {
        for (int i = 0; i < boxSprites.Count; i++)
        {
            _spriteRenderer.sprite = boxSprites[i];
            yield return new WaitForSeconds(.04f);
        }
    }
}