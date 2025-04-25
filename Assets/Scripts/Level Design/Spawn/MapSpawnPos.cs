using System;
using System.Collections.Generic;
using System.Linq;
using ToyBox.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapSpawnPos : MonoBehaviour
{
    
    void Start()
    {
        SetPlayersPos();
    }
    public void SetPlayersPos()
    {
        List<Player> ps = new();
        
        for (int i = 0; i <PlayerManager.Instance.Players.Count; i++)
        {
             ps.Add(PlayerManager.Instance.Players[i]);
        }
        
        ps = ps.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < ps.Count; i++)
        {
            Vector3 newPos = transform.GetChild(i).position;
            ps[i].PlayerObject.transform.position = newPos;
        }
    }
}
