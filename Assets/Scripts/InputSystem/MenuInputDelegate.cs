using System;
using UnityEngine;

namespace ToyBox.InputSystem {
    public class MenuInputDelegate : MonoBehaviour {
        public MenuPlayerInputManager PlayerInControl;
        public static MenuInputDelegate Instance;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
        }
    }
}