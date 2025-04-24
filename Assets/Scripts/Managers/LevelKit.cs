using System;
using UnityEngine;

namespace ToyBox.Managers {
    public class LevelKit : MonoBehaviour {
        [SerializeField] private GameObject _UI;
        public static LevelKit Instance { get; set; }

        public void ToggleUI(bool toggle) => _UI.SetActive(toggle);
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }
    }
}
