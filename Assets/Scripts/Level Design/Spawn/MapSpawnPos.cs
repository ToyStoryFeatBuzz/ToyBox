using System.Collections.Generic;
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
            
            PlayerManager.Instance.Players[i].PlayerObject.transform.position=transform.GetChild(i).position;
        }
    }
}
