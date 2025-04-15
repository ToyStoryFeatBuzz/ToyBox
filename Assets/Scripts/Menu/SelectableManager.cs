using System;
using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class SelectableManager : MonoBehaviour {
        public static SelectableManager Instance;

        public Selectable CurrentSelectable;
        
        public int SelectableHooveredCount;

        public bool IsSelectableHoovered => SelectableHooveredCount > 0;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }
    }
}