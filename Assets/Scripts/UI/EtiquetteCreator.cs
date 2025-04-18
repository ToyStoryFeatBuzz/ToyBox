using System.Collections.Generic;
using ToyBox.Managers;
using UnityEngine;

public class EtiquetteCreator : MonoBehaviour
{
    List<Player> _players = new List<Player>();
    [SerializeField] GameObject _etiquettePrefab;
    void Start()
    {
        _players = PlayerManager.Instance.Players;
        for (int i = 0; i < _players.Count; i++)
        {
            GameObject _etiquette=Instantiate(_etiquettePrefab,transform);
            _etiquette.GetComponent<Etiquette>().PlayerRef = _players[i];
        }
        
    }
    
    
    
}
