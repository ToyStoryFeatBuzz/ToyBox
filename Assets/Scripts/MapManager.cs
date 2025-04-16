using System.Collections.Generic;
using System.Linq;
using ToyBox.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public List<Map> MapList = new();

    PlayerManager _playerManager => PlayerManager.Instance;

    GameModeManager _gameModeManager => GameModeManager.Instance;

    private void Start()
    {
        foreach (Map map in MapList)
        {
            map.OnPlayerOnMapEvent +=MapTriggered;
        }
    }

    public void MapTriggered() {
        foreach (Map map in MapList.Where(map => map.GetPlayersOn() > 0)) {
            _playerManager.SetNewPlayersEntries(false);
            AsyncOperation op = SceneManager.LoadSceneAsync(map.SceneName);
            if (op != null) {
                op.completed += OnCompleted;
            }
        }
    }

    private void OnCompleted(AsyncOperation operation)
    {
        print("GameManager is "+ (_gameModeManager != null));
        _gameModeManager.StartCountDown(3.5f);
    }
}
