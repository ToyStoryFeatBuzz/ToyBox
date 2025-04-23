using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.Leaderboard;
using ToyBox.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviour
{
    public static ReadyManager Instance;

    [SerializeField] LeaderboardGraph _leaderboardGraph;
    [SerializeField] Sprite checkSprite;
    [SerializeField] Sprite uncheckSprite;
    public List<Image> CheckedImageList = new();
    
    PlayerManager _playerManager => PlayerManager.Instance;
    
    [SerializeField] private List<ReadyUpHandler> readyPlayers = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GameModeManager.Instance.OnLeaderboardGraphStart += PlayerCheckUI;
    }

    public void PlayerSetReady(ReadyUpHandler handler)
    {
        if (!readyPlayers.Contains(handler))
            readyPlayers.Add(handler);
        
        for(int i = 0; i < _playerManager.Players.Count; i++)
        {
            if (_playerManager.Players[i].ReadyUpHandler.CheckReady())
            {
                CheckedImageList[i].sprite = checkSprite;
            }
            else
            {
                CheckedImageList[i].sprite = uncheckSprite;
            }
        }

        if (AreAllPlayersReady())
        {
            Debug.Log("Tous les joueurs sont prêts !");
            SceneManager.LoadScene("Lobby");
            _leaderboardGraph.HideLeaderboard();
        }
    }

    bool AreAllPlayersReady()
    {
        if (_playerManager == null || _playerManager.Players == null || _playerManager.Players.Count == 0)
        {
            Debug.LogWarning("Aucun joueur trouvé dans PlayerManager !");
            return false;
        }

        foreach (Player player in _playerManager.Players)
        {
            if (player.ReadyUpHandler == null)
            {
                Debug.LogWarning($"Un joueur n'a pas de ReadyUpHandler !");
                return false;
            }
            
            
            if (!player.ReadyUpHandler.CheckReady())
            {
                return false;
            }
        }

        return true;
    }

    public void PlayerCheckUI()
    {
        for(int i= 0; i < _playerManager.Players.Count; i++)
        {
            CheckedImageList[i].gameObject.SetActive(true);
        }
    }
}
