using UnityEngine;
using UnityEngine.UI;

namespace ToyBox.Menu {
    public class SelectableManager : MonoBehaviour {
        public static SelectableManager Instance;

        public Selectable CurrentSelectable;

        public bool IsHoovering;
        
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }
    }
}