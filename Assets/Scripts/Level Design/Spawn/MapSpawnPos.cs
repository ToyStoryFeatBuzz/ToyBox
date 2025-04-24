using ToyBox.Managers;
using UnityEngine;

public class MapSpawnPos : MonoBehaviour
{
    
    void Start()
    {
        SetPlayersPos();
    }
    public void SetPlayersPos()
    {
        for (int i = 0; i <PlayerManager.Instance.Players.Count; i++)
        {
            Vector3 newPos = transform.GetChild(i).position;
            PlayerManager.Instance.Players[i].PlayerObject.transform.position = newPos;
        }
    }
}
