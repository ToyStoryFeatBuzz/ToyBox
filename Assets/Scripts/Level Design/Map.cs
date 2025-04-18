using System;
using UnityEngine;

namespace ToyBox.LevelDesign {
    public class Map : MonoBehaviour {
        public string SceneName = "";

        int _playerOnMap = 0;

        public Action OnPlayerOnMapEvent;

        private void OnTriggerEnter2D(Collider2D collision) {
            if (!collision.gameObject.CompareTag("Player")) return;
            _playerOnMap++;
            OnPlayerOnMapEvent.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (!collision.gameObject.CompareTag("Player")) return;
            _playerOnMap--;
            OnPlayerOnMapEvent.Invoke();
        }

        public int GetPlayersOn() => _playerOnMap;
    }
}