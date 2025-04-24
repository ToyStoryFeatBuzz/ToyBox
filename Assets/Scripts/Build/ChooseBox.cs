using UnityEngine;

namespace ToyBox.Build {
    public class ChooseBox : MonoBehaviour {
        public static ChooseBox Instance;

        public Transform TR;
        public Transform BL;

        [SerializeField] private Animator _chooseBox;

        public void OpenChooseBox() {
            _chooseBox.SetTrigger("Open");
        }

        private void Awake() {
            Instance = this;
            gameObject.SetActive(false);
        }
    }
}