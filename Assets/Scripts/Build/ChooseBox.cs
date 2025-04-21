using UnityEngine;

namespace ToyBox.Build {
    public class ChooseBox : MonoBehaviour {
        public static ChooseBox Instance;

        public Transform TR;
        public Transform BL;

        private void Awake() {
            Instance = this;
            gameObject.SetActive(false);
        }
    }
}