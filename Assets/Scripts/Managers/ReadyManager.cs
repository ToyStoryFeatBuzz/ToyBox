using System.Collections.Generic;
using ToyBox.Leaderboard;
using ToyBox.Managers;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviour
{
    public static ReadyManager Instance;

    [SerializeField] LeaderboardGraph _leaderboardGraph;
    [SerializeField] Sprite _checkSprite;
    [SerializeField] Sprite _uncheckSprite;
    public List<Image> CheckedImageList = new();
    
    PlayerManager _playerManager => PlayerManager.Instance;
    GameModeManager _gameModeManager => GameModeManager.Instance;
    
    [SerializeField] private List<ReadyUpHandler> readyPlayers = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GameModeManager.Instance.OnLeaderboardGraphStartIntern += PlayerCheckUI;
    }

    public void PlayerSetReady(ReadyUpHandler handler)
    {
        if (!readyPlayers.Contains(handler))
            readyPlayers.Add(handler);
        
        for(int i = 0; i < _playerManager.Players.Count; i++) {
            CheckedImageList[i].sprite = _playerManager.Players[i].ReadyUpHandler.CheckReady() ? _checkSprite : _uncheckSprite;
        }

        if (!AreAllPlayersReady()) {
            return;
        }
        ResetReady();
        _gameModeManager.ReturnToLobby();
        _leaderboardGraph.HideLeaderboard();
    }

    bool AreAllPlayersReady()
    {
        if (_playerManager == null || _playerManager.Players == null || _playerManager.Players.Count == 0)
        {
            return false;
        }

        foreach (Player player in _playerManager.Players)
        {
            if (player.ReadyUpHandler == null)
            {
                return false;
            }
            
            
            if (!player.ReadyUpHandler.CheckReady())
            {
                return false;
            }
        }

        return true;
    }
    
    public void ResetReady()
    {
        foreach (Image image in CheckedImageList) {
            image.sprite = _uncheckSprite;
        }
        foreach (Player player in _playerManager.Players)
        {
            player.ReadyUpHandler.ResetReady();
        }
        readyPlayers.Clear();
    }

    public void PlayerCheckUI()
    {
        for(int i= 0; i < _playerManager.Players.Count; i++)
        {
            CheckedImageList[i].gameObject.SetActive(true);
        }
    }
}
