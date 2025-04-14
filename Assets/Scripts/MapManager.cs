using System;
using System.Collections.Generic;
using ToyBox.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public List<Map> mapList = new();

    public PlayerManager playerManager;

    public GameModeManager gameModeManager;

    private void Start()
    {
        foreach (var map in mapList)
        {
            map.playerOnMapEvent.AddListener(MapTriggered);
        }
    }

    public void MapTriggered()
    {
        foreach (var map in mapList)
        {
            if(map.GetPlayersOn() > 0)
            {
                playerManager.SetNewPlayersEntries(false);
                var op = SceneManager.LoadSceneAsync(map.sceneName);
                op.completed += OnCompleted;
            }
        }
    }

    private void OnCompleted(AsyncOperation operation)
    {
        print("dfhkufh  "+ (gameModeManager != null));
        gameModeManager.StartCountDown(3.5f);
    }
}
